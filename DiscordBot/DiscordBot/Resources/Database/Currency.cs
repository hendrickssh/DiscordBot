using System.ComponentModel.DataAnnotations;

namespace DiscordBot.Resources.Database
{
	public class Currency
	{
		[Key]
		public ulong UserId { get; set; }
		public int Account { get; set; }

	}
}
