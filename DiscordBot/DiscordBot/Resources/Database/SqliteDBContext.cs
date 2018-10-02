using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Resources.Database
{
	public class SqliteDBContext : DbContext
	{
		public DbSet<Currency> Currency { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder Options)
		{
			string DBLocation = Assembly.GetEntryAssembly().Location.Replace(@"\bin\Debug\netcoreapp2.0", @"\Data").Replace("DiscordBot.dll", "");
			Options.UseSqlite($"Data Source={DBLocation}\\Database.sqlite");
		}
	}
}
