namespace BoilerPlate.Services.HealthChecks.Options.Publishers;

public class EmailHealthChecksPublisherOptions : BaseHealthChecksPublisherOptions
{
    public string Email { get; set; } = null!;
}