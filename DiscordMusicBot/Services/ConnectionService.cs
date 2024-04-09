using DiscordMusicBot.Entities;
using DiscordMusicBot.Interfaces;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;

namespace DiscordMusicBot.Services;

public class ConnectionService: IConnectionService
{
    private readonly ConnectionStorageService _connectionStorageService;
    
    public ConnectionService(ConnectionStorageService connectionStorageService)
    {
        _connectionStorageService = connectionStorageService;
    }
    
    public Connection CreateConnection(ulong guildId, ulong voiceConnectionId, VoiceNextConnection vnext, CommandContext commandContext)
    {
        var connection = new Connection
        {
            GuildId = guildId,
            VoiceChannelId = voiceConnectionId,
            Vnext = vnext,
            CmdCtx = commandContext
        };
        _connectionStorageService.AddConnection(guildId, connection);
        return connection;
    }

    public bool RemoveConnection(ulong guildId)
    {
        return _connectionStorageService.RemoveConnection(guildId);
    }

    public Connection? GetConnection(ulong guildId)
    {
        return _connectionStorageService.GetConnection(guildId);
    }
}