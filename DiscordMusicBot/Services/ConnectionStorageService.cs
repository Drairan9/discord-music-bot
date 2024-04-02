using System.Collections.Concurrent;
using DiscordMusicBot.Entities;
using DiscordMusicBot.Interfaces;

namespace DiscordMusicBot.Services;

public class ConnectionStorageService: IConnectionStorageService
{
    private readonly ConcurrentDictionary<string, Connection> _connections = [];
    
    public void AddConnection(string guildId, Connection connection)
    {
        _connections.TryAdd(guildId, connection);
    }

    public void RemoveConnection(string guildId)
    {
        _connections.TryRemove(guildId, out _);
    }

    public bool CheckIfConnectionExist(string guildId)
    {
        return _connections.TryGetValue(guildId, out _);
    }

    public Connection? GetConnection(string guildId)
    {
        _connections.TryGetValue(guildId, out var connection);
        return connection;
    }
}