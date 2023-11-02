using NJsonSchema;

namespace BoilerPlate.Utils.TsClientGenerator.NameGenerators;

public class InterfaceTypeNameGenerator : ITypeNameGenerator
{
    public string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
    {
        typeNameHint = typeNameHint.Replace("Dto", "DTO");

        return schema.Type switch
        {
            JsonObjectType.String => typeNameHint,
            _ => 'I' + typeNameHint
        };
    }
}