using MediatR;
using Microsoft.Extensions.Options;
using BoilerPlate.App.Application.Options;
using BoilerPlate.App.Application.Utils;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.System.Authentication.Requests;
using BoilerPlate.Data.DTO.System.Authentication.Responses;

namespace BoilerPlate.App.Application.RequestHandlers.System.Authentication;

public class GetAccessTokenHandler : IRequestHandler<GetAccessTokenDto, JwtTokensDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExceptionFactory _exceptionFactory;
    private readonly JwtOptions _jwtOptions;

    public GetAccessTokenHandler(IUnitOfWork unitOfWork, IExceptionFactory exceptionFactory,
        IOptions<JwtOptions> jwtOptions)
    {
        _unitOfWork = unitOfWork;
        _exceptionFactory = exceptionFactory;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<JwtTokensDto> Handle(GetAccessTokenDto request, CancellationToken ct)
    {
        var user = await _unitOfWork.Repository<User>().GetAsync(x => x.Login == request.Login, ct);
        _exceptionFactory.ThrowIf<EntityNotFoundException>(
            user == null || user.IsDeleted,
            ExceptionCode.System_Authentication_GetAccessToken_UserNotFound,
            nameof(request.Login));

        var isValidPassword = HashingUtils.VerifyBCrypt(request.Password, user!.PasswordHash);
        _exceptionFactory.ThrowIf<BusinessException>(
            !isValidPassword,
            ExceptionCode.System_Authentication_GetAccessToken_PasswordInvalid,
            nameof(request.Password));

        var accessToken = JwtUtils.GenerateAccessToken(_jwtOptions, user);
        var refreshToken = JwtUtils.GenerateRefreshToken(_jwtOptions, user);

        user.AccessTokenId = accessToken.Claims.GetJti();
        user.AccessTokenExpiresAt = accessToken.Claims.GetExp();
        user.RefreshTokenId = refreshToken.Claims.GetJti();
        user.RefreshTokenExpiresAt = refreshToken.Claims.GetExp();

        await _unitOfWork.WithTransactionAsync(() =>
        {
            _unitOfWork.Repository<User>().Update(user);
        }, ct);

        return new JwtTokensDto
        {
            AccessToken = JwtUtils.WriteToken(accessToken),
            RefreshToken = JwtUtils.WriteToken(refreshToken)
        };
    }
}