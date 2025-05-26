using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    public class Enemy
    {
        public string name;       // Vihollisen nimi
        public Vector2 position;  // Missä vihollinen on kentässä
        public int spriteIndex;  // Missä kohdassa spriteAtlas kuvaa vihollinen on
        public int hitPoints;     // Vihollisen Hp

        /// <summary>
        /// Alustaa vihollisen nimellä, sijainnilla ja sprite-indeksillä.
        /// </summary>
        public Enemy(string name, Vector2 position, int spriteIndex)
        {
            this.name = name;
            this.position = position;
            this.spriteIndex = spriteIndex;
        }

        /// <summary>
        /// Alustaa vihollisen nimellä, elinvoimalla ja sprite-indeksillä.
        /// </summary>
        public Enemy(string name, int hitPoints, int spriteIndex)
        {
            this.name = name;
            this.hitPoints = hitPoints;
            this.spriteIndex = spriteIndex;
        }

        public Enemy() { }

        /// <summary>
        /// Luo kopion annetusta vihollisesta.
        /// </summary>
        public Enemy(Enemy copy)
        {
            this.name = copy.name;
            this.position = copy.position;
            this.spriteIndex = copy.spriteIndex;
            this.hitPoints = copy.hitPoints;
        }

        /// <summary>
        /// Palauttaa merkkijonon, joka sisältää vihollisen nimen ja elinvoiman.
        /// </summary>
        public override string ToString()
        {
            return $"Enemy: {name} HP:{hitPoints}";
        }
    }
}