using Discord.Commands;
using DiscordBot.Resources.Datatypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;

namespace DiscordBot.Core.Moderation
{
	public class Moderation : ModuleBase<SocketCommandContext>
	{
		[Command("reload"), Alias("reload"), Summary("Reloads the settings file")]
		public async Task Reload()
		{
			if(Context.User.Id != ISetting.Settings.Owner)
			{
				await Context.Channel.SendMessageAsync($":x: You do not have permission to do that {Context.User.Username}!");
				return;
			}

			string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Replace(@"bin\Debug\netcoreapp2.0", @"\Data\Appsettings.json").Replace("DiscordBot.dll", ""));
			if(!File.Exists(path))
			{
				await Context.Channel.SendMessageAsync(":x: The file is not found");
				Console.WriteLine($"[{DateTime.Now}] Expect Location: " + path);
				return;
			}

			string JSON = "";
			using (var Stream = new FileStream(path, FileMode.Open, FileAccess.Read))
			using (var ReadSettings = new StreamReader(Stream))
			{
				JSON = ReadSettings.ReadToEnd();
			}

			Setting Setting = JsonConvert.DeserializeObject<Setting>(JSON);
			ISetting.Settings = Setting;

			await Context.Channel.SendMessageAsync(":white_check_mark: Settings updated!");
		}
	}
}
