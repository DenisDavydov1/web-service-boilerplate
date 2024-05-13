using BoilerPlate.Core.Extensions;
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
        var telegramOptions = services.AddServiceOptions<TelegramOptions>(configuration,
        [
            ("Bots",
            [
                (null, typeof(TelegramBotOptions))
            ])
        ]);

        foreach (var options in telegramOptions.Bots)
        {
            var botOptions = (TelegramBotOptions) options;
            services.AddHttpClient(botOptions.Name);
            services.AddKeyedTransient<ITelegramBotClient>(botOptions.Name, (provider, _) =>
            {
                var httpClient = provider.GetRequiredService<IHttpClientFactory>()
                    .CreateClient(botOptions.Name);
                var botClientOptions = new TelegramBotClientOptions(botOptions.Token);
                return new TelegramBotClient(botClientOptions, httpClient);
            });
        }

        services.AddScoped<ITelegramBotTestService, TelegramBotTestService>();
    }
}