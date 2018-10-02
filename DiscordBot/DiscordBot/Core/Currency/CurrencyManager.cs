using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Resources.Database;
using DiscordBot.Resources.Datatypes;

namespace DiscordBot.Core.Currency
{
	public class CurrencyManager : ModuleBase<SocketCommandContext>
	{
		[Group("currency"), Alias("money", "cash"), Summary("Group to manage stuff with currency")]
		public class StonesGroup : ModuleBase<SocketCommandContext>
		{
			[Command(""), Alias("me", "my"), Summary("Shows all your currency")]
			public async Task Me(IUser User = null)
			{
				if(User == null)
					await Context.Channel.SendMessageAsync($"{Context.User.Username}, you have {Data.Data.GetStones(Context.User.Id)} {ISetting.Settings.Currency}!");
				else
					await Context.Channel.SendMessageAsync($"{User.Username}, you have {Data.Data.GetStones(User.Id)} {ISetting.Settings.Currency}!");
			}

			[Command("give"), Alias("gift"), Summary("Used to give currency")]
			public async Task Give(IUser User = null, int Amount = 0)
			{
				if(User == null)
				{
					await Context.Channel.SendMessageAsync(":x: User not specified: a!currency give **<@user>** <amount>");
					return;
				}

				if(User.IsBot)
				{
					await Context.Channel.SendMessageAsync($":x: Bots don't need {ISetting.Settings.Currency}");
					return;
				}

				if(Amount == 0)
				{
					await Context.Channel.SendMessageAsync($":x: Invalid amount of {ISetting.Settings.Currency} specified for {User.Username}: a!currency give <@user> **<amount>**");
					return;
				}

				SocketGuildUser User1 = Context.User as SocketGuildUser;
				if(!User1.GuildPermissions.Administrator)
				{
					await Context.Channel.SendMessageAsync($":x: You don't have permission to do this, ask an admin");
					return;
				}

				await Context.Channel.SendMessageAsync($":tada: {User.Mention} you have been given {Amount} {ISetting.Settings.Currency} from {Context.User.Username}!");


				await Data.Data.SaveStones(User.Id, Amount);
			}

			[Command("reset"), Summary("resets users progress")]
			public async Task Reset(IUser User = null)
			{
				if (User == null)
				{
					await Context.Channel.SendMessageAsync(":x: User not specified: a!currency reset **<@user>**");
					return;
				}

				if (User.IsBot)
				{
					await Context.Channel.SendMessageAsync($":x: Bots don't have {ISetting.Settings.Currency}");
					return;
				}

				SocketGuildUser User1 = Context.User as SocketGuildUser;
				if (!User1.GuildPermissions.Administrator)
				{
					await Context.Channel.SendMessageAsync($":x: You don't have permission to do this, ask an admin");
					return;
				}

				await Context.Channel.SendMessageAsync($":tada: {User.Mention} you have been reset by {Context.User.Username}!");


				using (var DbContext = new SqliteDBContext())
				{
					DbContext.Currency.RemoveRange(DbContext.Currency.Where(x => x.UserId == User.Id));
					await DbContext.SaveChangesAsync();
				}
			}
		}
	}
}