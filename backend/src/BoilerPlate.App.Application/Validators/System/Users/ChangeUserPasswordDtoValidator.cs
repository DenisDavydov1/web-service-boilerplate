using Microsoft.AspNetCore.Http;
using BoilerPlate.App.Application.Validators.Extensions;
using BoilerPlate.Data.DTO.System.Users.Requests;

namespace BoilerPlate.App.Application.Validators.System.Users;

public class ChangeUserPasswordDtoValidator : BaseValidator<ChangeUserPasswordDto>
{
    public ChangeUserPasswordDtoValidator(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        RuleFor(x => x.OldPassword)
            .Password();

        RuleFor(x => x.NewPassword)
            .Password();
    }
}