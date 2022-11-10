namespace DiscordBot.Services;

using Discord.Addons.Hosting;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Reflection;

public class CommandHandler : DiscordClientService
{
    public readonly InteractionService _interactionService;
    private readonly IServiceProvider _provider;

    public CommandHandler(DiscordSocketClient client, IServiceProvider provider, InteractionService interactionService, ILogger<DiscordClientService> logger)
        : base(client, logger)
    {
        _provider = provider;
        _interactionService = interactionService;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Client.Ready += Client_Ready;
        Client.InteractionCreated += OnSlashCommandExecuted;
    }

    public async Task Client_Ready()
    {
        // Install commands.
        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        await _interactionService.RegisterCommandsGloballyAsync();
    }

    private async Task OnSlashCommandExecuted(SocketInteraction interaction)
    {
        var ctx = new SocketInteractionContext(Client, interaction);
        await _interactionService.ExecuteCommandAsync(ctx, _provider);
    }
}