using BoilerPlate.Core.Serialization;
using BoilerPlate.Services.HealthChecks.Options.Publishers;
using BoilerPlate.Services.Mail.Message;
using BoilerPlate.Services.Mail.SmtpService;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace BoilerPlate.Services.HealthChecks.Publishers;

internal class MailHealthChecksPublisher(MailHealthChecksPublisherOptions options, IMailSmtpService mailSmtpService)
    : BaseHealthChecksPublisher<MailHealthChecksPublisherOptions>(options)
{
    protected override Task PublishInternal(HealthReport report, CancellationToken ct) =>
        mailSmtpService.Send(new MailMessage
        {
            From = [Options.EmailFrom],
            To = [Options.EmailTo],
            Cc = null,
            Bcc = null,
            Subject = "BoilerPlate services health check report",
            Body = JsonConvert.SerializeObject(report, SerializationSettings.Indented)
        }, ct);
}