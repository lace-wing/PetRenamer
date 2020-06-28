using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace PetRenamer.UI.RenamePetUI
{
	//ty jopojelly and darthmorf
	internal class UIQuitButton : UIPanel
	{
		internal static Texture2D texture;

		internal string hoverText;

		internal UIQuitButton(string hoverText)
		{
			if (texture == null)
			{
				texture = ModContent.GetTexture("PetRenamer/UI/RenamePetUI/UIQuitButton");
			}
			this.hoverText = hoverText;
			BackgroundColor = Color.Transparent;
			BorderColor = Color.Transparent;
			Width.Pixels = texture.Width;
			Height.Pixels = texture.Height;
			Recalculate();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			CalculatedStyle innerDimensions = GetInnerDimensions();
			Vector2 pos = new Vector2(innerDimensions.X, innerDimensions.Y) - new Vector2((int)(Width.Pixels * 0.75f), (int)(Height.Pixels * 0.75f));

			spriteBatch.Draw(texture, pos, texture.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			if (IsMouseHovering)
			{
				Main.hoverItemName = hoverText;
			}
		}
	}
}