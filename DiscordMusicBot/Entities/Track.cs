namespace DiscordMusicBot.Entities;

public class Track
{
    public string Title { get; set; } = "";

    public string Author { get; set; } = "";

    public string Url { get; set; } = "";

    public string ThumbnailUrl { get; set; } = "";

    public TimeSpan Duration { get; set; }
}