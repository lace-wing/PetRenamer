using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace PetRenamer
{
    public class PRCommand : ModCommand
    {
        #region Custom stuff

        private const string COMMANDNAME = "renamepet";
        private const string ARGUMENT = "newName";
        private const string RESET = "reset";

        public static string CommandStart
        {
            get
            {
                return "/" + COMMANDNAME + " ";
            }
        }
        #endregion

        public override CommandType Type
        {
            get
            {
                return CommandType.Chat;
            }
        }

        public override string Command
        {
            get
            {
                return COMMANDNAME;
            }
        }

        public override string Usage
        {
            get
            {
                return "/" + COMMANDNAME + " " + ARGUMENT;
            }
        }

        public override string Description
        {
            get
            {
                return "Set a name or rename the pet item in your mouse. " + ARGUMENT + " = " + RESET + " -> remove name";
            }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1) Main.NewText(Usage);
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
                        newName += args[i] + ((i != args.Length - 1) ? " ": "");
                    }

                    if (previousName != "" && newName == RESET)
                    {
                        petItem.petName = "";
                        petItem.petOwner = "";
                        Main.NewText("Nickname '" + previousName + "' reset", Color.OrangeRed);
                    }
                    else
                    {
                        petItem.petName = newName;
                        petItem.petOwner = caller.Player.name;
                        if (previousName == "")
                        {
                            Main.NewText("Named the pet summoned by " + item.Name + " '" + petItem.petName + "'", Color.Orange);
                        }
                        else if (previousName == petItem.petName)
                        {
                            Main.NewText("Pet is already called '" + previousName + "'", Color.OrangeRed);
                        }
                        else
                        {
                            Main.NewText("Renamed the pet summoned by " + item.Name + " from '" + previousName + "' to '" + petItem.petName + "'", Color.Orange);
                        }
                    }
                }
                else
                {
                    if (item.type == 0)
                    {
                        Main.NewText("No item to rename! Hold a pet summon item in your cursor", Color.OrangeRed);
                    }
                    else
                    {
                        Main.NewText(item.Name + " is not a valid item!", Color.OrangeRed);
                    }
                }
            }
        }
    }
}
