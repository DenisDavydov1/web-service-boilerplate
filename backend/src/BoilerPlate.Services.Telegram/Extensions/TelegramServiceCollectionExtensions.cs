using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Options;
using BoilerPlate.Services.Telegram.BotTestService;
using BoilerPlate.Services.Telegram.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace BoilerPlate.Services.Telegram.Extensions;

public static class TelegramServiceCollectionExtensions
{
    public static void AddTelegram(this IServiceCollection services, IConfiguration configuration)
    {
        var telegramOptions = services.AddServiceOptions<TelegramOptions>(configuration);

        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>(httpClient =>
            {
                var botOptions = new TelegramBotClientOptions(telegramOptions.BotToken);
                return new TelegramBotClient(botOptions, httpClient);
            });

        services.AddScoped<ITelegramBotTestService, TelegramBotTestService>();
    }
}