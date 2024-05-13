using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.Telegram.Options;

public class TelegramOptions : IServiceOptions
{
    public static string SectionName => "Telegram";

    public bool Enabled { get; set; } = true;

    public IEnumerable<BasePolymorphicArrayElementOptions> Bots { get; set; } =
        Array.Empty<BasePolymorphicArrayElementOptions>();
}