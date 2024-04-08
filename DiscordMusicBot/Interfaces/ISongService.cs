using DiscordMusicBot.Entities;
using YoutubeExplode.Videos;

namespace DiscordMusicBot.Interfaces;

public interface ISongService
{
    Task<Track> RequestSongData(VideoId videoId);

    Task<string> GetYouTubeStreamUrl(string videoId);
}