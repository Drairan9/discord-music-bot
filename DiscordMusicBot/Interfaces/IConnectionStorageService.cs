using DiscordMusicBot.Entities;

namespace DiscordMusicBot.Interfaces;

public interface IConnectionStorageService
{
    bool AddConnection(ulong guildId, Connection connection);

    bool RemoveConnection(ulong guildId);

    bool CheckIfConnectionExist(ulong guildId);

    Connection? GetConnection(ulong guildId);
}