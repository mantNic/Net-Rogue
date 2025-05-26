using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayGuiCreator;
using ZeroElectric.Vinculum;

namespace Rogue
{
    class PauseMenu
    {
        public event EventHandler BackButtonPressedEvent;
        public event EventHandler OptionsPressed;

        /// <summary>
        /// Piirtää taukovalikon ja laukaisee tapahtumat, kun painikkeita painetaan.
        /// </summary>
        public void DrawMenu()
        {
            int menuWidth = 200;
            int menuX = Raylib.GetScreenWidth() / 2 - menuWidth / 2;
            int menuY = 10;
            int rowHeight = Raylib.GetScreenHeight() / 10;
            MenuCreator creator = new MenuCreator(menuX, menuY, rowHeight, menuWidth);



            creator.Label("Pause Menu");
            if (creator.Button("Options"))
            {
                OptionsPressed.Invoke(this, new EventArgs());
            }
            if (creator.Button("Back"))
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
