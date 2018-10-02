using DiscordBot.Resources.Database;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Core.Data
{
	public static class Data
	{
		public static int GetStones(ulong UserId)
		{
			using (var DbContext = new SqliteDBContext())
			{
				var count = DbContext.Currency.Where(x => x.UserId == UserId);
				if (DbContext.Currency.Where(x => x.UserId == UserId).Count() < 1)
					return 0;
				return DbContext.Currency.Where(x => x.UserId == UserId).Select(x => x.Account).FirstOrDefault();
			}
		}

		public static async Task SaveStones(ulong UserId, int amount)
		{
			using (var DbContext = new SqliteDBContext())
			{
				if (DbContext.Currency.Where(x => x.UserId == UserId).Count() < 1)
				{
					DbContext.Currency.Add(new Resources.Database.Currency
					{
						UserId = UserId,
						Account = amount
					});
				}
				else
				{
					Resources.Database.Currency current = DbContext.Currency.Where(x => x.UserId == UserId).FirstOrDefault();
					current.Account += amount;
					DbContext.Currency.Update(current);
				}
				await DbContext.SaveChangesAsync();
			}
		}
	}
}
