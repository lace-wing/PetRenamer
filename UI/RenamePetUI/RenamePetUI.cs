using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.UI;

namespace PetRenamer.UI.RenamePetUI
{
	//ty jopojelly and darthmorf
	internal class RenamePetUI : UIPanel
	{
		private UIText titleText;
		private UIBetterTextBox commandInput;
		private UIPanel applyButton;
		private UIPanel randomizeButton;
		private UIPanel clearButton;
		private List<UIPanel> panels;
		private VanillaItemSlotWrapper itemSlot;

		private readonly static Color bgColor = new Color(73, 94, 171);
		private readonly static Color hoverColor = new Color(100, 118, 184);

		internal const int width = 480;
		internal const int height = 155;

		//UI aligned with center right below the player
		internal int RelativeLeft => Main.screenWidth / 2 - width / 2;
		internal int RelativeTop => Main.screenHeight / 2 + 42; //Half the player height on 200% zoom

		internal bool firstDraw = true;

		//Used to notify the UI to save the item instead of dropping it
		internal static bool saveItemInUI = false;

		public override void OnInitialize()
		{
			Width.Pixels = width;
			Height.Pixels = height;
			Top.Pixels = int.MaxValue / 2;
			Left.Pixels = int.MaxValue / 2;

			panels = new List<UIPanel>();

			float nextElementY = -PaddingTop / 2;

			titleText = new UIText("Rename Pet")
			{
				Top = { Pixels = nextElementY },
				HAlign = 0.5f
			};
			Append(titleText);

			nextElementY += 20;
			string name = string.Empty;

			itemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 1f)
			{
				Left = { Pixels = 0f },
				Top = { Pixels = nextElementY },
				HAlign = 0.5f,
				ValidItemFunc = item => item.IsAir || PetRenamer.IsPetItem(item)
			};
			itemSlot.OnEmptyMouseover += (timer) =>
			{
				Main.hoverItemName = "Place a pet summoning item here";
				if (timer > 60)
				{
					Main.hoverItemName = "1. Place a pet summoning item here"
				+ "\n2. Type a name into the text box"
				+ "\n2. (optional) Press 'Clear' to delete the text"
				+ "\n2. (optional) Press 'Random' for a random name"
				+ "\n3. Press 'Apply' to set the text from the text box as the name for the pet"
				+ "\n3. (optional) Take out the item"
				+ "\n4. Press 'X' to close. Item will be returned to you if it's still in the UI"
				+ "\nNote: If you leave the UI with the item in it open and close the game, it will appear again next time you play";
				}
				else if (timer > 3600)
				{
					Main.hoverItemName = "Santa isn't real";
				}
			};
			Append(itemSlot);

			nextElementY += 49 + 6; //49 is the size of an item slot, 6 is for visual gap

			Item uiItem = Main.mouseItem;
			bool skipCheck = false;

			Item renamePetUIItem = Main.LocalPlayer.GetModPlayer<PRPlayer>().renamePetUIItem;
			if (!renamePetUIItem.IsAir)
			{
				skipCheck = true;
				uiItem = renamePetUIItem;
			}

			if (skipCheck || !uiItem.IsAir && itemSlot.Valid(uiItem))
			{
				if (uiItem.type != ModContent.ItemType<UnloadedItem>())
				{
					PRItem petItem = uiItem.GetGlobalItem<PRItem>();
					name = petItem.petName;
				}
				itemSlot.Item = uiItem.Clone();
				uiItem.TurnToAir(); //The previous item reference (mouse item or saved item) gets cleared
			}

			commandInput = new UIBetterTextBox("Enter Name Here", name)
			{
				BackgroundColor = Color.White,
				Top = { Pixels = nextElementY },
				Width = { Precent = 1f },
				Height = { Pixels = 30f }
			};
			if (!itemSlot.Item.IsAir)
			{
				commandInput.Focus();
			}
			Append(commandInput);

			nextElementY += 36;

			float ratioFromCenter = 0.22f;

			applyButton = new UIPanel()
			{
				Top = { Pixels = nextElementY },
				Width = { Pixels = 60f },
				Height = { Pixels = 30f },
				HAlign = 0.5f - ratioFromCenter,
				BackgroundColor = bgColor
			};
			applyButton.OnClick += (evt, element) => { ApplyNameToItem(); };

			UIText applyButonText = new UIText("Apply")
			{
				Top = { Pixels = -4f },
				Left = { Pixels = -2f }
			};
			applyButton.Append(applyButonText);
			Append(applyButton);
			panels.Add(applyButton);

