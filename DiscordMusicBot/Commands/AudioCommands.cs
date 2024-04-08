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
    public AudioCommands(ConnectionService connectionService, SongService songService, StreamService streamService)
    {
        _connectionService = connectionService;
        _songService = songService;
        _streamService = streamService;
    }
    
    [Command("play")]
    public async Task PlayCommand(CommandContext ctx, string url)
    {
        var userVoiceChannel = ctx.Member?.VoiceState.Channel;
        if (userVoiceChannel is null)
        {
            await ctx.RespondAsync("You need to be in a voice channel");
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
        
        if (userVoiceChannel.Id != connectionInfo.VoiceChannelId)
        {
            await ctx.RespondAsync("You need to be in the same voice channel");
            return;
        }

        var track = await _songService.RequestSongData(url);
        connectionInfo.AddTrack(track);

        if (!connectionInfo.IsPlaying)
            _streamService.startStream(connectionInfo);
        
        await ctx.RespondAsync($"Added track: {track.Title}");
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