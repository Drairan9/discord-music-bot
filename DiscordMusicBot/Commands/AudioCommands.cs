using DiscordMusicBot.Services;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

namespace DiscordMusicBot.Commands;

public class AudioCommands: BaseCommandModule
{
    private readonly ConnectionService _connectionService;
    private readonly SongService _songService;
    private readonly StreamService _streamService;
    private readonly CommandHelperService _commandHelperService;
    public AudioCommands(ConnectionService connectionService, SongService songService, StreamService streamService, CommandHelperService commandHelperService)
    {
        _connectionService = connectionService;
        _songService = songService;
        _streamService = streamService;
        _commandHelperService = commandHelperService;
    }
    
    [Command("play")]
    public async Task PlayCommand(CommandContext ctx, string url)
    {
        var userVoiceChannel = ctx.Member?.VoiceState.Channel;
        if (userVoiceChannel is null)
        {
            await ctx.RespondAsync(embed: _commandHelperService.GetErrorEmbed("You need to be in a voice channel"));
            return;
        }
        
        var vnext = ctx.Client.GetVoiceNext();
        var discordConnection = vnext.GetConnection(ctx.Guild);

        if (discordConnection is null)
        {
            _connectionService.RemoveConnection(ctx.Guild.Id);
            discordConnection = await userVoiceChannel.ConnectAsync();
        }
        
        var connectionInfo = _connectionService.GetConnection(ctx.Guild.Id) ??
                             _connectionService.CreateConnection(ctx.Guild.Id, userVoiceChannel.Id, discordConnection, ctx);
        connectionInfo.Vnext = discordConnection;
        
        if (userVoiceChannel.Id != connectionInfo.VoiceChannelId)
        {
            await ctx.RespondAsync(embed: _commandHelperService.GetErrorEmbed("You need to be in the same voice channel"));
            return;
        }

        var track = await _songService.GetSongData(url);
        track.AddedBy = ctx.Member;
        connectionInfo.AddTrack(track);
        
        var messageEmbed = _commandHelperService.GetSongAddedEmbed(track, ctx);
        await ctx.RespondAsync(embed: messageEmbed);
        

        if (!connectionInfo.IsPlaying)
            await _streamService.StartStream(connectionInfo);
    }
    
    [Command("stop")]
    public async Task StopCommand(CommandContext ctx)
    {
        var connection = await _commandHelperService.ValidateAndGetUserConnection(ctx);
        if (connection is null)
            return;
        
        await _streamService.KillStream(connection);
        _connectionService.RemoveConnection(ctx.Guild.Id);
        connection.Vnext?.Disconnect();
    }
    
    [Command("skip")]
    public async Task SkipCommand(CommandContext ctx)
    {
        var connection = await _commandHelperService.ValidateAndGetUserConnection(ctx);
        if (connection is null)
            return;

        var remainingTracks = connection.GetTracks();
        if (remainingTracks is null || remainingTracks.Length < 1)
        {
            await ctx.RespondAsync(embed: _commandHelperService.GetInfoEmbed("No more tracks in the queue."));
            return;
        }

        await ctx.RespondAsync(embed: _commandHelperService.GetInfoEmbed("Skipping."));
        await _streamService.KillStream(connection);
        await _streamService.StartStream(connection);
    }
}