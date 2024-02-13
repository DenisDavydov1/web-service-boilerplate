namespace BoilerPlate.App.Handlers.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = null!;

    public string PublicKey { get; set; } = null!;

    public string PrivateKey { get; set; } = null!;

    public int LifetimeInMinutes { get; set; }

    public int RefreshTokenLifetimeInMinutes { get; set; }
}