namespace BoilerPlate.Services.HealthChecks.Options.Publishers;

public class MailHealthChecksPublisherOptions : BaseHealthChecksPublisherOptions
{
    public string SmtpServerName { get; set; } = null!;

    public string EmailFrom { get; set; } = null!;

    public string EmailTo { get; set; } = null!;
}