using MediatR;
using Microsoft.Extensions.Options;
using BoilerPlate.App.Handlers.Options;
using BoilerPlate.App.Handlers.Utils;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.System.Authentication.Requests;
using BoilerPlate.Data.DTO.System.Authentication.Responses;

namespace BoilerPlate.App.Handlers.RequestHandlers.System.Authentication;

public class GetAccessTokenHandler(
    IUnitOfWork unitOfWork,
    IExceptionFactory exceptionFactory,
    IOptions<JwtOptions> jwtOptions)
    : IRequestHandler<GetAccessTokenDto, JwtTokensDto>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<JwtTokensDto> Handle(GetAccessTokenDto request, CancellationToken ct)
    {
        var user = await unitOfWork.Repository<User>().GetAsync(x => x.Login == request.Login, ct);
        exceptionFactory.ThrowIf<EntityNotFoundException>(
            user == null || user.IsDeleted,
            ExceptionCode.System_Authentication_GetAccessToken_UserNotFound,
            args: [nameof(request.Login)]);

        var isValidPassword = HashingUtils.VerifyBCrypt(request.Password, user!.PasswordHash);
        exceptionFactory.ThrowIf<BusinessException>(
            !isValidPassword,
            ExceptionCode.System_Authentication_GetAccessToken_PasswordInvalid,
            args: [nameof(request.Password)]);

        var accessToken = JwtUtils.GenerateAccessToken(_jwtOptions, user);
        var refreshToken = JwtUtils.GenerateRefreshToken(_jwtOptions, user);

        user.AccessTokenId = accessToken.Claims.GetJti();
        user.AccessTokenExpiresAt = accessToken.Claims.GetExp();
        user.RefreshTokenId = refreshToken.Claims.GetJti();
        user.RefreshTokenExpiresAt = refreshToken.Claims.GetExp();

        await unitOfWork.WithTransactionAsync(() =>
        {
            unitOfWork.Repository<User>().Update(user);
        }, ct);

        return new JwtTokensDto
        {
            AccessToken = JwtUtils.WriteToken(accessToken),
            RefreshToken = JwtUtils.WriteToken(refreshToken)
        };
    }
}