using DiscordMusicBot.Entities;
using DiscordMusicBot.Interfaces;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace DiscordMusicBot.Services;

public class SongService: ISongService
{
    public async Task<Track> RequestSongData(VideoId videoId)
    {
        var client = new YoutubeClient();
        var video = await client.Videos.GetAsync(videoId);

        return new Track
        {
            Title = video.Title,
            Author = video.Author.ChannelTitle,
            ThumbnailUrl = video.Thumbnails[0].Url,
            Duration = video.Duration ?? TimeSpan.Zero
        };
    }

    public async Task<string> GetYouTubeStreamUrl(string videoId)
    {
        var client = new YoutubeClient();
        var streamManifest = await client.Videos.Streams.GetManifestAsync(videoId);
        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
        return streamInfo.Url;
    }
}