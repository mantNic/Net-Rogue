using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{
    internal class Map
    {
        public int mapWidth;
        public MapLayer[] layers;
        private Texture Spritesheet;
        private int imagesPerRow;
        private List<Enemy> enemies;
        private List<Item> items;
       
        
        
       public Enemy GetEnemyAt(int x, int y)
       {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].position.X == x && enemies[i].position.Y == y)
                {
                    return enemies[i];
                }                             
            }
            return null;
       }
       public Item GetItemAt(int x, int y)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].position.X == x && items[i].position.Y == y)
                {
                    return items[i];
                }
            }
            return null;
        }

        public Map()
        {
            mapWidth = 1;
            int mapHeight = 1;
            layers = new MapLayer[3];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new MapLayer(mapWidth * mapHeight);
            }
            enemies = new List<Enemy>() { };
            items = new List<Item>() { };
        }

        public void LoadEnemies()
        {
            // Hae viholliset sisältävä taso kentästä
            MapLayer enemyLayer = GetLayer("enemies");
            int[] enemyTiles = enemyLayer.mapTiles;
            int layerHeight = enemyTiles.Length / mapWidth;

            // Käy taso läpi ja luo viholliset
            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;

                    int tileId = enemyTiles[index];

                    if (tileId == 0)
                    {
                        // Tässä kohdassa kenttää ei ole vihollista
                        continue;
                    }
                    else
                    {
                        // Tässä kohdassa kenttää on jokin vihollinen

                        // Tässä pitää vähentää 1,
                        // koska Tiled editori tallentaa
                        // palojen numerot alkaen 1:sestä.
                        int spriteId = tileId - 1;

                        // Hae vihollisen nimi
                        string name = GetEnemyName(spriteId);

                        // Luo uusi vihollinen ja lisää se listaan
                        enemies.Add(new Enemy(name, position, spriteId));
                    }
                }
            }
        }

        public void LoadItems()
        {

            MapLayer itemLayer = GetLayer("items");
            int[] itemTiles = itemLayer.mapTiles;
            int layerHeight = itemTiles.Length / mapWidth;


            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;

                    int tileId = itemTiles[index];

                    if (tileId == 0)
                    {

                        continue;
                    }
                    else
                    {

                        int spriteId = tileId - 1;

                        // Hae vihollisen nimi
                        string name = GetItemName(spriteId);

                        // Luo uusi vihollinen ja lisää se listaan
                        items.Add(new Item(name, position, spriteId));
                    }
                }
            }
        }

        public void SetSpriteSheet(Texture image, int imagesPerRow)
        {
            this.Spritesheet = image;
            this.imagesPerRow = imagesPerRow;
        }

        public MapLayer GetLayer(string layerName)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].name == layerName)
                {
                    return layers[i];
                }
            }
            Console.WriteLine($"Error: No layer with name: {layerName}");
            return null; // Wanted layer was not found!
        }

        public int getTile(int x, int y)
        {
            int index = x + y * mapWidth;
            int tileId = GetLayer("ground").mapTiles[index];
            if (Game.WallTileNumbers.Contains(tileId))
            {
                // Is a wall
                return 2;
            }
            return 1;
        }

        private void Drawtile(int tileIndex, int pixelX, int pixelY)
        {
            int imageX = tileIndex % imagesPerRow;
            int imageY = (int)(tileIndex / imagesPerRow);

            int imagePixelX = imageX * Game.tileSize;
            int imagePixelY = imageY * Game.tileSize;

            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

            Vector2 Position = new Vector2(pixelX, pixelY);
            Raylib.DrawTextureRec(Spritesheet, imageRect, Position, Raylib.WHITE);
        }
        public void draw()
        {


            Console.ForegroundColor = ConsoleColor.Gray; // Change to map color
            MapLayer groundLayer = GetLayer("ground");
            int[] mapTiles = groundLayer.mapTiles;
            int mapHeight = mapTiles.Length / mapWidth; // Calculate the height: the amount of rows
            for (int y = 0; y < mapHeight; y++) // for each row
            {
                for (int x = 0; x < mapWidth; x++) // for each column in the row
                {
                    int index = x + y * mapWidth; // Calculate index of tile at (x, y)
                    int tileIndex = mapTiles[index]; // Read the tile value at index

                    int pixelX = (int)(x * Game.tileSize);
                    int pixelY = (int)(y * Game.tileSize);
                    // Draw the tile graphics
                    Console.SetCursorPosition(x, y);
                    if(tileIndex > 0)
                    {
                        tileIndex--;
                    }
                    Drawtile(tileIndex, pixelX, pixelY);

                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy currentEnemy = enemies[i];
                Vector2 enemyPosition = currentEnemy.position;
                int enemySpriteIndex = currentEnemy.spriteIndex;
                int pixelX = (int)(enemyPosition.X * Game.tileSize);
                int pixelY = (int)(enemyPosition.Y * Game.tileSize);

                Drawtile(enemySpriteIndex, pixelX, pixelY);
            }

            for (int i = 0; i < items.Count; i++)
            {
                Item currentItem = items[i];
                Vector2 itemPosition = currentItem.position;
                int itemSpriteIndex = currentItem.spriteIndex;
                int pixelX = (int)(itemPosition.X * Game.tileSize);
                int pixelY = (int)(itemPosition.Y * Game.tileSize);

                Drawtile(itemSpriteIndex, pixelX, pixelY);
            }
        }

        public string GetEnemyName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Ghost"; break;
                case 109: return "Cyclops"; break;
                default: return "Unknown"; break;
            }
        }

        public string GetItemName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Damage"; break;
                case 109: return "Heal"; break;
                default: return "Unknown"; break;
            }
        }
    }
}

