using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetRenamer
{
	public class PRItem : GlobalItem
	{
		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
			return lateInstantiation && PetRenamer.IsPetItem(entity);
		}

		public static LocalizedText PetNameText { get; private set; }
		public static LocalizedText PetOwnerText { get; private set; }

		public override void Load()
		{
			string category = $"Items.PetItems.";
			PetNameText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}PetName"));
			PetOwnerText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}PetOwner"));
		}

		public string petName;
		public string petOwner;

		public PRItem()
		{
			petName = string.Empty;
			petOwner = string.Empty;
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
				tooltips.Add(new TooltipLine(Mod, "PetName", PetNameText.Format(petName))
				{
					OverrideColor = color
				});
				tooltips.Add(new TooltipLine(Mod, "PetOwner", PetOwnerText.Format(petOwner))
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
			if (petName.Length == 0)
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
