using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

        /*Basically press hotkey (P), window opens
         * in the window:
         * title ("Rename pet")
         * quit/cancel button
         * item slot 
         * Text field
         * Save button
         */
        internal static UserInterface commandInterface;
        //This is just a UIState, it gets a CommandUI appended //maybe redo
        internal static UIState modUiState;
        internal static List<CommandUI> commandUis;

        internal static UserInterface PRMouseUIInterface;
        internal static PRMouseUI PRMouseUI;


        // Show/Hide individual instance of UI, tied to specific tile
        internal static void ToggleCommandUI()
        {
            int count = commandUis.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    CommandUI cUI = commandUis[i];
                    modUiState.RemoveChild(cUI);
                }
                commandUis.Clear();
            }
            else
            {
                CommandUI cUI = new CommandUI();
                modUiState.Append(cUI);
                commandUis.Add(cUI);
                cUI.OnInitialize();
            }

        }

        public static int[] ACTPetsWithSmallVerticalHitbox;

        public static bool IsPetItem(Item item)
        {
            return item.type > ItemID.None && item.shoot > ProjectileID.None &&
                item.buffType > 0 && item.buffType < Main.vanityPet.Length &&
                (Main.vanityPet[item.buffType] || Main.lightPet[item.buffType]);
        }

        internal static ModHotKey AutoRecallHotKey;

        public override void Load()
        {
            AutoRecallHotKey = RegisterHotKey("Rename Pet", "P");
            if (!Main.dedServ)
            {
                modUiState = new UIState();
                modUiState.Activate();
                commandInterface = new UserInterface();
                commandInterface?.SetState(modUiState);

                commandUis = new List<CommandUI>();

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

        public override void PreSaveAndQuit()
        {
            modUiState.RemoveAllChildren();
        }

        public override void Unload()
        {
            AutoRecallHotKey = null;
            ACTPetsWithSmallVerticalHitbox = null;

            if (!Main.dedServ)
            {
                commandInterface = null;
                modUiState = null;
                commandUis = null;

                PRMouseUIInterface = null;
                PRMouseUI = null;
            }
        }

        private GameTime _lastUpdateUiGameTime;

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (commandInterface?.CurrentState != null)
            {
                commandInterface.Update(gameTime);
            }
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
                        if (_lastUpdateUiGameTime != null && PRMouseUIInterface?.CurrentState != null)
                        {
                            PRMouseUIInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "PetRenamer: Text Box",
                delegate
                {
                    if (_lastUpdateUiGameTime != null && commandInterface?.CurrentState != null)
                    {
                        commandInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                    }
                    return true;
                },
                InterfaceScaleType.UI));
            }
        }
    }
}
