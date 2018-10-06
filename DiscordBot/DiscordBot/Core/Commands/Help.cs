using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace DiscordBot.Core.Commands
{
	public class Help : ModuleBase<SocketCommandContext>
	{
		[Command("Help"), Alias("help", "saveme", "botty"), Summary("Explains how to use botty")]
		public async Task Embed([Remainder]string Input = "None")
		{
			EmbedBuilder embed = new EmbedBuilder();
			embed.WithAuthor("Welcome, I'm here to help", Context.Guild.IconUrl);
			embed.WithFooter($"Thanks for gaming with us!");
			embed.WithDescription($@"Welcome to {Context.Guild.Name}!
											Here's how things work around here:
											a! is the keyword to trigger a command, use it before any other command!
											speak, say, reapeat will have me say everything after
												i.e 'a!say hello' and I'll say hi!");
			await Context.Channel.SendMessageAsync("", false, embed.Build());
		}
	}
}