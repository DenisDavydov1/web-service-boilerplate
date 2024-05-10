using BoilerPlate.Core.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace BoilerPlate.App.Validators;

public abstract class BaseValidator<TRequest>(IHttpContextAccessor httpContextAccessor)
    : AbstractValidator<TRequest>
    where TRequest : class
{
    private readonly string _languageCode = httpContextAccessor.GetUserLanguageCode();

    public override ValidationResult Validate(ValidationContext<TRequest> context)
    {
        var validationResult = base.Validate(context);

        if (validationResult.IsValid == false)
        {
            validationResult.Errors.ForEach(error =>
            {
                if (error.CustomState is ValidationErrorCode errorCode)
                {
                    error.ErrorMessage = errorCode.GetText(_languageCode);
                }
            });
            RaiseValidationException(context, validationResult);
        }

        return validationResult;
    }
}