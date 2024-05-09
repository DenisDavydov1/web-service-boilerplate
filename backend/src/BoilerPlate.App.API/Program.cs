using System.Reflection;
using BoilerPlate.App.API.Extensions;
using BoilerPlate.App.API.Middlewares;
using BoilerPlate.App.Handlers.Extensions;
using BoilerPlate.App.Jobs.Extensions;
using BoilerPlate.App.Mappers.Extensions;
using BoilerPlate.App.Validators.Extensions;
using BoilerPlate.Core.Exceptions.Extensions;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.DAL.Extensions;
using BoilerPlate.Data.Seeds.Extensions;
using BoilerPlate.Services.Kafka.Extensions;
using BoilerPlate.Services.System.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServiceProvider();

var configuration = builder.Configuration;

if (builder.Environment.IsLocal() || builder.Environment.IsTest())
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

builder.Services.AddDatabase(configuration);
builder.Services.AddUnitOfWork();
builder.Services.AddMediator();
builder.Services.AddMappers();
builder.Services.AddValidators();
builder.Services.AddAuthentication(configuration);
builder.Services.AddUserRoleAuthorization();
builder.Services.AddControllersWithOptions();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.SetDateTimeFormat();
builder.Services.AddSwagger();
builder.Services.AddExceptions();
builder.Services.AddJobs();
builder.Services.AddHttpLogging(o =>
{
    o.MediaTypeOptions.AddText("application/json");
    o.MediaTypeOptions.AddText("multipart/form-data");
    o.MediaTypeOptions.AddText("application/x-www-form-urlencoded");
    o.LoggingFields = HttpLoggingFields.All;
});

if (configuration.IsFileStorageEnabled())
    builder.Services.AddFileStorage(configuration);

builder.Services.AddSystemServices();

if (configuration.IsKafkaEnabled() && EnvUtils.IsSwaggerGen == false)
    builder.Services.AddKafka(builder.Configuration);

builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<JwtValidationMiddleware>();

var app = builder.Build();

app.UseSwaggerWithUi();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<JwtValidationMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/healthz").WithMetadata(new AllowAnonymousAttribute(), new HttpLoggingAttribute(HttpLoggingFields.None));
app.UseHttpLogging();

if (EnvUtils.IsSwaggerGen == false)
{
    app.Services.UseJobs();
    await app.Services.MigrateDatabase();
    await app.Services.SeedDatabase();
    app.Run();
}