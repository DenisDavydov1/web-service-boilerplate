using System.Reflection;
using System.Security.Cryptography;
using BoilerPlate.App.API.Extensions;
using BoilerPlate.App.API.Middlewares;
using BoilerPlate.App.Handlers.Extensions;
using BoilerPlate.App.Handlers.Options;
using BoilerPlate.App.Jobs.Extensions;
using BoilerPlate.App.Mappers.Extensions;
using BoilerPlate.App.Validators.Extensions;
using BoilerPlate.Core.Exceptions.Extensions;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.DAL.Extensions;
using BoilerPlate.Data.Seeds.Extensions;
using BoilerPlate.Services.HealthChecks.Extensions;
using BoilerPlate.Services.HealthChecks.Options;
using BoilerPlate.Services.Kafka.Extensions;
using BoilerPlate.Services.Kafka.Options;
using BoilerPlate.Services.Mail.Extensions;
using BoilerPlate.Services.Mail.Options;
using BoilerPlate.Services.System.Extensions;
using BoilerPlate.Services.Telegram.Extensions;
using BoilerPlate.Services.Telegram.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServiceProvider();

var configuration = builder.Configuration;

if (builder.Environment.IsLocal() || builder.Environment.IsTest())
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration));

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

if (configuration.IsServiceEnabled<FileStorageOptions>())
    builder.Services.AddFileStorage(configuration);

builder.Services.AddSystemServices();

if (configuration.IsServiceEnabled<KafkaOptions>() && EnvUtils.IsSwaggerGen == false)
    builder.Services.AddKafka(configuration);

if (configuration.IsServiceEnabled<TelegramOptions>())
    builder.Services.AddTelegram(configuration);

if (configuration.IsServiceEnabled<MailOptions>())
    builder.Services.AddMail(configuration);

if (configuration.IsServiceEnabled<HealthChecksOptions>())
    builder.Services.AddServicesHealthChecks(configuration);

builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<JwtValidationMiddleware>();

var app = builder.Build();

app.UseSwaggerWithUi();
app.UseServicesHealthChecks();
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<JwtValidationMiddleware>();
app.UseAuthorization();
app.MapControllers();

if (EnvUtils.IsSwaggerGen == false)
{
    app.Services.UseJobs();
    await app.Services.MigrateDatabase();
    await app.Services.SeedDatabase();
    app.Run();
}

/// <summary> For integration tests </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
}