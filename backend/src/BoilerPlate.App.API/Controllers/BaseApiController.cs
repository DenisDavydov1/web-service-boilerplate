using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BoilerPlate.App.API.Constants;
using BoilerPlate.Core.Exceptions.Factory;

namespace BoilerPlate.App.API.Controllers;

/// <summary>
/// API base controller
/// </summary>
[ApiController, Authorize(Policy = AuthorizationConstants.Policy)]
public abstract class BaseApiController : ControllerBase
{
    /// <summary> Mediator </summary>
    protected readonly IMediator Mediator;

    /// <summary> Logger </summary>
    protected readonly ILogger Logger;

    /// <summary> Exception factory </summary>
    protected readonly IExceptionFactory ExceptionFactory;

    /// <summary> Constructor </summary>
    protected BaseApiController(IMediator mediator, ILogger logger, IExceptionFactory exceptionFactory)
    {
        Mediator = mediator;
        Logger = logger;
        ExceptionFactory = exceptionFactory;
    }
}