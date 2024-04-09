using DiscordMusicBot.Entities;
using DiscordMusicBot.Interfaces;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace DiscordMusicBot.Services;

public class CommandHelperService: ICommandHelperService
{
    private readonly ConnectionService _connectionService;
    
    public CommandHelperService(ConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    public async Task<Connection?> ValidateAndGetUserConnection(CommandContext ctx)
    {
        var userVoiceChannel = ctx.Member?.VoiceState.Channel;
        if (userVoiceChannel is null)
        {
            var embedMessage = GetErrorEmbed("You need to be in a voice channel.");
            await ctx.RespondAsync(embed: embedMessage);
            return null;
        }

        var connection = _connectionService.GetConnection(ctx.Guild.Id);
        if (connection is null)
        {
            var embedMessage = GetErrorEmbed("Connection does not exist.");
            await ctx.RespondAsync(embed: embedMessage);
            return null;
        }

        if (userVoiceChannel.Id != connection.VoiceChannelId)
        {
            var embedMessage = GetErrorEmbed("You need to be in the same voice channel.");
            await ctx.RespondAsync(embed: embedMessage);
            return null;
        }

        return connection;
    }

    public DiscordEmbedBuilder GetSongAddedEmbed(Track track, CommandContext ctx)
    {
        return new DiscordEmbedBuilder
        {
            Color = DiscordColor.Purple,
            Title = "Added song",
            Description = track.Title,
            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = track.ThumbnailUrl,
                    Width = 128,
                    Height = 128
                },
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"Added by {track.AddedBy?.Username}",
                IconUrl = track.AddedBy?.AvatarUrl
            }
        };
    }

    public DiscordEmbedBuilder GetNowPlayingEmbed(Track track, CommandContext ctx)
    {
        return new DiscordEmbedBuilder
        {
            Color = DiscordColor.Purple,
            Title = "Started playing",
            Description = track.Title,
            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
            {
                Url = track.ThumbnailUrl,
                Width = 128,
                Height = 128
            },
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"Added by {track.AddedBy?.Username}",
                IconUrl = track.AddedBy?.AvatarUrl
            }
        };
    }

    public DiscordEmbedBuilder GetInfoEmbed(string description)
    {
        return new DiscordEmbedBuilder
        {
            Color = DiscordColor.Purple,
            Description = description
        };
    }
    
    public DiscordEmbedBuilder GetErrorEmbed(string description)
    {
        return new DiscordEmbedBuilder
        {
            Color = DiscordColor.Red,
            Description = description
        };
    }
}