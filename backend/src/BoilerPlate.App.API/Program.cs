using System.Reflection;
using BoilerPlate.App.API.Extensions;
using BoilerPlate.App.API.Middlewares;
using BoilerPlate.App.Application.Extensions;
using BoilerPlate.App.Jobs.Extensions;
using BoilerPlate.Core.Exceptions.Extensions;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.DAL.Extensions;
using BoilerPlate.Data.Seeds.Extensions;
using BoilerPlate.Services.Kafka.Extensions;
using BoilerPlate.Services.System.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

if (builder.Environment.IsLocal() || builder.Environment.IsTest())
    builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

builder.Services.AddDatabase(configuration);
builder.Services.AddUnitOfWork();
builder.Services.AddMediator();
builder.Services.AddMapper();
builder.Services.AddValidators();
builder.Services.AddAuthentication(configuration);
builder.Services.AddUserRoleAuthorization();
builder.Services.AddControllersWithNormalizedEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.SetDateTimeFormat();
builder.Services.AddSwagger();
builder.Services.AddExceptions();
builder.Services.AddJobs();

if (configuration.IsFileStorageEnabled())
    builder.Services.AddFileStorage(configuration);

// add boilerPlate.services
builder.Services.AddSystemServices();

if (configuration.IsKafkaEnabled() && EnvUtils.IsSwaggerGen == false)
    builder.Services.AddKafka(builder.Configuration);

// add middlewares
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<JwtValidationMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsLocal())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// use middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<JwtValidationMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

if (EnvUtils.IsSwaggerGen == false)
{
    app.Services.UseJobs();
    await app.Services.MigrateDatabase();
    await app.Services.SeedDatabase();
    app.Run();
}