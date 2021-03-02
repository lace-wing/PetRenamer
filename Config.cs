using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace PetRenamer
{
	public class Config : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		public static Config Instance => ModContent.GetInstance<Config>();

		[Tooltip("If chat should autofill the command automatically when a pet item is selected")]
		[Label("Enable Chat Autofill")]
		[DefaultValue(true)]
		public bool EnableChatAutofill;
	}
}
