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
				tooltips.Add(new TooltipLine(Mod, "PetName", "Pet Name: " + petName)
				{
					overrideColor = Color.Orange
				});
				tooltips.Add(new TooltipLine(Mod, "PetOwner", "Owner: " + petOwner)
				{
					overrideColor = Color.Orange
				});
			}
		}

		public override void Load(Item item, TagCompound tag)
		{
			petName = tag.GetString("petName");
			petOwner = tag.GetString("petOwner");
		}

		public override bool NeedsSaving(Item item)
		{
			return petName.Length > 0 && PetRenamer.IsPetItem(item);
		}

		public override TagCompound Save(Item item)
		{
			return new TagCompound
			{
				{"petName", petName},
				{"petOwner", petOwner},
			};
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
