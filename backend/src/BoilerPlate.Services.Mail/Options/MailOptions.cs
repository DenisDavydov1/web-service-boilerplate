using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.Mail.Options;

public class MailOptions : IServiceOptions
{
    public static string SectionName => "Mail";

    public bool Enabled { get; } = true;

    public IEnumerable<BasePolymorphicArrayElementOptions> SmtpServers { get; set; } =
        Array.Empty<BasePolymorphicArrayElementOptions>();

    public IEnumerable<BasePolymorphicArrayElementOptions> ImapServers { get; set; } =
        Array.Empty<BasePolymorphicArrayElementOptions>();
}