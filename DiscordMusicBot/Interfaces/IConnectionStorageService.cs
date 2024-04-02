using DiscordMusicBot.Entities;

namespace DiscordMusicBot.Interfaces;

public interface IConnectionStorageService
{
    void AddConnection(string guildId, Connection connection);

    void RemoveConnection(string guildId);

    bool CheckIfConnectionExist(string guildId);

    Connection? GetConnection(string guildId);
}