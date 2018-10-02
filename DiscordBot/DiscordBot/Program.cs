using System;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

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
			client = new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Debug
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

			string Token = "";
			using (var Stream = new FileStream((Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).Replace(@"bin\Debug\netcoreapp2.0", @"Data\Token.txt"), FileMode.Open, FileAccess.Read))
			using (var ReadToken = new StreamReader(Stream))
			{
				Token = ReadToken.ReadToEnd();
			}
			await client.LoginAsync(TokenType.Bot, Token);
			await client.StartAsync();

			await Task.Delay(-1);
		}

		private async Task ClientLog(LogMessage message)
		{
			Console.WriteLine($"[{DateTime.Now} at {message.Source}] {message.Message}");
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
