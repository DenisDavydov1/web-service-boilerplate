using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.Mail.Options.Servers;

public abstract class BaseMailServerOptions : BasePolymorphicArrayElementOptions
{
    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int Port { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public bool UseSsl { get; set; }

    public bool SkipSslCertificateValidation { get; set; }
}