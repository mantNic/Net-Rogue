using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    internal class MapReader
    {

        public Map LoadTestMap()
        {
            Map testi = new Map();
            testi.mapWidth = 8;
            testi.mapTiles = new int[] {
    2, 2, 2, 2, 2, 2, 2, 2,
    2, 1, 1, 2, 1, 1, 1, 2,
    2, 1, 1, 2, 1, 1, 1, 2,
    2, 1, 1, 1, 1, 1, 2, 2,
    2, 2, 2, 2, 1, 1, 1, 2,
    2, 1, 1, 1, 1, 1, 1, 2,
    2, 2, 2, 2, 2, 2, 2, 2 };
            return testi;
        }
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
        public Map LoadMapFromFile(string fileName)
        {
            bool exists = File.Exists(fileName);
            if (exists == false)
            {
                Console.WriteLine($"File {fileName} not found");
                return LoadTestMap(); // Return the test map as fallback
            }

            string fileContents;

            using (StreamReader reader = File.OpenText(fileName))
            {
                // TODO: Read all lines into fileContens
                fileContents =  reader.ReadToEnd();
            }

            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents); 

            return loadedMap;
        }
        
    }
}
