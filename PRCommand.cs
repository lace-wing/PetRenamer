using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetRenamer
{
	internal class PRCommand : ModCommand
	{
		#region Custom stuff

		private const string COMMANDNAME = "renamepet";
		private const string ARGUMENT = "newName";
		private const string RESET = "reset";

		public static string CommandStart => "/" + COMMANDNAME + " ";
		#endregion

		public override CommandType Type => CommandType.Chat;

		public override string Command => COMMANDNAME;

		public override string Usage => "/" + COMMANDNAME + " " + ARGUMENT;

		public override string Description => DescriptionText.ToString();

		public static LocalizedText DescriptionText { get; private set; }
		public static LocalizedText NicknameResetText { get; private set; }
		public static LocalizedText NamedNewText { get; private set; }
		public static LocalizedText NamedSameText { get; private set; }
		public static LocalizedText RenamedText { get; private set; }

		public static LocalizedText NoItemText { get; private set; }
		public static LocalizedText InvalidItemText { get; private set; }

		public override void Load()
		{
			string category = $"Commands.PRCommand.";
			DescriptionText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}Description")).WithFormatArgs(ARGUMENT, RESET);
			NicknameResetText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}NicknameReset"));
			NamedNewText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}NamedNew"));
			NamedSameText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}NamedSame"));
			RenamedText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}Renamed"));

			NoItemText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}NoItem"));
			InvalidItemText ??= Language.GetOrRegister(Mod.GetLocalizationKey($"{category}InvalidItem"));
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args.Length < 1) caller.Reply(Usage);
			else
			{
				Item item = Main.mouseItem;

				if (item.TryGetGlobalItem(out PRItem petItem))
				{
					string previousName = petItem.petName;
					string newName = string.Join(" ", args);

					if (previousName != string.Empty && newName == RESET)
					{
						petItem.petName = string.Empty;
						petItem.petOwner = string.Empty;
						caller.Reply(NicknameResetText.Format(previousName), Color.OrangeRed);
					}
					else
					{
						petItem.petName = newName;
						petItem.petOwner = caller.Player.name;
						if (previousName == string.Empty)
						{
							caller.Reply(NamedNewText.Format(item.Name, petItem.petName), Color.Orange);
						}
						else if (previousName == petItem.petName)
						{
							caller.Reply(NamedSameText.Format(previousName), Color.OrangeRed);
						}
						else
						{
							caller.Reply(RenamedText.Format(item.Name, previousName, petItem.petName), Color.Orange);
						}

						if (previousName != petItem.petName)
						{
							SoundEngine.PlaySound(SoundID.ResearchComplete);
						}
					}
				}
				else
				{
					if (item.type == ItemID.None)
					{
						caller.Reply(NoItemText.ToString(), Color.OrangeRed);
					}
					else
					{
						caller.Reply(InvalidItemText.Format(item.Name), Color.OrangeRed);
					}
				}
			}
		}
	}
}
