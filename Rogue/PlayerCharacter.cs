using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace Rogue
{

    
   public enum Race
    {
        Human,
        Elf,
        Orc

    }

    public enum Class
    {
        Rogue,
        Warrior,
        Magician
    }

    internal class PlayerCharacter
    {
        public string name;
        public Race rotu;
        public Class luokka;

        public Vector2 paikka;
        public Color pelaajanväri;
        public Texture spritesheet;


        public void draw()
        {
            Console.SetCursorPosition((int)paikka.X, (int)paikka.Y);
            int pixelX = (int)(paikka.X * Game.tileSize);
            int pixelY = (int)(paikka.Y * Game.tileSize);

            var imageRect = new Rectangle(0, 8 * Game.tileSize,Game.tileSize, Game.tileSize);
            var position = new Vector2(pixelX, pixelY);
            Raylib.DrawRectangle(pixelX, pixelY, Game.tileSize, Game.tileSize, Raylib.BLUE);
            Raylib.DrawTextureRec(spritesheet, imageRect, position, Raylib.WHITE);

            // we will need setimageindex also
            



        }

    }
}