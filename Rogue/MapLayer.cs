using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    internal class MapLayer
    {
        public string name;
        public int[] mapTiles;

        /// <summary>
        /// Alustaa uuden karttakerroksen annetulla koolla.
        /// </summary>
        public MapLayer(int mapSize)
        {
            name = "";
            mapTiles = new int[mapSize];
        }
    }
}
