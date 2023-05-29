using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetRenamer
{
	internal class PRBuff : GlobalBuff
	{
		public static LocalizedText NameText { get; private set; }

		public override void Load()
		{
			string category = $"Buffs.PetBuff.";
			NameText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}Name"));
		}

		public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
		{
			PRPlayer petPlayer = Main.LocalPlayer.GetModPlayer<PRPlayer>();
			if (Main.vanityPet[type] && petPlayer.petNameVanity != string.Empty)
			{
				tip += $"\n{NameText.Format(petPlayer.petNameVanity)}";
			}
			else if (Main.lightPet[type] && petPlayer.petNameLight != string.Empty)
			{
				tip += $"\n{NameText.Format(petPlayer.petNameLight)}";
			}
		}
	}
}
