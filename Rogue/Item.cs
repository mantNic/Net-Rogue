﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    internal class Item
    {
        public string name;       // Vihollisen nimi
        public Vector2 position;  // Missä vihollinen on kentässä
        public int spriteIndex;  // Missä kohdassa spriteAtlas kuvaa vihollinen on

        public Item(string name, Vector2 position, int spriteIndex)
        {
            // this. viittaa olioon itseensä. Eli olion
            // name muuttujan arvoksi tulee parametrin name arvo.
            this.name = name;
            this.position = position;
            this.spriteIndex = spriteIndex;
            // jne...
        }
    }
}
