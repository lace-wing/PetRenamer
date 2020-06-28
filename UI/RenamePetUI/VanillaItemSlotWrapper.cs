using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace PetRenamer.UI.RenamePetUI
{
	internal class VanillaItemSlotWrapper : UIElement
	{
		internal Item Item;
		private readonly int _context;
		private readonly float _scale;
		internal Func<Item, bool> ValidItemFunc;

		internal event Action<int> OnEmptyMouseover;

		private int timer = 0;

		internal VanillaItemSlotWrapper(int context = ItemSlot.Context.BankItem, float scale = 1f)
		{
			_context = context;
			_scale = scale;
			Item = new Item();
			Item.SetDefaults(0);

			Width.Set(Main.inventoryBack9Texture.Width * scale, 0f);
			Height.Set(Main.inventoryBack9Texture.Height * scale, 0f);
		}

		/// <summary>
		/// Returns true if this item can be placed into the slot (either empty or a pet item)
		/// </summary>
		internal bool Valid(Item item)
		{
			return ValidItemFunc(item);
		}

		internal void HandleMouseItem()
		{
			if (ValidItemFunc == null || Valid(Main.mouseItem))
			{
				//Handles all the click and hover actions based on the context
				ItemSlot.Handle(ref Item, _context);
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float oldScale = Main.inventoryScale;
			Main.inventoryScale = _scale;
			Rectangle rectangle = GetDimensions().ToRectangle();

			bool contains = ContainsPoint(Main.MouseScreen);

			if (contains && !PlayerInput.IgnoreMouseInterface)
			{
				Main.LocalPlayer.mouseInterface = true;
				HandleMouseItem();
			}
			ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());

			if (contains && Item.IsAir)
			{
				timer++;
				OnEmptyMouseover?.Invoke(timer);
			}
			else if (!contains)
			{
				timer = 0;
			}

			Main.inventoryScale = oldScale;
		}
	}
}