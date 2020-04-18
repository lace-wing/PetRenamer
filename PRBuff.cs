using Terraria;
using Terraria.ModLoader;

namespace PetRenamer
{
    class PRBuff : GlobalBuff
    {
        private const string NEWLINE = "\nName: ";

        public override void ModifyBuffTip(int type, ref string tip, ref int rare)
        {
            PRPlayer petPlayer = Main.LocalPlayer.GetModPlayer<PRPlayer>();
            if (Main.vanityPet[type] && petPlayer.petNameVanity != "")
            {
                tip += NEWLINE + petPlayer.petNameVanity;
            }
            else if(Main.lightPet[type] && petPlayer.petNameLight != "")
            {
                tip += NEWLINE + petPlayer.petNameLight;
            }
        }
    }
}
