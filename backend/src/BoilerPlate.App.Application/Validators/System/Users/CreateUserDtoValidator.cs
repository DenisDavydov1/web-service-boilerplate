using FluentValidation;
using Microsoft.AspNetCore.Http;
using BoilerPlate.App.Application.Validators.Extensions;
using BoilerPlate.Data.DTO.System.Users.Requests;

namespace BoilerPlate.App.Application.Validators.System.Users;

public class CreateUserDtoValidator : BaseValidator<RegisterUserDto>
{
    public CreateUserDtoValidator(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        RuleFor(x => x.Login)
            .NotEmpty();

        RuleFor(x => x.Password)
            .Password();

        RuleFor(user => user.Email)
            .EmailAddress().WithState(_ => ValidationErrorCode.Common_EmailAddress)
            .Length(5, 100).WithState(_ => ValidationErrorCode.Common_Length);

        RuleFor(x => x.LanguageCode)
            .NotEmpty().WithState(_ => ValidationErrorCode.Common_NotEmpty)
            .Length(2).WithState(_ => ValidationErrorCode.Common_Length);

        RuleFor(x => x.Role)
            .IsInEnum().WithState(_ => ValidationErrorCode.Common_IsInEnum);
    }
}