using Telegram.Bot;
using Telegram.Bot.Types;

namespace BoilerPlate.Services.Telegram.BotTestService;

internal class TelegramBotTestService(ITelegramBotClient telegramBotClient) : ITelegramBotTestService
{
    public async Task SendMessage(string chatId, string message, CancellationToken ct) =>
        await telegramBotClient.SendTextMessageAsync(new ChatId(chatId), message, cancellationToken: ct);
}