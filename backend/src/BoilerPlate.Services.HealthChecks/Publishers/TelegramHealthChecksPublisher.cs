using BoilerPlate.Core.Serialization;
using BoilerPlate.Services.HealthChecks.Options.Publishers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BoilerPlate.Services.HealthChecks.Publishers;

internal class TelegramHealthChecksPublisher(
    TelegramHealthChecksPublisherOptions options,
    ITelegramBotClient telegramBotClient)
    : BaseHealthChecksPublisher<TelegramHealthChecksPublisherOptions>(options)
{
    protected override Task PublishInternal(HealthReport report, CancellationToken ct)
    {
        var message = JsonConvert.SerializeObject(report, SerializationSettings.Indented);

        if (message.Length > 4096)
        {
            message = message[..2045] + "\n\n...\n\n" + message[^2044..];
        }

        return telegramBotClient.SendTextMessageAsync(
            new ChatId(Options.ChatId),
            message,
            cancellationToken: ct);
    }
}