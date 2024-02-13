using FluentValidation;

namespace BoilerPlate.App.Validators.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilderInitial<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithState(_ => ValidationErrorCode.Common_NotEmpty)
            .Length(5, 50).WithState(_ => ValidationErrorCode.Common_Length)
            .Matches("[0-9a-zA-Z]").WithState(_ => ValidationErrorCode.System_Users_PasswordValidCharacters);
}