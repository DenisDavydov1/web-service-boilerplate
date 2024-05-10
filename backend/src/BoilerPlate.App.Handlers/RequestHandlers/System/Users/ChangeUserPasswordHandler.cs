using AutoMapper;
using MediatR;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.Users.Requests;
using BoilerPlate.Services.System.Users;

namespace BoilerPlate.App.Handlers.RequestHandlers.System.Users;

public class ChangeUserPasswordHandler(
    IUnitOfWork unitOfWork,
    IExceptionFactory exceptionFactory,
    IMapper mapper,
    IUsersService usersService)
    : IRequestHandler<ChangeUserPasswordDto, IdDto>
{
    public async Task<IdDto> Handle(ChangeUserPasswordDto request, CancellationToken ct)
    {
        var user = await usersService.GetCurrentUser(ct);

        var isOldPasswordValid = HashingUtils.VerifyBCrypt(request.OldPassword, user!.PasswordHash);
        exceptionFactory.ThrowIf<BusinessException>(
            isOldPasswordValid == false,
            ExceptionCode.System_Users_ChangeUserPassword_OldPasswordInvalid,
            args: [nameof(request.OldPassword)]);

        user.PasswordHash = HashingUtils.HashBCrypt(request.NewPassword);
        user.UpdatedBy = usersService.GetCurrentUserId();
        user.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.WithTransactionAsync(() => unitOfWork.Repository<User>().Update(user), ct);

        return mapper.Map<IdDto>(user);
    }
}