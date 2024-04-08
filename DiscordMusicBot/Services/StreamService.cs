using DiscordMusicBot.Entities;
using DSharpPlus.VoiceNext;

namespace DiscordMusicBot.Services;

public class StreamService
{
    private FFMpegService _ffMpegService;
    private SongService _songService;

    public StreamService(FFMpegService ffMpegService, SongService songService)
    {
        _ffMpegService = ffMpegService;
        _songService = songService;
    }
    
    public async Task startStream(Connection connection)
    {
        var currentTrack = connection.ConsumeTrack();
        if (currentTrack is null || connection.Vnext is null)
            return;
        
        var streamUrl = await _songService.GetYouTubeStreamUrl(currentTrack.Url);
        var process = _ffMpegService.GetFFmpegProcess(streamUrl);
        var stream = _ffMpegService.GetStreamFromProcess(process);
        
        var transmit = connection.Vnext.GetTransmitSink();
        
        try
        {
            await stream.CopyToAsync(transmit);
        }
        finally
        {
            await transmit.FlushAsync();
        }
        var remainingTracks = connection.GetTracks();
        if (remainingTracks is null || remainingTracks.Length < 1)
        {
            connection.Vnext.Disconnect();
            await stream.DisposeAsync();
            return;
        }
        
        startStream(connection);
    }
}