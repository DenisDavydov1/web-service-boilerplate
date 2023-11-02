using System.Runtime.CompilerServices;
using CaseExtensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BoilerPlate.App.API.Swagger;

/// <summary>
/// https://stackoverflow.com/questions/53319588/how-to-mark-a-property-as-required-in-swagger-without-asp-net-model-validation
/// </summary>
public class SwaggerRequiredSchemaFilter : ISchemaFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
        {
            return;
        }

        var properties = context.Type.GetProperties();

        foreach (var schemaProp in schema.Properties)
        {
            var codeProp = properties.SingleOrDefault(x => x.Name.ToCamelCase() == schemaProp.Key);
            if (codeProp == null) continue;

            var isRequired = Attribute.IsDefined(codeProp, typeof(RequiredMemberAttribute));
            if (!isRequired) continue;

            schemaProp.Value.Nullable = false;
            _ = schema.Required.Add(schemaProp.Key);
        }
    }
}