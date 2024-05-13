using BoilerPlate.Core.Options;

namespace BoilerPlate.Services.Telegram.Options;

public class TelegramBotOptions : BasePolymorphicArrayElementOptions
{
    public string Name { get; set; } = null!;

    public string Token { get; set; } = null!;
}