using BBTimes.Plugin;
using MTM101BaldAPI.SaveSystem;
using System.Collections.Generic;
using System.IO;

namespace BBTimes
{
	public class TimesHandler(BepInEx.PluginInfo info, BasePlugin plug) : ModdedSaveGameIOBinary // Dummy class structure from the api
	{
		readonly private BepInEx.PluginInfo _info = info;
		public override BepInEx.PluginInfo pluginInfo => _info;

		readonly BasePlugin plug = plug;

		public override void Save(BinaryWriter writer)
		{
			writer.Write((byte)0);
		}

		public override void Load(BinaryReader reader)
		{
			reader.ReadByte();
		}

		public override void Reset() { }

		public override string[] GenerateTags()
		{
			List<string> tags = [];

			plug.disabledCharacters.ForEach(x => tags.Add($"Times_DisabledCharacterTag_{x}"));
			plug.disabledBuilders.ForEach(x => tags.Add($"Times_DisabledBuilderTag_{x}"));
			plug.disabledEvents.ForEach(x => tags.Add($"Times_DisabledEventTag_{x}"));
			plug.disabledItems.ForEach(x => tags.Add($"Times_DisabledItemTag_{x}"));


			if (plug.disableHighCeilings.Value)
				tags.Add("Times_Config_DisableHighCeilingsFunction");

			if (plug.enableBigRooms.Value)
				tags.Add("Times_Config_EnableBigRoomsMode");

			if (plug.enableReplacementNPCsAsNormalOnes.Value)
				tags.Add("Times_Config_ReplacementDisable");

			if (plug.enableYoutuberMode.Value)
				tags.Add("Times_Config_YoutuberMode");

			if (BooleanStorage.IsChristmas)
				tags.Add("Times_Specials_Christmas");

			if (plug.HasInfiniteFloors && plug.disableArcadeRennovationsSupport.Value)
				tags.Add("Times_Config_DisableArcadeRennovationsSupport");

			return [.. tags];
		}

		public override string DisplayTags(string[] tags)
		{
			for (int i = 0; i < tags.Length; i++)
			{
				if (tags[i].StartsWith("Times_DisabledCharacterTag_"))
				{
					tags[i] = "Disabled Character: " + tags[i].Split('_')[2]; // The third item from this array should be the Character's name
					continue;
				}
				if (tags[i].StartsWith("Times_DisabledBuilderTag_"))
				{
					tags[i] = "Disabled Builder: " + tags[i].Split('_')[2];
					continue;
				}
				if (tags[i].StartsWith("Times_DisabledEventTag_"))
				{
					tags[i] = "Disabled Random Event: " + tags[i].Split('_')[2];
					continue;
				}
				if (tags[i].StartsWith("Times_DisabledItemTag_"))
				{
					tags[i] = "Disabled Item: " + tags[i].Split('_')[2];
					continue;
				}

				tags[i] = tags[i] switch
				{
					"Times_Config_DisableHighCeilingsFunction" => "Disabled Highceilings for Special Rooms",
					"Times_Config_EnableBigRoomsMode" => "Enabled big room layouts",
					"Times_Config_ReplacementDisable" => "Character replacement feature disabled",
					"Times_Config_YoutuberMode" => "Youtube Mode enabled",
					"Times_Specials_Christmas" => "Christmas mode enabled",
					"Times_Config_DisableArcadeRennovationsSupport" => "No arcade renovations support",
					_ => tags[i]
				};
			}

			return base.DisplayTags(tags);
		}

		public override bool TagsReady() =>
			plug.IsModLoaded;

	}
}
