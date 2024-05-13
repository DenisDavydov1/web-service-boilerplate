using BoilerPlate.Services.Mail.Message;

namespace BoilerPlate.Services.Mail.SmtpService;

public interface IMailSmtpService
{
    Task<string?> Send(MailMessage message, CancellationToken ct = default);
}