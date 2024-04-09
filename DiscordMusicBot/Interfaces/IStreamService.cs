using DiscordMusicBot.Entities;

namespace DiscordMusicBot.Interfaces;

public interface IStreamService
{
   public Task StartStream(Connection connection);
   
   public Task KillStream(Connection connection);
}