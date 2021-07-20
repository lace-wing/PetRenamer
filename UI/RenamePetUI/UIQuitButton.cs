using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace PetRenamer.UI.RenamePetUI
{
	//ty jopojelly and darthmorf
	internal class UIQuitButton : UIPanel
	{
		internal static Asset<Texture2D> asset;

		internal string hoverText;

		internal UIQuitButton(string hoverText)
		{
			if (asset == null)
			{
				//When UI dimensions depend on the texture, Immediate is required
				asset = ModContent.Request<Texture2D>("PetRenamer/UI/RenamePetUI/UIQuitButton", AssetRequestMode.ImmediateLoad);
			}
			this.hoverText = hoverText;
			BackgroundColor = Color.Transparent;
			BorderColor = Color.Transparent;
			Width.Pixels = asset.Width();
			Height.Pixels = asset.Height();
			Recalculate();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			CalculatedStyle innerDimensions = GetInnerDimensions();
			Vector2 pos = new Vector2(innerDimensions.X, innerDimensions.Y) - new Vector2((int)(Width.Pixels * 0.75f), (int)(Height.Pixels * 0.75f));

			Texture2D value = asset.Value;
			spriteBatch.Draw(value, pos, value.Bounds, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			if (IsMouseHovering)
			{
				Main.hoverItemName = hoverText;
			}
		}
	}
}
