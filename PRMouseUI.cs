using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using System;

namespace PetRenamer
{
    class PRMouseUI : UIState
    {
        internal static string drawString = "";
        internal static Color drawColor = Color.White;

        private string GetPetName()
        {
            string ret = "";
            PRPlayer petPlayer;
            string petName;
            //Rectangle mouse = new Rectangle((int)(Main.mouseX + Main.screenPosition.X), (int)(Main.mouseY + Main.screenPosition.Y), 1, 1);
            //if (Main.LocalPlayer.gravDir == -1f)
            //{
            //    mouse.Y = (int)Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
            //}
            Rectangle mouse = new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 1, 1);

            for (int k = 0; k < 1000; k++)
            {
                Projectile proj = Main.projectile[k];
                if (proj.active)
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

                    //fix for some ACT pets
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
            if (Main.hoverItemName != "" || Main.LocalPlayer.mouseInterface || Main.mouseText) return;
            base.Update(gameTime);

            int lastMouseXbak = Main.lastMouseX;
            int lastMouseYbak = Main.lastMouseY;
            int mouseXbak = Main.mouseX;
            int mouseYbak = Main.mouseY;
            int screenWidthbak = Main.screenWidth;
            int screenHeightbak = Main.screenHeight;

            PlayerInput.SetZoom_Unscaled();
            PlayerInput.SetZoom_MouseInWorld();

            //do stuff
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
            if (Main.hoverItemName != "" || drawString == "" || Main.LocalPlayer.mouseInterface || Main.mouseText) return;
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
