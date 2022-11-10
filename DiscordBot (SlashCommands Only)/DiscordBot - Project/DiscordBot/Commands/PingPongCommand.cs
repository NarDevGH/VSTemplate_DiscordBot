using Discord.Interactions;

namespace DiscordBot.Commands
{
    public class PingPongCommand : InteractionModuleBase<SocketInteractionContext>
    {
        //Command name most be lowercase.

        [SlashCommand("ping", "bot respond with pong")]
        public async Task PingPong()
        {
            await RespondAsync("Pong", ephemeral: true);
        }
    }
}
