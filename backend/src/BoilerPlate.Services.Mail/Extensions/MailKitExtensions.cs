using MimeKit;
using BoilerPlate.Services.Mail.Message;
using BoilerPlate.Services.Mail.Utils;

namespace BoilerPlate.Services.Mail.Extensions;

internal static class MailKitExtensions
{
    public static MimeMessage ToMimeMessage(this MailMessage mailMessage)
    {
        var message = new MimeMessage();

        message.From.AddRange(mailMessage.From.Select(MailboxAddress.Parse));
        message.To.AddRange(mailMessage.To.Select(MailboxAddress.Parse));

        if (mailMessage.Cc?.Count() > 0)
        {
            message.Cc.AddRange(mailMessage.Cc.Select(MailboxAddress.Parse));
        }

        if (mailMessage.Bcc?.Count() > 0)
        {
            message.Bcc.AddRange(mailMessage.Bcc.Select(MailboxAddress.Parse));
        }

        message.Subject = mailMessage.Subject;

        var body = new Multipart("mixed");

        if (mailMessage.Body?.Length > 0)
        {
            body.Add(new TextPart("html")
            {
                Text = HtmlUtils.Encode(mailMessage.Body)
            });
        }

        message.Body = body;

        return message;
    }

    public static MailMessage ToMailMessage(this MimeMessage mimeMessage) =>
        new()
        {
            From = mimeMessage.From.Select(x => x.ToString()),
            To = mimeMessage.To.Select(x => x.ToString()),
            Cc = mimeMessage.Cc.Select(x => x.ToString()),
            Bcc = mimeMessage.Bcc.Select(x => x.ToString()),
            Subject = mimeMessage.Subject,
            Body = mimeMessage.TextBody
        };
}