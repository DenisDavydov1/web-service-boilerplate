using BoilerPlate.Core.Options;

namespace BoilerPlate.App.Handlers.Options;

public class JwtOptions : IServiceOptions
{
    public static string SectionName => "Jwt";

    public bool Enabled => true;

    public string Issuer { get; set; } = null!;

    public string PublicKey { get; set; } = null!;

    public string PrivateKey { get; set; } = null!;

    public int LifetimeInMinutes { get; set; }

    public int RefreshTokenLifetimeInMinutes { get; set; }
}