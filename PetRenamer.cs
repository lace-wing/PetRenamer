using Microsoft.Xna.Framework;
using PetRenamer.UI.MouseoverUI;
using PetRenamer.UI.RenamePetUI;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace PetRenamer
{
	internal class PetRenamer : Mod
	{
		internal const int VANITY_PET = 0;
		internal const int LIGHT_PET = 1;

		internal static UserInterface petRenameInterface;

		internal static UserInterface mouseoverUIInterface;
		internal static MouseoverUI mouseoverUI;

		internal static ModHotKey RenamePetUIHotkey;

		internal static int[] ACTPetsWithSmallVerticalHitbox;

		private GameTime _lastUpdateUiGameTime;

		internal static void ToggleRenamePetUI()
		{
			if (petRenameInterface.CurrentState != null)
			{
				CloseRenamePetUI();
			}
			else
			{
				OpenRenamePetUI();
			}
		}

		internal static void OpenRenamePetUI()
		{
			RenamePetUI ui = new RenamePetUI();
			UIState state = new UIState();
			state.Append(ui);
			petRenameInterface.SetState(state);
		}

		internal static void CloseRenamePetUI()
		{
			petRenameInterface.SetState(null);
		}

		internal static bool IsPetItem(Item item)
		{
			return item.type > ItemID.None && item.shoot > ProjectileID.None &&
				item.buffType > 0 && item.buffType < Main.vanityPet.Length &&
				(Main.vanityPet[item.buffType] || Main.lightPet[item.buffType]);
		}

		public override void Load()
		{
			RenamePetUIHotkey = RegisterHotKey("Rename Pet", "P");
			if (!Main.dedServ)
			{
				petRenameInterface = new UserInterface();

				mouseoverUI = new MouseoverUI();
				mouseoverUI.Activate();
				mouseoverUIInterface = new UserInterface();
				mouseoverUIInterface.SetState(mouseoverUI);
			}
		}

		public override void Unload()
		{
			RenamePetUIHotkey = null;
			ACTPetsWithSmallVerticalHitbox = null;

			if (!Main.dedServ)
			{
				petRenameInterface = null;

				mouseoverUIInterface = null;
				mouseoverUI = null;

				UIQuitButton.texture = null;
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
			//Calls Deactivate and drops the item
			if (petRenameInterface.CurrentState != null)
			{
				RenamePetUI.saveItemInUI = true;
				petRenameInterface.SetState(null);
			}
		}

		public override void UpdateUI(GameTime gameTime)
		{
			_lastUpdateUiGameTime = gameTime;
			if (petRenameInterface?.CurrentState != null)
			{
				petRenameInterface.Update(gameTime);
			}
			mouseoverUI.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Over"));
			if (index != -1)
			{
				layers.Insert(++index, new LegacyGameInterfaceLayer
					(
					"PetRenamer: Mouse Over",
					delegate
					{
						if (_lastUpdateUiGameTime != null && mouseoverUIInterface?.CurrentState != null)
						{
							mouseoverUIInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
			index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (index != -1)
			{
				layers.Insert(index, new LegacyGameInterfaceLayer(
					"PetRenamer: Rename Pet",
					delegate
					{
						if (_lastUpdateUiGameTime != null && petRenameInterface?.CurrentState != null)
						{
							petRenameInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}
