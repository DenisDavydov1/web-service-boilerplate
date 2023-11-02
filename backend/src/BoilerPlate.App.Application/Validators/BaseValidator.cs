using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using BoilerPlate.Core.Constants;
using BoilerPlate.Core.Extensions;

namespace BoilerPlate.App.Application.Validators;

public abstract class BaseValidator<TRequest> : AbstractValidator<TRequest>
    where TRequest : class
{
    private readonly string _languageCode;

    protected BaseValidator(IHttpContextAccessor httpContextAccessor) =>
        _languageCode = httpContextAccessor.HttpContext?.User.FindFirstValue(LanguageCodes.ClaimLanguageCode)
                        ?? LanguageCodes.English;

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