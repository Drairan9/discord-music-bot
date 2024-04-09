using System.Diagnostics;

namespace DiscordMusicBot.Interfaces;

public interface IFFMpegService
{
    Task<MemoryStream> TransformIntoPcm(Stream inputStream, CancellationToken cancellationToken);
}