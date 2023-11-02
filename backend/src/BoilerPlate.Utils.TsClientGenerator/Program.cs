using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.TypeScript;
using BoilerPlate.Utils.TsClientGenerator.NameGenerators;

var swaggerFilePath = args[0];
var tsClientFilePath = args[1];

var document = await OpenApiDocument.FromFileAsync(swaggerFilePath);
var settings = new TypeScriptClientGeneratorSettings
{
    GenerateClientClasses = false,
    GenerateClientInterfaces = false,
    TypeScriptGeneratorSettings =
    {
        PropertyNameGenerator = new CasePreservingPropertyNameGenerator(),
        NullValue = TypeScriptNullValue.Null,
        TypeStyle = TypeScriptTypeStyle.Interface,
        DateTimeType = TypeScriptDateTimeType.Date,
        TypeNameGenerator = new InterfaceTypeNameGenerator()
    }
};

var resolver = new TypeScriptTypeResolver(settings.TypeScriptGeneratorSettings);
var generator = new TypeScriptClientGenerator(document, settings, resolver);
var client = generator.GenerateFile().Replace("  ", " ");

File.WriteAllLines(tsClientFilePath, new List<string> { client });