using Terraria;
using Terraria.ModLoader;

namespace PetRenamer
{
    class PRBuff : GlobalBuff
    {
        public override void ModifyBuffTip(int type, ref string tip, ref int rare)
        {
            PRPlayer petPlayer = Main.LocalPlayer.GetModPlayer<PRPlayer>();
            if (Main.vanityPet[type] && petPlayer.petNameVanity != "")
            {
                tip += "\nName: " + petPlayer.petNameVanity;
            }
            else if(Main.lightPet[type] && petPlayer.petNameLight != "")
            {
                tip += "\nName: " + petPlayer.petNameLight;
            }
        }
    }
}
