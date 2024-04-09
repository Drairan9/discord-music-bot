using DiscordMusicBot.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace DiscordMusicBot.Interfaces;

public interface ICommandHelperService
{
    public Task<Connection?> ValidateAndGetUserConnection(CommandContext ctx);

    public DiscordEmbedBuilder GetSongAddedEmbed(Track track, CommandContext ctx);
    
    public DiscordEmbedBuilder GetNowPlayingEmbed(Track track, CommandContext ctx);

    public DiscordEmbedBuilder GetInfoEmbed(string description);

    public DiscordEmbedBuilder GetErrorEmbed(string description);
}