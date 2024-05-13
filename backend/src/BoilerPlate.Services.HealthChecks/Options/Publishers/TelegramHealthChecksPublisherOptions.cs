namespace BoilerPlate.Services.HealthChecks.Options.Publishers;

public class TelegramHealthChecksPublisherOptions : BaseHealthChecksPublisherOptions
{
    public string BotName { get; set; } = null!;

    public string ChatId { get; set; } = null!;
}