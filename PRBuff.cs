using Terraria;
using Terraria.ModLoader;

namespace PetRenamer
{
	internal class PRBuff : GlobalBuff
	{
		private const string NEWLINE = "\nName: ";

		public override void ModifyBuffTip(int type, ref string tip, ref int rare)
		{
			PRPlayer petPlayer = Main.LocalPlayer.GetModPlayer<PRPlayer>();
			if (Main.vanityPet[type] && petPlayer.petNameVanity != string.Empty)
			{
				tip += NEWLINE + petPlayer.petNameVanity;
			}
			else if (Main.lightPet[type] && petPlayer.petNameLight != string.Empty)
			{
				tip += NEWLINE + petPlayer.petNameLight;
			}
		}
	}
}
