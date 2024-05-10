using MediatR;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.System.Users.Requests;
using BoilerPlate.Services.System.Users;

namespace BoilerPlate.App.Handlers.RequestHandlers.System.Users;

public class LogOutUserHandler(IUnitOfWork unitOfWork, IUsersService usersService)
    : IRequestHandler<LogOutUserRequest>
{
    public async Task Handle(LogOutUserRequest request, CancellationToken ct)
    {
        var user = await usersService.GetCurrentUser(ct);

        user.AccessTokenId = null;
        user.AccessTokenExpiresAt = null;
        user.RefreshTokenId = null;
        user.RefreshTokenExpiresAt = null;

        await unitOfWork.WithTransactionAsync(() => unitOfWork.Repository<User>().Update(user), ct);
    }
}