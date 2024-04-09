using DiscordMusicBot.Entities;
using DiscordMusicBot.Interfaces;

namespace DiscordMusicBot.Services;

public class StreamService: IStreamService
{
    private readonly FFMpegService _ffMpegService;
    private readonly SongService _songService;
    private readonly CommandHelperService _commandHelperService;

    public StreamService(FFMpegService ffMpegService, SongService songService, CommandHelperService commandHelperService)
    {
        _ffMpegService = ffMpegService;
        _songService = songService;
        _commandHelperService = commandHelperService;
    }
    
    public async Task StartStream(Connection connection)
    {
        while (true)
        {
            var currentTrack = connection.ConsumeTrack();
            connection.CancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = connection.CancellationTokenSource.Token;

            if (currentTrack is null || connection.Vnext is null)
                break;
            
            
            var nowPlayingEmbed = _commandHelperService.GetNowPlayingEmbed(currentTrack, connection.CmdCtx);
            await connection.CmdCtx.RespondAsync(
                embed: nowPlayingEmbed);
            
            connection.IsPlaying = true;
            var audioStream = await _songService.DownloadSongIntoStream(currentTrack.Url, cancellationToken);
            var pcmStream = await _ffMpegService.TransformIntoPcm(audioStream, cancellationToken);
            var transmit = connection.Vnext.GetTransmitSink();

            await transmit.WriteAsync(pcmStream.ToArray(), 0, (int)pcmStream.Length, cancellationToken);
            await transmit.FlushAsync(cancellationToken);
            
            var remainingTracks = connection.GetTracks();
            if (remainingTracks is null || remainingTracks.Length < 1)
            {
                await connection.CmdCtx.RespondAsync(embed: _commandHelperService.GetInfoEmbed("No more tracks in the queue."));
                connection.Vnext.Disconnect();
                break;
            };
        }
    }

    public async Task KillStream(Connection connection)
    {
        if (connection.CancellationTokenSource is not null)
            await connection.CancellationTokenSource.CancelAsync();

        connection.Vnext?.GetTransmitSink().FlushAsync();
        connection.IsPlaying = false;
        connection.IsPaused = false;
    }
}