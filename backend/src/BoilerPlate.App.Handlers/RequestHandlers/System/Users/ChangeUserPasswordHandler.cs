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

public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPasswordDto, IdDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExceptionFactory _exceptionFactory;
    private readonly IMapper _mapper;
    private readonly IUsersService _usersService;

    public ChangeUserPasswordHandler(IUnitOfWork unitOfWork, IExceptionFactory exceptionFactory, IMapper mapper,
        IUsersService usersService)
    {
        _unitOfWork = unitOfWork;
        _exceptionFactory = exceptionFactory;
        _mapper = mapper;
        _usersService = usersService;
    }

    public async Task<IdDto> Handle(ChangeUserPasswordDto request, CancellationToken ct)
    {
        var user = await _usersService.GetCurrentUser(ct);

        var isOldPasswordValid = HashingUtils.VerifyBCrypt(request.OldPassword, user!.PasswordHash);
        _exceptionFactory.ThrowIf<BusinessException>(
            isOldPasswordValid == false,
            ExceptionCode.System_Users_ChangeUserPassword_OldPasswordInvalid,
            args: [nameof(request.OldPassword)]);

        var s = string.Format("{Name} sdf", "kek");

        user.PasswordHash = HashingUtils.HashBCrypt(request.NewPassword);

        await _unitOfWork.WithTransactionAsync(() => _unitOfWork.Repository<User>().Update(user), ct);

        return _mapper.Map<IdDto>(user);
    }
}