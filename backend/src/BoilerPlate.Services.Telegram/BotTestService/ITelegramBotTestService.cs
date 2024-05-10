namespace BoilerPlate.Services.Telegram.BotTestService;

public interface ITelegramBotTestService
{
    Task SendMessage(string chatId, string message, CancellationToken ct);
}