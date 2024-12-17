using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayGuiCreator;
using ZeroElectric.Vinculum;

namespace Rogue
{

    class OptionsMenu
    {
        public event EventHandler BackButtonPressedEvent;
       public void DrawMenu()
        {
            int menuWidth = 200;
            int menuX = Raylib.GetScreenWidth() / 2 - menuWidth / 2;
            int menuY = 10;
            int rowHeight = Raylib.GetScreenHeight() / 10;
            MenuCreator creator = new MenuCreator(menuX, menuY, rowHeight, menuWidth);

            creator.Label("Options Menu");
            if (creator.Button("Back"))
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
        }

    }
}
