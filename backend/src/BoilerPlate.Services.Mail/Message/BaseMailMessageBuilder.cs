namespace BoilerPlate.Services.Mail.Message;

public abstract class BaseMailMessageBuilder<TMessage>
{
    public required IEnumerable<string> From { get; set; }
    public required IEnumerable<string> To { get; set; }
    public IEnumerable<string>? Cc { get; set; }
    public IEnumerable<string>? Bcc { get; set; }
    public required string Subject { get; set; }
    public string? Body { get; set; }

    public abstract TMessage Build();
}