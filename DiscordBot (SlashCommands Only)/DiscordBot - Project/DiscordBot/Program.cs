using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureLogging(x =>
            {
                x.AddConsole();
                x.SetMinimumLevel(LogLevel.Information);
            })
            .ConfigureDiscordHost((context, config) =>
            {
                config.SocketConfig = new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    AlwaysDownloadUsers = true,
                    DefaultRetryMode = RetryMode.AlwaysRetry,
                    MessageCacheSize = 500,
                };

                config.Token = context.Configuration.GetValue<string>("Token");
            })
            .UseInteractionService((ctx, config) =>
            {
                config.LogLevel = LogSeverity.Debug;
                config.AutoServiceScopes = true;
                config.EnableAutocompleteHandlers = true;
                config.DefaultRunMode = Discord.Interactions.RunMode.Async;
            })
            .ConfigureServices((context, service) =>
            {
                service
                .AddHostedService<CommandHandler>() // Needs to Inherite from DiscordClientService to AddHostedService.
                .AddSingleton<DiscordHostConfiguration>();
            })
            .Build();

await host.RunAsync();