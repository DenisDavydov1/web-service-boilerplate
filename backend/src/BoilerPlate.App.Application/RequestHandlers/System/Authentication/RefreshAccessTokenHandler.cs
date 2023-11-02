using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.Extensions.Options;
using BoilerPlate.App.Application.Options;
using BoilerPlate.App.Application.Utils;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.System.Authentication.Requests;
using BoilerPlate.Data.DTO.System.Authentication.Responses;

namespace BoilerPlate.App.Application.RequestHandlers.System.Authentication;

public class RefreshAccessTokenHandler : IRequestHandler<RefreshAccessTokenDto, JwtTokensDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExceptionFactory _exceptionFactory;
    private readonly JwtOptions _jwtOptions;

    public RefreshAccessTokenHandler(IUnitOfWork unitOfWork, IExceptionFactory exceptionFactory,
        IOptions<JwtOptions> jwtOptions)
    {
        _unitOfWork = unitOfWork;
        _exceptionFactory = exceptionFactory;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<JwtTokensDto> Handle(RefreshAccessTokenDto request, CancellationToken ct)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = JwtUtils.GetTokenValidationParameters(_jwtOptions);
        var principal = tokenHandler.ValidateToken(request.RefreshToken, tokenValidationParameters, out _);

        var userId = principal.Claims.GetNameIdentifier();
        var jti = principal.Claims.GetJti();
        var exp = principal.Claims.GetExp();
        _exceptionFactory.ThrowIf<BusinessException>(
            userId == null || jti == null || exp == null,
            ExceptionCode.System_Authentication_RefreshAccessToken_InvalidRefreshToken,
            nameof(request.RefreshToken));

        var user = await _unitOfWork.IdRepository<User>().GetByIdAsync(userId!.Value, ct);
        _exceptionFactory.ThrowIf<EntityNotFoundException>(
            user == null || user.IsDeleted,
            ExceptionCode.System_Authentication_RefreshAccessToken_UserNotFound,
            nameof(request.RefreshToken));

        _exceptionFactory.ThrowIf<BusinessException>(
            user!.RefreshTokenId != jti || user.RefreshTokenExpiresAt != exp,
            ExceptionCode.System_Authentication_RefreshAccessToken_InvalidRefreshToken,
            nameof(request.RefreshToken));

        var accessToken = JwtUtils.GenerateAccessToken(_jwtOptions, user);
        var refreshToken = JwtUtils.GenerateRefreshToken(_jwtOptions, user);

        user.AccessTokenId = accessToken.Claims.GetJti();
        user.AccessTokenExpiresAt = accessToken.Claims.GetExp();
        user.RefreshTokenId = refreshToken.Claims.GetJti();
        user.RefreshTokenExpiresAt = refreshToken.Claims.GetExp();

        await _unitOfWork.WithTransactionAsync(() =>
        {
            _unitOfWork.IdRepository<User>().Update(user);
        }, ct);

        return new JwtTokensDto
        {
            AccessToken = JwtUtils.WriteToken(accessToken),
            RefreshToken = JwtUtils.WriteToken(refreshToken)
        };
    }
}