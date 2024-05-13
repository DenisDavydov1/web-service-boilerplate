using BoilerPlate.Core.Concurrency;
using BoilerPlate.Services.Mail.Extensions;
using BoilerPlate.Services.Mail.Message;
using BoilerPlate.Services.Mail.Options.Servers;
using MailKit;
using MailKit.Net.Imap;

namespace BoilerPlate.Services.Mail.ImapService;

internal class MailKitImapService(MailImapServerOptions options) : IMailImapService, IDisposable
{
    private readonly SemaphoreLocker _semaphoreLocker = new();
    private ImapClient? _imapClient;

    public void Dispose()
    {
        if (_imapClient?.IsConnected == true)
        {
            _imapClient.Disconnect(quit: true);
        }

        _imapClient?.Dispose();
    }

    public async Task<IEnumerable<MailMessage>> GetMessages(string path, int? skip, int? take,
        CancellationToken ct = default)
    {
        await Connect(ct);
        var mailFolder = await _imapClient!.GetFolderAsync(path, ct);
        await mailFolder.OpenAsync(FolderAccess.ReadOnly, ct);

        var query = mailFolder.OrderByDescending(x => x.Date).AsQueryable();

        if (skip != null)
            query = query.Skip(skip.Value);

        if (take != null)
            query = query.Take(take.Value);

        var messages = query
            .ToArray()
            .Select(x => x.ToMailMessage());

        return messages;
    }

    public async Task<long> AddMessage(string path, MailMessage message, CancellationToken ct = default)
    {
        var id = await _semaphoreLocker.LockAsync(async () =>
        {
            await Connect(ct);
            var mailFolder = await _imapClient!.GetFolderAsync(path, ct);
            return await mailFolder.AppendAsync(message.ToMimeMessage(), MessageFlags.None, ct);
        });

        return id?.Id ?? 0;
    }

    private async Task Connect(CancellationToken ct = default)
    {
        _imapClient ??= new ImapClient();

        if (_imapClient.IsConnected == false)
        {
            if (options.SkipSslCertificateValidation)
            {
                _imapClient.ServerCertificateValidationCallback = (_, _, _, _) => true;
            }

            await _imapClient.ConnectAsync(options.Address, options.Port, options.UseSsl, ct);
        }

        if (_imapClient.IsAuthenticated == false && options is { Username: not null, Password: not null })
        {
            await _imapClient.AuthenticateAsync(options.Username, options.Password, ct);
        }
    }
}