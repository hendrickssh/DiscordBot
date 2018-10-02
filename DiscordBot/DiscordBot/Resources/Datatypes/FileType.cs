using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Resources.Datatypes
{
	public static class ISetting
	{
		public static Setting Settings;
	}

	public class Setting
	{
		public string Token { get; set; }
		public ulong Owner { get; set; }
		public List<ulong> Log { get; set; }
		public string Version { get; set; }
		public List<ulong> Banned { get; set; }
		public string Currency { get; set; }
	}
}
