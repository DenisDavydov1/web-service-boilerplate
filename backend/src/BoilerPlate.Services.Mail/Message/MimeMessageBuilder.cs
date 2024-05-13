using BoilerPlate.Services.Mail.Utils;
using MimeKit;

namespace BoilerPlate.Services.Mail.Message;

public class MimeMessageBuilder : BaseMailMessageBuilder<MimeMessage>
{
    public override MimeMessage Build()
    {
        var message = new MimeMessage();

        message.From.AddRange(From.Select(MailboxAddress.Parse));
        message.To.AddRange(To.Select(MailboxAddress.Parse));

        if (Cc?.Count() > 0)
            message.Cc.AddRange(Cc.Select(MailboxAddress.Parse));

        if (Bcc?.Count() > 0)
            message.Bcc.AddRange(Bcc.Select(MailboxAddress.Parse));

        message.Subject = Subject;

        var body = new Multipart("mixed");

        if (Body?.Length > 0)
        {
            body.Add(new TextPart("html")
            {
                Text = HtmlUtils.Encode(Body)
            });
        }

        message.Body = body;

        return message;
    }
}