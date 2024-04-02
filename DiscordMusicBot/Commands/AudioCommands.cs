using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DiscordMusicBot.Commands;

public class AudioCommands: BaseCommandModule
{
    [Command("play")]
    public async Task PlayCommand(CommandContext ctx, string url)
    {
        await ctx.RespondAsync(url);
    }
    
    [Command("stop")]
    public async Task StopCommand(CommandContext ctx)
    {
        await ctx.RespondAsync("stop");
    }
    
    [Command("skip")]
    public async Task SkipCommand(CommandContext ctx)
    {
        await ctx.RespondAsync("stop");
    }
}