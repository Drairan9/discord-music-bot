using DiscordMusicBot.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;

namespace DiscordMusicBot.Interfaces;

public interface IConnectionService
{
    public Connection CreateConnection(ulong guildId, ulong voiceChannelId, VoiceNextConnection vnext, CommandContext commandContext);

    public bool RemoveConnection(ulong guildId);

    public Connection? GetConnection(ulong guildId);
}