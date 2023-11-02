using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.DTO.System.Authentication.Requests;
using BoilerPlate.Data.DTO.System.Authentication.Responses;

namespace BoilerPlate.App.API.Controllers;

/// <summary>
/// User authentication
/// </summary>
[Route("api/auth")]
public class AuthenticationController : BaseApiController
{
    /// <inheritdoc />
    public AuthenticationController(IMediator mediator, ILogger<AuthenticationController> logger,
        IExceptionFactory exceptionFactory)
        : base(mediator, logger, exceptionFactory)
    {
    }

    /// <summary> Initial authentication </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<JwtTokensDto>> GetAccessTokenAsync(GetAccessTokenDto request, CancellationToken ct)
    {
        var response = await Mediator.Send(request, ct);
        return response;
    }

    /// <summary> Refresh expired access token </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<JwtTokensDto>> RefreshAccessTokenAsync(RefreshAccessTokenDto request,
        CancellationToken ct)
    {
        var response = await Mediator.Send(request, ct);
        return response;
    }
}