using System.Diagnostics;

namespace DiscordMusicBot.Interfaces;

public interface IFFMpegService
{
    Process GetFFmpegProcess(string streamUrl);

    Stream GetStreamFromProcess(Process process);

    void TerminateProcess(Process process);
}