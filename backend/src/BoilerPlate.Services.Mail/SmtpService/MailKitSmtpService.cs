using BoilerPlate.Core.Concurrency;
using BoilerPlate.Services.Mail.Extensions;
using BoilerPlate.Services.Mail.Message;
using BoilerPlate.Services.Mail.Options.Servers;
using MailKit.Net.Smtp;

namespace BoilerPlate.Services.Mail.SmtpService;

internal class MailKitSmtpService(MailSmtpServerOptions options) : IMailSmtpService, IDisposable
{
    private readonly SemaphoreLocker _semaphoreLocker = new();
    private SmtpClient? _smtpClient;

    public void Dispose()
    {
        if (_smtpClient?.IsConnected == true)
        {
            _smtpClient.Disconnect(quit: true);
        }

        _smtpClient?.Dispose();
    }

    public async Task<string?> Send(MailMessage message, CancellationToken ct = default)
    {
        var response = await _semaphoreLocker.LockAsync(async () =>
        {
            await Connect(ct);
            return await _smtpClient!.SendAsync(message.ToMimeMessage(), ct);
        });

        return response;
    }

    private async Task Connect(CancellationToken ct = default)
    {
        _smtpClient ??= new SmtpClient();

        if (_smtpClient.IsConnected == false)
        {
            if (options.SkipSslCertificateValidation)
            {
                _smtpClient.ServerCertificateValidationCallback = (_, _, _, _) => true;
            }

            await _smtpClient.ConnectAsync(options.Address, options.Port, options.UseSsl, ct);
        }

        if (_smtpClient.IsAuthenticated == false && options is { Username: not null, Password: not null })
        {
            await _smtpClient.AuthenticateAsync(options.Username, options.Password, ct);
        }
    }
}