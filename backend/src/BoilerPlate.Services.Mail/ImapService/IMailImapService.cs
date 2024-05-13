using BoilerPlate.Services.Mail.Message;

namespace BoilerPlate.Services.Mail.ImapService;

public interface IMailImapService
{
    Task<IEnumerable<MailMessage>> GetMessages(string path, int? skip, int? take, CancellationToken ct = default);

    Task<long> AddMessage(string path, MailMessage message, CancellationToken ct = default);
}