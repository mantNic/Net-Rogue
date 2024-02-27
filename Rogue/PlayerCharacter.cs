using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

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
    }

   
}