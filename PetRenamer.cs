using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace PetRenamer
{
    class PetRenamer : Mod
    {
        public const int VANITY_PET = 0;
        public const int LIGHT_PET = 1;

        internal static UserInterface PRMouseUIInterface;
        internal static PRMouseUI PRMouseUI;

        public static bool IsPetItem(Item item)
        {
            return item.type > 0 && (item.shoot != 0 || item.buffType != 0);
        }

        public override void PostSetupContent()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                PRMouseUI = new PRMouseUI();
                PRMouseUI.Activate();
                PRMouseUIInterface = new UserInterface();
                PRMouseUIInterface.SetState(PRMouseUI);
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                PRMouseUIInterface = null;
                PRMouseUI = null;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            PRMouseUI.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseOverIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Over"));
            if (mouseOverIndex != -1)
            {
                layers.Insert(++mouseOverIndex, new LegacyGameInterfaceLayer
                    (
                    "PetRenamer: Mouse Over",
                    delegate
                    {
                        PRMouseUIInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
