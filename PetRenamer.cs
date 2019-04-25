using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
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

        public static int[] ACTPetsWithSmallVerticalHitbox;

        public static bool IsPetItem(Item item)
        {
            bool checkItem = item.type > 0 && item.shoot != 0 && item.buffType != 0;
            bool checkBuff = false;
            if (checkItem && item.buffType < Main.vanityPet.Length)
            {
                checkBuff = Main.vanityPet[item.buffType] || Main.lightPet[item.buffType];
            }
            return checkItem && checkBuff;
        }

        public override void Load()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                PRMouseUI = new PRMouseUI();
                PRMouseUI.Activate();
                PRMouseUIInterface = new UserInterface();
                PRMouseUIInterface.SetState(PRMouseUI);
            }
        }

        public override void PostSetupContent()
        {
            List<int> tempList = new List<int>();
            for (int i = Main.maxProjectileTypes; i < ProjectileLoader.ProjectileCount; i++)
            {
                ModProjectile mProj = ProjectileLoader.GetProjectile(i);
                if (mProj != null && mProj.mod.Name == "AssortedCrazyThings" && mProj.GetType().Name.StartsWith("CuteSlime"))
                {
                    tempList.Add(mProj.projectile.type);
                }
            }
            ACTPetsWithSmallVerticalHitbox = tempList.ToArray();
            Array.Sort(ACTPetsWithSmallVerticalHitbox);
        }

        public override void Unload()
        {
            if (!Main.dedServ && Main.netMode != 2)
            {
                PRMouseUIInterface = null;
                PRMouseUI = null;
            }
            ACTPetsWithSmallVerticalHitbox = null;
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
