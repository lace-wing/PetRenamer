# PetRenamer

![Icon](https://raw.githubusercontent.com/direwolf420/PetRenamer/master/icon.png)

Terraria Forum link: https://forums.terraria.org/index.php?threads/pet-renamer-allows-you-to-name-your-pets.79293/

Allows the player to name the summon item of any pet, so that the pet will show its name when hovered over it (similar to how NPCs show their name)

How to use:
* Either open your chat (default: Enter), then left click on a summon item (so it sticks to your cursor), or hold an item in your cursor and then open your chat
* The chat will then say "/renamepet ", the following words you type in will be accepted as the new pet name
* It will also save the name of the player that gave the pet its last nickname
* To remove the name, simply type "reset" as the name (yes, you can name your pet "reset" aswell, but only if it doesn't have a name currently)

Notes:
* Pet names will be saved on the item, even when disabling the mod and enabling it again
* Multiplayer compatible
* Compatible with most modded pets if they do it the vanilla way:
    * If a summon item happens to summon multiple different pets at once, only one of them will be named
    * If the pet name hover text only shows up in a specific position, contact that other mods dev so he can fix the pets hitbox
    * (For example some Thorium Mod pets only show a name when hovered over the top left corner of the sprite)

Example:

`/renamepet Mr. Bones` >> sets or renames the name of the pet item you are holding to "Mr. Bones"

`/renamepet reset` >> deletes the current name

Demonstration (gfycat videos):

[Setting a name](https://gfycat.com/unsteadysplendidannelid)

[Removing a name](https://gfycat.com/flickeringringediraniangroundjay)

Changelog:

v1.0.0.4: tml 0.11 update, command says the correct thing now when not holding a pet item

v1.0.0.3: Buff tip of the summoned pet now also displays its name

v1.0.0.2: Narrowed down detection of pet summoning items, if you had any previous items set to a name that weren't a pet, they will be reset upon game quit

v1.0 and v1.0.0.1: Initial release and added icon
