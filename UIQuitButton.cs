using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace PetRenamer
{
	// ty jopojelly and darthmorf
	internal class UIQuitButton : UIPanel
	{
		public static Texture2D texture;

		public event EventHandler OnSelectedChanged;

		internal string hoverText;

		public UIQuitButton(string hoverText)
		{
			if (texture == null)
			{
				//TODO unload
				texture = ModContent.GetTexture("PetRenamer/exit");
			}
            this.hoverText = hoverText;
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;
            Width.Set(14f, 0);
            Height.Set(15f, 0);
            Recalculate();
		}

		public void SetHoverText(string hoverText)
		{
			this.hoverText = hoverText;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
            base.DrawSelf(spriteBatch);

            CalculatedStyle innerDimensions = GetInnerDimensions();
            Vector2 pos = new Vector2(innerDimensions.X-15, innerDimensions.Y - 5);


            spriteBatch.Draw(texture, pos, texture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            

			if (IsMouseHovering)
			{
				Main.hoverItemName = hoverText;
			}
		}
	}
}