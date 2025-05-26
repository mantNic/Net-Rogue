using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboMapReader;

namespace Rogue
{
    internal class MapReader
    {
        /// <summary>
        /// Lukee ja tulostaa tekstitiedoston sisällön rivi riviltä.
        /// </summary>
        public void ReadMapFromFileTest(string fileName)
        {
            using (StreamReader reader = File.OpenText(fileName))
            {
                Console.WriteLine("File contents:");
                Console.WriteLine();

                string line;
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break; // End of file
                    }
                    Console.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Lataa kartan JSON-tiedostosta ja deserialisoi sen Map-olioksi.
        /// </summary>
        public Map LoadMapFromFile(string fileName)
        {
            bool exists = File.Exists(fileName);
            if (exists == false)
            {
                Console.WriteLine($"File {fileName} not found");
                return null; // Return the test map as fallback
            }

            string fileContents;

            using (StreamReader reader = File.OpenText(fileName))
            {
                // TODO: Read all lines into fileContens
                fileContents = reader.ReadToEnd();
            }

            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);

            return loadedMap;
        }

        /// <summary>
        /// Lataa Tiled-editorilla luodun kartan TurboMapReaderin avulla ja muuntaa sen Map-olioksi.
        /// </summary>
        public Map LoadTiled()
        {
            TiledMap mapMadeInTiled = TurboMapReader.MapReader.LoadMapFromFile("tiledmap.tmj");
            Map map = ConvertTiledMapToMap(mapMadeInTiled);
            return map;
        }

        /// <summary>
        /// Muuntaa TiledMap-olion pelissä käytettäväksi Map-olioksi.
        /// </summary>
        public Map ConvertTiledMapToMap(TiledMap turboMap)
        {
            // Luo tyhjä kenttä
            Map rogueMap = new Map();

            // Muunna tason "ground" tiedot
            TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("ground");
            TurboMapReader.MapLayer itemLayer = turboMap.GetLayerByName("items");
            TurboMapReader.MapLayer enemyLayer = turboMap.GetLayerByName("enemies");

            // TODO: Lue kentän leveys. Kaikilla TurboMapReader.MapLayer olioilla on sama leveys
            rogueMap.mapWidth = groundLayer.width;

            // Kuinka monta kenttäpalaa tässä tasossa on?
            int howManyTiles = groundLayer.data.Length;

            // Taulukko jossa palat ovat
            int[] groundTiles = groundLayer.data;

            // Luo uusi taso tietojen perusteella
            MapLayer myGroundLayer = new MapLayer(howManyTiles);
            myGroundLayer.name = "ground";
            myGroundLayer.mapTiles = groundTiles;

            MapLayer myItemsLayer = new MapLayer(howManyTiles);
            myItemsLayer.name = "items";
            myItemsLayer.mapTiles = itemLayer.data;

            MapLayer myEnemiesLayer = new MapLayer(howManyTiles);
            myEnemiesLayer.name = "enemies";
            myEnemiesLayer.mapTiles = enemyLayer.data;

            // Tallenna taso kenttään
            rogueMap.layers[0] = myGroundLayer;
            rogueMap.layers[1] = myItemsLayer;
            rogueMap.layers[2] = myEnemiesLayer;

            // Lopulta palauta kenttä
            return rogueMap;
        }
    }
}
