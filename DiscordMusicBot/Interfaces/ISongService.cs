using DiscordMusicBot.Entities;
using YoutubeExplode.Videos;

namespace DiscordMusicBot.Interfaces;

public interface ISongService
{
    Task<Track> GetSongData(VideoId videoId);

    Task<Track> SearchSong(string query);

    Task<Stream> DownloadSongIntoStream(VideoId videoId, CancellationToken cancellationToken);
}