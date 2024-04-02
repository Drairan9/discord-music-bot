using System.Diagnostics;

namespace DiscordMusicBot.Entities;

public class Connection
{
    public string GuildId { get; set; }

    public string VoiceChannelId { get; set; }

    public Process? Process { get; set; }
}