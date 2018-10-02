using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace DiscordBot.Core.Commands
{
    public class TextToVoice : ModuleBase<SocketCommandContext>
    {
		[Command("speak"), Alias("say", "repeat"), Summary("Text to voice")]
		public async Task TextToVoiceCommand([Remainder]string input = "")
		{
			await Context.Channel.SendMessageAsync(input, true);
		}

		[Command("embed")]
		public async Task Embed([Remainder]string Input = "None")
		{
			EmbedBuilder embed = new EmbedBuilder();
			embed.WithAuthor("Test", Context.User.GetAvatarUrl());
			embed.WithColor(40, 200, 150);
			embed.WithFooter($"footer test {Context.Guild.Owner.Nickname}", Context.Guild.Owner.GetAvatarUrl());
			embed.WithDescription("Dummy description");

			embed.AddInlineField("User Input: ", Input);
			await Context.Channel.SendMessageAsync("", false, embed.Build());
		}
    }
}
