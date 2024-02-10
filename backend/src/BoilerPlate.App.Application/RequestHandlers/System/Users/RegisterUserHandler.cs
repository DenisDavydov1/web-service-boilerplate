using AutoMapper;
using MediatR;
using BoilerPlate.Core.Exceptions.Enums;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Exceptions.Factory;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Abstractions.Enums;
using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.Domain.ValueObjects.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.Users.Requests;
using BoilerPlate.Data.Notifications.System.Users;
using BoilerPlate.Data.Seeds.Constants;

namespace BoilerPlate.App.Application.RequestHandlers.System.Users;

public class RegisterUserHandler : IRequestHandler<RegisterUserDto, IdDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IExceptionFactory _exceptionFactory;

    public RegisterUserHandler(IMapper mapper, IMediator mediator, IUnitOfWork unitOfWork,
        IExceptionFactory exceptionFactory)
    {
        _mapper = mapper;
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _exceptionFactory = exceptionFactory;
    }

    public async Task<IdDto> Handle(RegisterUserDto request, CancellationToken ct)
    {
        var isUserExist = await _unitOfWork.Repository<User>().ExistsAsync(x => x.Login == request.Login, ct);
        _exceptionFactory.ThrowIf<BusinessException>(
            isUserExist,
            ExceptionCode.System_Users_RegisterUser_LoginTaken,
            nameof(request.Login));

        var user = _mapper.Map<User>(request);
        user.Role = UserRole.User;
        user.PasswordHash = HashingUtils.HashBCrypt(request.Password);
        user.SecurityQuestions = request.SecurityQuestions
            .Select(x => new UserSecurityQuestion
            {
                Question = x.Key,
                AnswerHash = HashingUtils.HashBCrypt(x.Value)
            })
            .ToArray();
        user.CreatedBy = SeedConstants.RootUserId;

        await _unitOfWork.WithTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<User>().AddAsync(user, ct);
        }, ct);

        var notification = new UserCreatedNotification { User = user };
        await _mediator.Publish(notification, ct);

        return _mapper.Map<IdDto>(user);
    }
}