using System;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;

using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Newtonsoft.Json;
using DiscordBot.Resources.Datatypes;
using System.Linq;

namespace DiscordBot
{
	class Program
	{
		private DiscordSocketClient client;
		private CommandService commands;

		static void Main(string[] args)
		{
			new Program().MainAsync().GetAwaiter().GetResult();
		}

		private async Task MainAsync()
		{
			string JSON = "";
			string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Replace(@"bin\Debug\netcoreapp2.0", @"\Data\Settings.json").Replace("DiscordBot.dll", ""));
			using (var Stream = new FileStream(path, FileMode.Open, FileAccess.Read))
			using (var ReadSettings = new StreamReader(Stream))
			{
				JSON = ReadSettings.ReadToEnd();
			}
			Setting settings = JsonConvert.DeserializeObject<Setting>(JSON);
			ISetting.Settings = settings;


			client = new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Info
			});

			commands = new CommandService(new CommandServiceConfig
			{
				CaseSensitiveCommands = false,
				DefaultRunMode = RunMode.Async,
				LogLevel = LogSeverity.Debug
			});

			client.MessageReceived += ClientMessageRecieved;

			await commands.AddModulesAsync(Assembly.GetEntryAssembly());

			client.Ready += ClientReady;
			client.Log += ClientLog;

			await client.LoginAsync(TokenType.Bot, ISetting.Settings.Token);
			await client.StartAsync();

			await Task.Delay(-1);
		}

		private async Task ClientLog(LogMessage message)
		{
			Console.WriteLine($"[{DateTime.Now} at {message.Source}] {message.Message}");
			try
			{
				SocketGuild guild = client.Guilds.Where(x => x.Id == ISetting.Settings.Log[0]).FirstOrDefault();
				SocketTextChannel channel = guild.Channels.Where(x => x.Id == ISetting.Settings.Log[1]).FirstOrDefault() as SocketTextChannel;
				await channel.SendMessageAsync($"[{DateTime.Now} at {message.Source}] {message.Message}");
			}
			catch { }
		}

		private async Task ClientReady()
		{
			await client.SetGameAsync("Minding It's Damn Business");
		}

		private async Task ClientMessageRecieved(SocketMessage message)
		{
			var Message = message as SocketUserMessage;
			var Context = new SocketCommandContext(client, Message);

			if (Context.Message == null || Context.Message.Content == "") return;
			if (Context.User.IsBot) return;

			int argPos = 0;
			if (!(Message.HasStringPrefix("a!", ref argPos) || Message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;

			var Result = await commands.ExecuteAsync(Context, argPos);
			if (!Result.IsSuccess)
				Console.WriteLine($"[{DateTime.Now} at Commands] Something went wrong with executing command. Text: {Context.Message.Content} | Error: {Result.Error}");
		}
	}
}
