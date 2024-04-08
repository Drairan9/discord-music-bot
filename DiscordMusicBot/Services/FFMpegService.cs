using System.Diagnostics;
using DiscordMusicBot.Interfaces;

namespace DiscordMusicBot.Services;

public class FFMpegService: IFFMpegService
{
    private ConnectionService _connectionService;
    
    public FFMpegService(ConnectionService connectionService)
    {
        _connectionService = connectionService;
    }

    public Process GetFFmpegProcess(string streamUrl)
    {
        var ffmpeg = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = $"-i {streamUrl} -ac 2 -f s16le -ar 48000 pipe:1",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        var process = Process.Start(ffmpeg);
        if (process == null)
            throw new Exception("Failed to start FFmpeg process.");

        process.StandardError.Close();
        return process;
    }
    
    public Stream GetStreamFromProcess(Process process)
    {
        return process.StandardOutput.BaseStream;
    }

    public void TerminateProcess(Process process)
    {
        process.Kill();
    }
}