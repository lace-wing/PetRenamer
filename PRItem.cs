using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetRenamer
{
	public class PRItem : GlobalItem
	{
		public string petName;
		public string petOwner;

		public PRItem()
		{
			petName = "";
			petOwner = "";
		}

		public override bool InstancePerEntity => true;

		public override GlobalItem Clone(Item item, Item itemClone)
		{
			PRItem myClone = (PRItem)base.Clone(item, itemClone);
			myClone.petName = petName;
			myClone.petOwner = petOwner;
			return myClone;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (petName.Length > 0)
			{
				Color color = Color.Lerp(Color.White, Color.Orange, 0.4f);
				tooltips.Add(new TooltipLine(Mod, "PetName", "Pet Name: " + petName)
				{
					OverrideColor = color
				});
				tooltips.Add(new TooltipLine(Mod, "PetOwner", "Owner: " + petOwner)
				{
					OverrideColor = color
				});
			}
		}

		public override void LoadData(Item item, TagCompound tag)
		{
			petName = tag.GetString("petName");
			petOwner = tag.GetString("petOwner");
		}

		public override void SaveData(Item item, TagCompound tag)
		{
			if (!(petName.Length > 0 && PetRenamer.IsPetItem(item)))
			{
				return;
			}

			tag.Add("petName", petName);
			tag.Add("petOwner", petOwner);
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(petName);
			writer.Write(petOwner);
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			petName = reader.ReadString();
			petOwner = reader.ReadString();
		}
	}
}
