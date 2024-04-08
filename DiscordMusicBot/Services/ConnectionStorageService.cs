using System.Collections.Concurrent;
using DiscordMusicBot.Entities;
using DiscordMusicBot.Interfaces;

namespace DiscordMusicBot.Services;

public class ConnectionStorageService: IConnectionStorageService
{
    private readonly ConcurrentDictionary<ulong, Connection> _connections = [];
    
    public bool AddConnection(ulong guildId, Connection connection)
    {
        return _connections.TryAdd(guildId, connection);
    }

    public bool RemoveConnection(ulong guildId)
    {
        return _connections.TryRemove(guildId, out _);
    }

    public bool CheckIfConnectionExist(ulong guildId)
    {
        return _connections.TryGetValue(guildId, out _);
    }

    public Connection? GetConnection(ulong guildId)
    {
        _connections.TryGetValue(guildId, out var connection);
        return connection;
    }
}