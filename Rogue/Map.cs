using Newtonsoft.Json;
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
        List<Enemy> enemyTypes;
        private List<Item> items;

        /// <summary>
        /// Palauttaa vihollisen annetussa (x, y) -sijainnissa, tai null jos ei löydy.
        /// </summary>
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

        /// <summary>
        /// Palauttaa esineen annetussa (x, y) -sijainnissa, tai null jos ei löydy.
        /// </summary>
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

        /// <summary>
        /// Lataa vihollistyypit JSON-tiedostosta enemyTypes-listaan.
        /// </summary>
        public void LoadEnemyTypes(string filename)
        {
            enemyTypes = new List<Enemy>();
            if (File.Exists(filename))
            {
                string fileContents = File.ReadAllText(filename);

                enemyTypes = JsonConvert.DeserializeObject<List<Enemy>>(fileContents);
            }
        }

        /// <summary>
        /// Lataa viholliset kentälle "enemies"-tasosta.
        /// </summary>
        public void LoadEnemies()
        {
            LoadEnemyTypes("EnemyTiedot.txt");
            MapLayer enemyLayer = GetLayer("enemies");
            int[] enemyTiles = enemyLayer.mapTiles;
            int layerHeight = enemyTiles.Length / mapWidth;

            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Vector2 position = new Vector2(x, y);
                    int index = x + y * mapWidth;
                    int tileId = enemyTiles[index];

                    if (tileId == 0)
                    {
                        continue;
                    }
                    else
                    {
                        int spriteId = tileId - 1;
                        Enemy newEnemy = GetEnemy(spriteId);
                        if (newEnemy != null)
                        {
                            newEnemy.position = position;
                            enemies.Add(newEnemy);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lataa esineet kentälle "items"-tasosta.
        /// </summary>
        public void LoadItems()
        {
            MapLayer itemLayer = GetLayer("items");
            int[] itemTiles = itemLayer.mapTiles;
            int layerHeight = itemTiles.Length / mapWidth;

            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
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
                        string name = GetItemName(spriteId);
                        items.Add(new Item(name, position, spriteId));
                    }
                }
            }
        }

        /// <summary>
        /// Asettaa spritesheet-kuvan ja rivikohtaisen kuvamäärän piirtämistä varten.
        /// </summary>
        public void SetSpriteSheet(Texture image, int imagesPerRow)
        {
            this.Spritesheet = image;
            this.imagesPerRow = imagesPerRow;
        }

        /// <summary>
        /// Palauttaa annetun nimisen MapLayerin tai null jos sitä ei löydy.
        /// </summary>
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
            return null;
        }

        /// <summary>
        /// Palauttaa laatta-tyyppikoodin kohdassa (x, y): 2 seinälle, 1 muille.
        /// </summary>
        public int getTile(int x, int y)
        {
            int index = x + y * mapWidth;
            int tileId = GetLayer("ground").mapTiles[index];
            if (Game.WallTileNumbers.Contains(tileId))
            {
                return 2;
            }
            return 1;
        }

        /// <summary>
        /// Piirtää yhden laatan spritesheetistä annettuihin pikselikoordinaatteihin.
        /// </summary>
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

        /// <summary>
        /// Piirtää kartan, mukaan lukien maa, viholliset ja esineet.
        /// </summary>
        public void draw()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            MapLayer groundLayer = GetLayer("ground");
            int[] mapTiles = groundLayer.mapTiles;
            int mapHeight = mapTiles.Length / mapWidth;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int index = x + y * mapWidth;
                    int tileIndex = mapTiles[index];

                    int pixelX = (int)(x * Game.tileSize);
                    int pixelY = (int)(y * Game.tileSize);
                    Console.SetCursorPosition(x, y);
                    if (tileIndex > 0)
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

        /// <summary>
        /// Palauttaa vihollisen nimen sprite-indeksin perusteella.
        /// </summary>
        public string GetEnemyName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Ghost"; break;
                case 109: return "Cyclops"; break;
                default: return "Unknown"; break;
            }
        }

        /// <summary>
        /// Palauttaa uuden Enemy-olion spriteId:n perusteella, tai null jos ei löydy.
        /// </summary>
        private Enemy GetEnemy(int spriteId)
        {
            foreach (Enemy template in enemyTypes)
            {
                if (spriteId == template.spriteIndex)
                {
                    return new Enemy(template);
                }
            }
            Console.WriteLine($"Error, no enemy found with id: {spriteId}");
            return null;
        }

        /// <summary>
        /// Palauttaa esineen nimen sprite-indeksin perusteella.
        /// </summary>
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
