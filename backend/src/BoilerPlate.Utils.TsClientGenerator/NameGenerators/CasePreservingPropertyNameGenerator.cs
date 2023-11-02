using NJsonSchema;
using NJsonSchema.CodeGeneration;

namespace BoilerPlate.Utils.TsClientGenerator.NameGenerators;

public class CasePreservingPropertyNameGenerator : IPropertyNameGenerator
{
    public string Generate(JsonSchemaProperty property) => property.Name;
}