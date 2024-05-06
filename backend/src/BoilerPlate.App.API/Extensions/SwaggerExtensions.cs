using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using BoilerPlate.App.API.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BoilerPlate.App.API.Extensions;

/// <summary>
/// Swagger settings
/// </summary>
public static class SwaggerExtensions
{
    /// <summary> Add swagger to application </summary>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();
            options.SchemaFilter<SwaggerRequiredSchemaFilter>();
            options.AddTypesToSwaggerSchema();

            options.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Description = "Jwt token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme,
                        },
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "API" });
            options.SwaggerDoc("v2", new OpenApiInfo { Version = "v2", Title = "API" });

            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        services.AddSwaggerGenNewtonsoftSupport();
    }

    /// <summary> Use swagger UI </summary>
    public static void UseSwaggerWithUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(o =>
        {
            o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            o.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
        });
    }

    private static void AddTypesToSwaggerSchema(this SwaggerGenOptions options)
    {
        // options.DocumentFilter<CustomModelDocumentFilter<SomeTypeDto>>();
    }

    private class CustomModelDocumentFilter<T> : IDocumentFilter where T : class
    {
        public void Apply(OpenApiDocument openapiDoc, DocumentFilterContext context) =>
            context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
    }
}
