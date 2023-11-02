using MediatR;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.System.Users.Requests;
using BoilerPlate.Services.System.Users;

namespace BoilerPlate.App.Application.RequestHandlers.System.Users;

public class LogOutUserHandler : IRequestHandler<LogOutUserRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersService _usersService;

    public LogOutUserHandler(IUnitOfWork unitOfWork, IUsersService usersService)
    {
        _unitOfWork = unitOfWork;
        _usersService = usersService;
    }

    public async Task Handle(LogOutUserRequest request, CancellationToken ct)
    {
        var user = await _usersService.GetCurrentUser(ct);

        user.AccessTokenId = null;
        user.AccessTokenExpiresAt = null;
        user.RefreshTokenId = null;
        user.RefreshTokenExpiresAt = null;

        await _unitOfWork.WithTransactionAsync(() => _unitOfWork.IdRepository<User>().Update(user), ct);
    }
}