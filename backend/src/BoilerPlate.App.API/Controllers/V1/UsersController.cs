using Asp.Versioning;
using BoilerPlate.App.API.Attributes;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Requests;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.Users.Requests;
using BoilerPlate.Data.DTO.System.Users.Responses;
using BoilerPlate.Services.System.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoilerPlate.App.API.Controllers.V1;

/// <summary>
/// Users controller
/// </summary>
[ApiVersion(1)]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : BaseApiController
{
    private readonly IUsersService _usersService;

    /// <inheritdoc />
    public UsersController(IMediator mediator, ILogger<UsersController> logger, IExceptionFactory exceptionFactory,
        IUsersService usersService) : base(mediator, logger, exceptionFactory) =>
        _usersService = usersService;

    #region GET

    /// <summary> Get user by ID </summary>
    [HttpGet("{id:guid}")]
    [MinimumRoleAuthorize(UserRole.Moderator)]
    public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var request = new GetByIdRequest<User, UserDto> { Id = id };
        var response = await Mediator.Send(request, ct);
        return response;
    }

    /// <summary> Get current user </summary>
    [HttpGet("current")]
    [MinimumRoleAuthorize(UserRole.Viewer)]
    public async Task<ActionResult<UserDto>> GetCurrentAsync(CancellationToken ct)
    {
        var request = new GetByIdRequest<User, UserDto> { Id = _usersService.GetCurrentUserId() };
        var response = await Mediator.Send(request, ct);
        return response;
    }

    /// <summary> Get all users </summary>
    [HttpGet]
    [MinimumRoleAuthorize(UserRole.Moderator)]
    public async Task<ActionResult<GetAllDto<UserDto>>> GetAllAsync(
        [FromQuery] int? page, [FromQuery] int? resultsPerPage,
        [FromQuery] string? sort, [FromQuery] string? filter,
        CancellationToken ct)
    {
        var request = new GetAllRequest<User, UserDto>
        {
            Page = page,
            ResultsPerPage = resultsPerPage,
            Sort = sort,
            Filter = filter
        };

        var response = await Mediator.Send(request, ct);

        return response;
    }

    #endregion

    #region POST

    /// <summary> Create new user </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<IdDto>> RegisterUserAsync([FromBody] RegisterUserDto request, CancellationToken ct)
    {
        var response = await Mediator.Send(request, ct);
        return Created("api/users", response);
    }

    /// <summary> Log out user </summary>
    [HttpPost("log-out")]
    [MinimumRoleAuthorize(UserRole.Viewer)]
    public async Task<ActionResult> LogOutAsync(CancellationToken ct)
    {
        var request = new LogOutUserRequest();
        await Mediator.Send(request, ct);
        return Ok();
    }

    #endregion

    #region PUT

    /// <summary> Change user password </summary>
    [HttpPut("password")]
    [MinimumRoleAuthorize(UserRole.User)]
    public async Task<ActionResult<IdDto>> ChangePasswordAsync([FromBody] ChangeUserPasswordDto request,
        CancellationToken ct)
    {
        var response = await Mediator.Send(request, ct);
        return response;
    }

    #endregion
}