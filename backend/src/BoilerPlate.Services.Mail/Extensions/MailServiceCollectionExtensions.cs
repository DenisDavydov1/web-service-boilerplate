using BoilerPlate.Core.Extensions;
using BoilerPlate.Services.Mail.ImapService;
using BoilerPlate.Services.Mail.Options;
using BoilerPlate.Services.Mail.Options.Servers;
using BoilerPlate.Services.Mail.SmtpService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoilerPlate.Services.Mail.Extensions;

public static class MailServiceCollectionExtensions
{
    public static void AddMail(this IServiceCollection services, IConfiguration configuration)
    {
        var mailOptions = services.AddServiceOptions<MailOptions>(configuration,
        [
            ("SmtpServers",
            [
                (null, typeof(MailSmtpServerOptions))
            ]),
            ("ImapServers",
            [
                (null, typeof(MailImapServerOptions))
            ])
        ]);

        foreach (var smtpServerOptions in mailOptions.SmtpServers)
        {
            var options = (MailSmtpServerOptions) smtpServerOptions;
            services.AddKeyedTransient<IMailSmtpService>(options.Name,
                (_, _) => new MailKitSmtpService(options));
        }

        foreach (var imapServerOptions in mailOptions.ImapServers)
        {
            var options = (MailImapServerOptions) imapServerOptions;
            services.AddKeyedTransient<IMailImapService>(options.Name,
                (_, _) => new MailKitImapService(options));
        }
    }
}