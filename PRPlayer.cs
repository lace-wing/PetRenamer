using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace PetRenamer
{
    public class PRPlayer : ModPlayer
    {
        public int petTypeVanity;
        public string petNameVanity;
        public int petTypeLight;
        public string petNameLight;

        private int prevItemType;

        public override void Initialize()
        {
            petTypeVanity = 0;
            petNameVanity = "";
            petTypeLight = 0;
            petNameLight = "";

            prevItemType = 0;
        }

        public bool MouseItemChangedToPetItem
        {
            get
            {
                return prevItemType != Main.mouseItem.type && PetRenamer.IsPetItem(Main.mouseItem);
            }
        }

        public override void PostUpdateEquips()
        {
            Item item = player.miscEquips[PetRenamer.VANITY_PET];
            if (PetRenamer.IsPetItem(item))
            {
                PRItem petItem = item.GetGlobalItem<PRItem>();
                petTypeVanity = item.shoot;
                petNameVanity = petItem.petName;
            }
            item = player.miscEquips[PetRenamer.LIGHT_PET];
            if (PetRenamer.IsPetItem(item))
            {
                PRItem petItem = item.GetGlobalItem<PRItem>();
                petTypeLight = item.shoot;
                petNameLight = petItem.petName;
            }

            // Only do the autocomplete in chat on the client
            if (Main.netMode != NetmodeID.Server && Main.myPlayer == player.whoAmI)
            {
                if (Main.drawingPlayerChat &&
                    ((PetRenamer.IsPetItem(Main.mouseItem) && !Main.chatRelease) ||
                    MouseItemChangedToPetItem))
                {
                    if (!Main.chatText.StartsWith(PRCommand.CommandStart) && Main.chatText.Length == 0)
                    {
                        ChatManager.AddChatText(Main.fontMouseText, PRCommand.CommandStart, Vector2.One);
                    }
                }

                prevItemType = Main.mouseItem.type;
            }
        }
    }
}
