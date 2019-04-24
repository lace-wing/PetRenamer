using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetRenamer
{
    public class PRCommand : ModCommand
    {
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
                return "renamepet";
            }
        }

        public override string Usage
        {
            get
            {
                return "/renamepet newName";
            }
        }

        public override string Description
        {
            get
            {
                return "Set a name or rename the pet item in your mouse. newName = del -> clear the name";
            }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1) Main.NewText("/renamepet newName");
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
                        newName += args[i] + ((i != args.Length - 1)?" ": "");
                    }

                    if (previousName != "" && (newName == "delete" || newName == "del"))
                    {
                        petItem.petName = "";
                        petItem.petOwner = "";
                        Main.NewText("Nickname '" + previousName + "' removed", Color.OrangeRed);
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
                    Main.NewText(item.Name + "not a valid item");
                }
            }
        }
    }
}
