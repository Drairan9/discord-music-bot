using CliWrap;
using DiscordMusicBot.Interfaces;

namespace DiscordMusicBot.Services;

public class FFMpegService: IFFMpegService
{
    private ConnectionService _connectionService;
    
    public FFMpegService(ConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    public async Task<MemoryStream> TransformIntoPcm(Stream inputStream, CancellationToken cancellationToken)
    {
        var outputStream = new MemoryStream();
        await Cli.Wrap("ffmpeg")
            .WithArguments(
                "-hide_banner -loglevel panic -i pipe:0 -af bass=g=5:f=110:w=0.6 -ac 2 -f s16le -ar 48000 -b:a 64k pipe:1")
            .WithStandardInputPipe(PipeSource.FromStream(inputStream))
            .WithStandardOutputPipe(PipeTarget.ToStream(outputStream)).ExecuteAsync(cancellationToken);
        return outputStream;
    }
}