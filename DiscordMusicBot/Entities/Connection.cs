using System.Diagnostics;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;

namespace DiscordMusicBot.Entities;

public class Connection
{
    public ulong GuildId { get; set; }

    public ulong VoiceChannelId { get; init; }
    
    public CommandContext CmdCtx { get; set; }

    public CancellationTokenSource? CancellationTokenSource { get; set; } 
        
    public bool IsPlaying { get; set; } = false;

    public bool IsPaused { get; set; } = false;
    
    public VoiceNextConnection? Vnext { get; set; }

    private readonly List<Track> _tracks = new List<Track>();

    public Track? ConsumeTrack()
    {
        if (_tracks.Count < 1)
            return null;

        var firstTrack = _tracks[0];
        _tracks.RemoveAt(0);
        return firstTrack;
    }
    
    public Track[]? GetTracks()
    {
        if (_tracks.Count < 1)
            return null;
        return _tracks.ToArray();
    }

    public void AddTrack(Track track)
    {
        _tracks.Add(track);
    }
}