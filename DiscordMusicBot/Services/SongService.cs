using DiscordMusicBot.Entities;
using DiscordMusicBot.Interfaces;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace DiscordMusicBot.Services;

public class SongService: ISongService
{
    public async Task<Track> GetSongData(VideoId videoId)
    {
        var client = new YoutubeClient();
        var video = await client.Videos.GetAsync(videoId);

        return new Track
        {
            Title = video.Title,
            Author = video.Author.ChannelTitle,
            ThumbnailUrl = video.Thumbnails[0].Url,
            Duration = video.Duration ?? TimeSpan.Zero,
            Url = video.Url,
        };
    }

    public async Task<Track> SearchSong(string query)
    {
        var client = new YoutubeClient();
        var videos = await client.Search.GetVideosAsync(query);
        var firstResult = videos[0];
        return new Track
        {
            Title = firstResult.Title,
            Author = firstResult.Author.ChannelTitle,
            ThumbnailUrl = firstResult.Thumbnails[0].Url,
            Duration = firstResult.Duration ?? TimeSpan.Zero,
            Url = firstResult.Url
        };
    }

    public async Task<Stream> DownloadSongIntoStream(VideoId videoId, CancellationToken cancellationToken)
    {
        var client = new YoutubeClient();
        var streamManifest = await client.Videos.Streams.GetManifestAsync(videoId, cancellationToken);
        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
        
        return await client.Videos.Streams.GetAsync(streamInfo, cancellationToken);
    }
}