			randomizeButton = new UIPanel()
			{
				Top = { Pixels = nextElementY },
				Width = { Pixels = 82f },
				Height = { Pixels = 30f },
				HAlign = 0.5f,
				BackgroundColor = bgColor
			};
			randomizeButton.OnClick += (evt, element) => { RandomizeText(); };

			UIText randomizeButtonText = new UIText("Random")
			{
				Top = { Pixels = -4f },
				Left = { Pixels = -2f }
			};
			randomizeButton.Append(randomizeButtonText);
			Append(randomizeButton);
			panels.Add(randomizeButton);

			clearButton = new UIPanel()
			{
				Top = { Pixels = nextElementY },
				Width = { Pixels = 60f },
				Height = { Pixels = 30f },
				HAlign = 0.5f + ratioFromCenter,
				BackgroundColor = bgColor
			};
			clearButton.OnClick += (evt, element) => { ClearTextField(); };

			UIText clearButtonText = new UIText("Clear")
			{
				Top = { Pixels = -4f },
				Left = { Pixels = -2f }
			};
			clearButton.Append(clearButtonText);
			Append(clearButton);
			panels.Add(clearButton);

			UIQuitButton quitButton = new UIQuitButton("Close");
			//Using initializer pattern here didn't work for some reason with the Width
			quitButton.Top.Pixels = -PaddingTop / 2;
			quitButton.Left.Pixels = width - PaddingRight - quitButton.Width.Pixels - 8;
			quitButton.OnClick += (evt, element) => { PRUISystem.CloseRenamePetUI(); };
			Append(quitButton);
		}

		//Called when the state is set
		public override void OnActivate()
		{
			base.OnActivate();
			SoundEngine.PlaySound(SoundID.MenuOpen);
		}

		//Called when this.Deactivate() is called, aka when SetState is called with null
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			if (!saveItemInUI && !Main.gameMenu)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
			}

			Player player = Main.LocalPlayer;
			Item item = itemSlot.Item;
			if (saveItemInUI)
			{
				saveItemInUI = false;
				if (!item.IsAir)
				{
					player.GetModPlayer<PRPlayer>().renamePetUIItem = item.Clone();
					//Save item for next time the player uses the UI
				}
			}
			else
			{
				//Give item back to player
				if (!item.IsAir)
				{
					var source = player.GetSource_Misc("PlayerDropItemCheck");
					player.QuickSpawnClonedItem(source, item, item.stack);
				}
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			//Constantly lock the UI in the position regardless of resolution changes
			Left.Pixels = RelativeLeft;
			Top.Pixels = RelativeTop;

			foreach (var panel in panels)
			{
				panel.BackgroundColor = panel.IsMouseHovering ? hoverColor : bgColor;
			}
		}

		private void ApplyNameToItem()
		{
			Item item = itemSlot.Item;
			if (!item.IsAir && itemSlot.Valid(item))
			{
				PRItem petItem = item.GetGlobalItem<PRItem>();
				var oldName = petItem.petName;
				petItem.petName = commandInput.currentString;
				petItem.petOwner = Main.LocalPlayer.name;

				if (oldName != petItem.petName)
				{
					SoundEngine.PlaySound(SoundID.ResearchComplete);
				}
				else
				{
					SoundEngine.PlaySound(SoundID.MenuTick);
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (firstDraw)
			{
				firstDraw = false;
				return;
			}

			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
				Main.LocalPlayer.cursorItemIconEnabled = false;
				Main.ItemIconCacheUpdate(0);
			}
			base.DrawSelf(spriteBatch);
		}

		private void ClearTextField()
		{
			if (commandInput.currentString.Length > 0)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
			}
			commandInput.SetText("");
		}

		private void RandomizeText()
		{
			if (PetRenamer.randomNames != null)
			{
				string name = Main.rand.Next(PetRenamer.randomNames);
				string fullText = name;
				if (PetRenamer.randomAdjectives != null)
				{
					if (!Main.rand.NextBool(10))
					{
						string adj = Main.rand.Next(PetRenamer.randomAdjectives);
						if (Main.rand.NextBool())
						{
							fullText = name + " the " + adj;
						}
						else
						{
							var adjSameLetter = PetRenamer.randomAdjectives.Where(s => s.StartsWith(name[0].ToString())).ToArray();
							if (adjSameLetter.Length > 0)
							{
								adj = Main.rand.Next(adjSameLetter);
								fullText = adj + " " + name;
							}
						}
					}
				}
				SoundEngine.PlaySound(SoundID.MenuTick);
				commandInput.SetText(fullText);
			}
		}
	}
}
