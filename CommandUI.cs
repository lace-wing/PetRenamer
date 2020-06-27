using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace PetRenamer
{
    // ty jopojelly and darthmorf
    internal class CommandUI : UIDragablePanel
    {
        private UIText titleText;
        private UIBetterTextBox commandInput;
        private UIPanel button;

        public override void OnInitialize()
        {
            int screenWidth = Main.screenWidth;
            Width.Set(0, 0.3f);
            Height.Set(105, 0f);
            Left.Set(screenWidth / 4, 0f); // can't use precent as that causes the draggable windows to jump when first dragged
            Top.Set(10f, 0f);

            titleText = new UIText("Rename Pet Window");
            titleText.Top.Pixels = -5;
            Append(titleText);

            commandInput = new UIBetterTextBox("Command");
            commandInput.BackgroundColor = Color.White;
            commandInput.Top.Set(0, 0.25f);
            commandInput.Width.Precent = 1f;
            commandInput.Height.Pixels = 30;
            commandInput.OnTextChanged += () => { CommandInputChanged(); };
            Append(commandInput);

            button = new UIPanel();
            button.Width.Pixels = 60f;
            button.Height.Pixels = 30f;
            button.Top.Precent = 0.70f;
            button.BackgroundColor = new Color(73, 94, 171);
            button.OnClick += (evt, element) => { SaveBtnPress(); };
            UIText buttonText = new UIText("Save");
            buttonText.Top.Pixels = -4f;
            buttonText.Left.Pixels = 0f;
            button.Append(buttonText);
            Append(button);

            UIQuitButton quitButton = new UIQuitButton("Close Menu");
            quitButton.Top.Set(-10,0f);
            quitButton.Left.Set(0,0.98f);
            quitButton.OnClick += (evt, element) => { PetRenamer.ToggleCommandUI(); };
            Append(quitButton);

        }

        public void InitValues()
        {
            OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (button.IsMouseHovering)
            {
               button.BackgroundColor = new Color(100, 118, 184);
            }
            else
            {
                button.BackgroundColor = new Color(73, 94, 171);
            }
        }

        private void SaveBtnPress()
        {

        }

        private void CommandInputChanged()
        {

        }
    }
}
