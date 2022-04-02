using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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

		public override string Description => "Set a name or rename the pet item in your mouse. " + ARGUMENT + " = " + RESET + " -> remove name";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args.Length < 1) caller.Reply(Usage);
			else
			{
				Item item = Main.mouseItem;

				if (PetRenamer.IsPetItem(item))
				{
					PRItem petItem = item.GetGlobalItem<PRItem>();

					string previousName = petItem.petName;
					string newName = "";

					for (int i = 0; i < args.Length; i++)
					{
						newName += args[i] + ((i != args.Length - 1) ? " " : "");
					}

					if (previousName != string.Empty && newName == RESET)
					{
						petItem.petName = string.Empty;
						petItem.petOwner = string.Empty;
						caller.Reply("Nickname '" + previousName + "' reset", Color.OrangeRed);
					}
					else
					{
						petItem.petName = newName;
						petItem.petOwner = caller.Player.name;
						if (previousName == string.Empty)
						{
							caller.Reply("Named the pet summoned by " + item.Name + " '" + petItem.petName + "'", Color.Orange);
						}
						else if (previousName == petItem.petName)
						{
							caller.Reply("Pet is already called '" + previousName + "'", Color.OrangeRed);
						}
						else
						{
							caller.Reply("Renamed the pet summoned by " + item.Name + " from '" + previousName + "' to '" + petItem.petName + "'", Color.Orange);
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
						caller.Reply("No item to rename! Hold a pet summon item in your cursor", Color.OrangeRed);
					}
					else
					{
						caller.Reply(item.Name + " is not a valid item!", Color.OrangeRed);
					}
				}
			}
		}
	}
}
