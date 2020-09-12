using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Chat;

namespace PetRenamer.UI.MouseoverUI
{
	internal class MouseoverUI : UIState
	{
		internal static string drawString = string.Empty;
		internal static Color drawColor = Color.White;

		private string GetPetName()
		{
			string ret = string.Empty;
			PRPlayer petPlayer;
			string petName;
			Rectangle mouse = new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 1, 1);

			for (int k = 0; k < Main.maxProjectiles; k++)
			{
				Projectile proj = Main.projectile[k];
				if (proj.active && proj.owner < Main.maxPlayers)
				{
					petPlayer = Main.player[proj.owner].GetModPlayer<PRPlayer>();
					if (proj.type == petPlayer.petTypeLight)
					{
						petName = petPlayer.petNameLight;
					}
					else if (proj.type == petPlayer.petTypeVanity)
					{
						petName = petPlayer.petNameVanity;
					}
					else
					{
						continue;
					}

					Rectangle projRect = proj.getRect();
					projRect.Inflate(2, 2);

					//Fix for some ACT pets
					if (Array.BinarySearch(PetRenamer.ACTPetsWithSmallVerticalHitbox, proj.type) >= 0)
					{
						projRect.Y -= (int)(16 * proj.scale);
						projRect.Height += (int)(16 * proj.scale);
					}

					if (mouse.Intersects(projRect)) //mouse cursor inside hitbox
					{
						drawColor = Main.mouseTextColorReal;
						ret = petName;
						break;
					}
				}
			}
			return ret;
		}

		public override void Update(GameTime gameTime)
		{
			if (Main.hoverItemName != string.Empty || Main.LocalPlayer.mouseInterface || Main.mouseText) return;
			base.Update(gameTime);

			int lastMouseXbak = Main.lastMouseX;
			int lastMouseYbak = Main.lastMouseY;
			int mouseXbak = Main.mouseX;
			int mouseYbak = Main.mouseY;
			int screenWidthbak = Main.screenWidth;
			int screenHeightbak = Main.screenHeight;

			PlayerInput.SetZoom_Unscaled();
			PlayerInput.SetZoom_MouseInWorld();

			//Do stuff
			drawString = GetPetName();

			Main.lastMouseX = lastMouseXbak;
			Main.lastMouseY = lastMouseYbak;
			Main.mouseX = mouseXbak;
			Main.mouseY = mouseYbak;
			Main.screenWidth = screenWidthbak;
			Main.screenHeight = screenHeightbak;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (Main.hoverItemName != string.Empty || drawString == string.Empty || Main.LocalPlayer.mouseInterface || Main.mouseText) return;
			base.DrawSelf(spriteBatch);

			Main.LocalPlayer.showItemIcon = false;
			Vector2 mousePos = new Vector2(Main.mouseX, Main.mouseY);
			mousePos.X += 10;
			mousePos.Y += 10;
			if (Main.ThickMouse)
			{
				mousePos.X += 6;
				mousePos.Y += 6;
			}

			Vector2 vector = Main.fontMouseText.MeasureString(drawString);

			if (mousePos.X + vector.X + 4f > Main.screenWidth)
			{
				mousePos.X = (int)(Main.screenWidth - vector.X - 4f);
			}
			if (mousePos.Y + vector.Y + 4f > Main.screenHeight)
			{
				mousePos.Y = (int)(Main.screenHeight - vector.Y - 4f);
			}

			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, drawString, mousePos, drawColor, 0, Vector2.Zero, Vector2.One);
		}
	}
}
