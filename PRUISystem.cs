using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetRenamer.UI.MouseoverUI;
using PetRenamer.UI.RenamePetUI;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace PetRenamer
{
	public class PRUISystem : ModSystem
	{

		internal static UserInterface petRenameInterface;

		internal static UserInterface mouseoverUIInterface;
		internal static MouseoverUI mouseoverUI;

		private GameTime _lastUpdateUiGameTime;

		public override void OnModLoad()
		{
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
			if (!Main.dedServ)
			{
				petRenameInterface = null;

				mouseoverUIInterface = null;
				mouseoverUI = null;

				UIQuitButton.asset = null;
			}
		}

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
