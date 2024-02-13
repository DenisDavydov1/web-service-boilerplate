using BoilerPlate.App.Validators.Extensions;
using BoilerPlate.Data.DTO.System.Users.Requests;
using Microsoft.AspNetCore.Http;

namespace BoilerPlate.App.Validators.System.Users;

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