using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetRenamer
{
	internal class PetRenamer : Mod
	{
		internal const int VANITY_PET = 0;
		internal const int LIGHT_PET = 1;

		internal static ModKeybind RenamePetUIHotkey;

		internal static int[] ACTPetsWithSmallVerticalHitbox;

		internal static string[] randomNames;

		internal static string[] randomAdjectives;

		internal static bool IsPetItem(Item item)
		{
			return item.type > ItemID.None && item.shoot > ProjectileID.None &&
				item.buffType > 0 && item.buffType < Main.vanityPet.Length &&
				(Main.vanityPet[item.buffType] || Main.lightPet[item.buffType]);
		}

		/// <summary>
		/// Can return null if an exception occurs
		/// </summary>
		private string[] GetArrayFromJson(string name)
		{
			//Json format always:
			/*
			name.json:
			{
				"name": [ "a", "b", "c"],
				"version": 1,
				"source": "google"
			}
			*/
			string[] ret = null;
			try
			{
				string bytes = Encoding.UTF8.GetString(GetFileBytes(name + ".json"));
				JObject json = JsonConvert.DeserializeObject<JObject>(bytes);
				var data = json?[name];
				if (data != null)
				{
					ret = data.ToObject<string[]>();
				}
			}
			catch (Exception e)
			{
				Logger.Warn(e);
			}
			return ret;
		}

		public override void Load()
		{
			RenamePetUIHotkey = KeybindLoader.RegisterKeybind(this, "RenamePet", "P");
			if (!Main.dedServ)
			{
				randomNames = GetArrayFromJson("names");
				randomAdjectives = GetArrayFromJson("adjectives");
			}
		}

		public override void Unload()
		{
			RenamePetUIHotkey = null;
			ACTPetsWithSmallVerticalHitbox = null;

			if (!Main.dedServ)
			{
				randomNames = null;
				randomAdjectives = null;
			}
		}

		public override void PostSetupContent()
		{
			List<int> tempList = new List<int>();
			for (int i = ProjectileID.Count; i < ProjectileLoader.ProjectileCount; i++)
			{
				ModProjectile mProj = ProjectileLoader.GetProjectile(i);
				if (mProj != null && mProj.Mod.Name == "AssortedCrazyThings" && mProj.GetType().Name.StartsWith("CuteSlime"))
				{
					tempList.Add(mProj.Projectile.type);
				}
			}
			ACTPetsWithSmallVerticalHitbox = tempList.ToArray();
			Array.Sort(ACTPetsWithSmallVerticalHitbox);
		}
	}
}
