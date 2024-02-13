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
                details.Status = (int)HttpStatusCode.BadRequest;
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
                details.Status = (int)HttpStatusCode.BadRequest;
                details.Errors.Add(ex.ParamName.ToCamelCase() ?? "Validation Error", [ex.Message]);
                break;
            case BusinessException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                details.Title = "One or more validation errors occurred.";
                details.Status = (int)HttpStatusCode.BadRequest;
                if (ex.ParameterNames?.Any() == true)
                {
                    ex.ParameterNames.ToList().ForEach(p => details.Errors.Add(p.ToCamelCase(), [ex.Message]));
                }
                else
                {
                    details.Errors.Add("Validation Error", [ex.Message]);
                }
                break;
            case EntityNotFoundException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
                details.Title = "Entity Not Found Error";
                details.Status = (int)HttpStatusCode.BadRequest;
                details.Errors.Add(ex.ParamName.ToCamelCase() ?? "Entity Not Found Error", [ex.Message]);
                break;

            #endregion

            #region 401_Unauthorized

            case AuthenticationException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
                details.Title = "Authentication Error";
                details.Status = (int)HttpStatusCode.Unauthorized;
                details.Errors.Add("Authentication Error", [ex.Message]);
                break;
            case SecurityTokenException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
                details.Title = "Security Error";
                details.Status = (int)HttpStatusCode.Unauthorized;
                details.Errors.Add("Security Error", [ex.Message]);
                break;

            #endregion

            #region 403_Forbidden

            case UnauthorizedAccessException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
                details.Title = "Authorization Error";
                details.Status = (int)HttpStatusCode.Forbidden;
                details.Errors.Add("Authorization Error", [ex.Message]);
                break;

            #endregion

            #region 409_Conflict

            case ConflictException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
                details.Title = "Conflict Error";
                details.Status = (int)HttpStatusCode.Conflict;
                details.Errors.Add("Conflict Error", [ex.Message]);
                break;
            case InvalidOperationException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
                details.Title = "Invalid Operation Error";
                details.Status = (int)HttpStatusCode.Conflict;
                details.Errors.Add("Invalid Operation Error", [ex.Message]);
                break;

            #endregion

            #region 422_UnprocessableEntity

            case DbUpdateException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc4918#section-11.2";
                details.Title = "Unprocessable Entity";
                details.Status = (int)HttpStatusCode.UnprocessableEntity;
                details.Errors.Add("Unprocessable Entity", [ex.Message]);
                break;

            #endregion

            #region 501_NotImplemented

            case NotImplementedException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2";
                details.Title = "Not Implemented Error";
                details.Status = (int)HttpStatusCode.NotImplemented;
                details.Errors.Add("Not Implemented Error", [ex.Message]);
                break;

            #endregion

            #region 500_InternalServerError

            case IntegrationException ex:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
                details.Title = "Internal Server Error";
                details.Status = (int)HttpStatusCode.InternalServerError;
                details.Errors.Add("Internal Server Error", [ex.Message]);
                break;
            default:
                details.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1";
                details.Title = "Internal Server Error";
                details.Status = (int)HttpStatusCode.InternalServerError;
                details.Errors.Add("Internal Server Error", [exception.Message]);
                break;

            #endregion
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