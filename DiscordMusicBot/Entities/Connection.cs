using System.Diagnostics;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;

namespace DiscordMusicBot.Entities;

public class Connection
{
    public ulong GuildId { get; set; }

    public ulong VoiceChannelId { get; init; }
    
    public CommandContext CmdCtx { get; set; }

    public Process? Process { get; set; }
    
    public Stream? CurrentStream { get; set; }

    public bool IsPlaying { get; set; } = false;

    public bool IsPaused { get; set; } = false;
    
    public VoiceNextConnection? Vnext { get; set; }

    private List<Track> Tracks = new List<Track>();

    public Track? ConsumeTrack()
    {
        if (Tracks.Count < 1)
            return null;

        var firstTrack = Tracks[0];
        Tracks.RemoveAt(0);
        return firstTrack;
    }
    
    public Track[]? GetTracks()
    {
        if (Tracks.Count < 1)
            return null;
        return Tracks.ToArray();
    }

    public void AddTrack(Track track)
    {
        Tracks.Add(track);
    }
}