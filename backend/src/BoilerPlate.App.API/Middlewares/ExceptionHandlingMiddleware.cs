using System.Net;
using System.Security.Authentication;
using CaseExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using BoilerPlate.Core.Exceptions.Exceptions;
using BoilerPlate.Core.Serialization;
using Serilog;
using ILogger = Serilog.ILogger;

namespace BoilerPlate.App.API.Middlewares;

/// <summary>
/// Http error handling
/// </summary>
public class ExceptionHandlingMiddleware : IMiddleware
{
    private static readonly ILogger Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .MinimumLevel.Debug()
        .CreateLogger();

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var details = new HttpValidationProblemDetails();

        Logger.Error(exception, "{ExceptionName} occured at the endpoint {EndpointRoute}",
            exception.GetType().Name, GetEndpointRoute(context));

        switch (exception)
        {
            #region 400_BadRequest

            case ValidationException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                details.Title = "One or more validation errors occurred.";
                details.Status = (int) HttpStatusCode.BadRequest;
                details.Detail = ex.Errors.Count() == 1 ?
                    ex.Errors.FirstOrDefault()?.ErrorMessage
                    : "One or more validation errors occurred.";
                ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        k => k.Key.ToCamelCase(),
                        v => v.Select(x => x.ErrorMessage).ToArray())
                    .ToList()
                    .ForEach(x => details.Errors.Add(x.Key, x.Value));
                break;
            case ArgumentException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                details.Title = "One or more validation errors occurred.";
                details.Status = (int) HttpStatusCode.BadRequest;
                details.Detail = ex.Message;
                details.Errors.Add(ex.ParamName?.ToCamelCase() ?? "Validation error", [ex.Message]);
                break;
            case BusinessException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                details.Title = "One or more validation errors occurred.";
                details.Status = (int) HttpStatusCode.BadRequest;
                details.Detail = ex.Message;
                if (ex.ParameterNames?.Any() == true)
                {
                    ex.ParameterNames.ToList().ForEach(p => details.Errors.Add(p.ToCamelCase(), [ex.Message]));
                }
                else
                {
                    details.Errors.Add("Validation error", [ex.Message]);
                }
                break;
            case EntityNotFoundException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
                details.Title = "Entity not found.";
                details.Status = (int) HttpStatusCode.BadRequest;
                details.Detail = ex.Message;
                details.Errors.Add(ex.ParamName?.ToCamelCase() ?? "Entity not found error", [ex.Message]);
                break;

            #endregion

            #region 401_Unauthorized

            case AuthenticationException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
                details.Title = "Unauthorized.";
                details.Status = (int) HttpStatusCode.Unauthorized;
                details.Detail = ex.Message;
                details.Errors.Add("Authentication error", [ex.Message]);
                break;
            case SecurityTokenException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
                details.Title = "Unauthorized.";
                details.Status = (int) HttpStatusCode.Unauthorized;
                details.Detail = ex.Message;
                details.Errors.Add("Security token error", [ex.Message]);
                break;

            #endregion

            #region 403_Forbidden

            case UnauthorizedAccessException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
                details.Title = "Forbidden.";
                details.Status = (int) HttpStatusCode.Forbidden;
                details.Detail = ex.Message;
                details.Errors.Add("Unauthorized access error", [ex.Message]);
                break;

            #endregion

            #region 409_Conflict

            case ConflictException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
                details.Title = "Conflict.";
                details.Status = (int) HttpStatusCode.Conflict;
                details.Detail = ex.Message;
                details.Errors.Add("Conflict error", [ex.Message]);
                break;
            case InvalidOperationException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
                details.Title = "Invalid Operation.";
                details.Status = (int) HttpStatusCode.Conflict;
                details.Detail = ex.Message;
                details.Errors.Add("Invalid operation error", [ex.Message]);
                break;

            #endregion

            #region 422_UnprocessableEntity

            case DbUpdateException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2";
                details.Title = "Unprocessable entity.";
                details.Status = (int) HttpStatusCode.UnprocessableEntity;
                details.Detail = ex.Message;
                details.Errors.Add("Unprocessable entity error", [ex.Message]);
                break;

            #endregion

            #region 501_NotImplemented

            case NotImplementedException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2";
                details.Title = "Not implemented.";
                details.Status = (int) HttpStatusCode.NotImplemented;
                details.Detail = ex.Message;
                details.Errors.Add("Not implemented error", [ex.Message]);
                break;

            #endregion

            #region 500_InternalServerError

            case IntegrationException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
                details.Title = "Internal server error.";
                details.Status = (int) HttpStatusCode.InternalServerError;
                details.Detail = ex.Message;
                details.Errors.Add("Integration error", [ex.Message]);
                break;
            default:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
                details.Title = "Internal server error.";
                details.Status = (int) HttpStatusCode.InternalServerError;
                details.Detail = exception.Message;
                details.Errors.Add("Internal server error", [exception.Message]);
                break;

            #endregion
        }

        if (exception is BaseExceptionWithCode exceptionWithCode)
        {
            details.Extensions.Add("code", exceptionWithCode.Code.ToString());
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = details.Status.Value;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(details, SerializationSettings.Default));
    }

    private static string GetEndpointRoute(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint == null)
        {
            return context.Request.Path.ToString();
        }

        var routePattern = (endpoint as RouteEndpoint)?.RoutePattern.RawText;

        return string.IsNullOrWhiteSpace(routePattern)
            ? context.Request.Path.ToString()
            : routePattern;
    }
}