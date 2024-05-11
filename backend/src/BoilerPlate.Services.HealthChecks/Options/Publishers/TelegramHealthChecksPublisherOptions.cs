namespace BoilerPlate.Services.HealthChecks.Options.Publishers;

public class TelegramHealthChecksPublisherOptions : BaseHealthChecksPublisherOptions
{
    public string ChatId { get; set; } = null!;
}