using DiscordMusicBot.Commands;
using DiscordMusicBot.Services;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordMusicBot;

public class Program
{
    public static async Task Main()
    {
        var botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
        if (botToken is null)
            throw new Exception("Add discord token to env variables");
        
        var discordClient = new DiscordClient(new DiscordConfiguration
        {
            Token = botToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });
        discordClient.UseVoiceNext();

        var services = new ServiceCollection()
            .AddSingleton<ConnectionStorageService>()
            .AddTransient<FFMpegService>()
            .AddTransient<ConnectionService>()
            .AddTransient<SongService>()
            .AddTransient<StreamService>()
            .AddTransient<CommandHelperService>()
            .BuildServiceProvider();

        var commandConfig = new CommandsNextConfiguration
        {
            StringPrefixes = new[] { "!" },
            Services = services
        };

        var commands = discordClient.UseCommandsNext(commandConfig);
        commands.RegisterCommands<AudioCommands>();

        await discordClient.ConnectAsync();
        await Task.Delay(-1);
    }
}