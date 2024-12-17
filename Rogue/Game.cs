using RayGuiCreator;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace Rogue
{
    public enum GameState
    {
        MainMenu,
        CharacterCreation,
        GameLoop,
        OptionsMenu,
        PauseMenu
    }
    public class Game
    {
        PauseMenu myPauseMenu = new PauseMenu();
        OptionsMenu myOptionsMenu = new OptionsMenu();
        PlayerCharacter player;
        Map level1;
        Map level2;
        Map currentlevel;
        public static readonly int tileSize = 16;
        public static readonly List<int> WallTileNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 12, 13, 14, 15, 16, 17, 18, 19, 20, 24, 25, 26, 27, 28, 29, 40, 57, 58, 59 };


        Stack<GameState> stateStack = new Stack<GameState>();

        TextBoxEntry playerNameEntry;
        MultipleChoiceEntry classChoiceEntry;
        MultipleChoiceEntry raceChoiceEntry;
        

        void DrawCharaterCreationMenu()
        {
            int menuWidth = 200;
            int menuX = Raylib.GetScreenWidth() / 2 - menuWidth / 2;
            int menuY = 10;
            int rowHeight = Raylib.GetScreenHeight() / 10;
            MenuCreator creator = new MenuCreator(menuX, menuY, rowHeight, menuWidth);

            creator.Label("Charater name");
            creator.TextBox(playerNameEntry);
            creator.DropDown(classChoiceEntry);
            creator.DropDown(raceChoiceEntry);

            if (creator.Button("Start Game"))
            {
                if (TestName(playerNameEntry.ToString()))
                {
                    player.name = playerNameEntry.ToString();

                    stateStack.Push(GameState.GameLoop); 

                    string luokkaVastaus = classChoiceEntry.ToString();

                    if (luokkaVastaus == Class.Rogue.ToString() || luokkaVastaus == "1")
                    {
                        player.luokka = Class.Rogue;

                    }
                    else if (luokkaVastaus == Class.Warrior.ToString() || luokkaVastaus == "2")
                    {
                        player.luokka = Class.Warrior;

                    }
                    else if (luokkaVastaus == Class.Magician.ToString() || luokkaVastaus == "3")
                    {
                        player.luokka = Class.Magician;

                    }
                    else
                    {
                        Console.WriteLine("Ei kelpaa");
                    }

                    string rotuVastaus = raceChoiceEntry.ToString();
                    if (rotuVastaus == Race.Human.ToString() || rotuVastaus == "1")
                    {
                        player.rotu = Race.Human;

                    }
                    else if (rotuVastaus == Race.Elf.ToString() || rotuVastaus == "2")
                    {
                        player.rotu = Race.Elf;

                    }
                    else if (rotuVastaus == Race.Orc.ToString() || rotuVastaus == "3")
                    {
                        player.rotu = Race.Orc;

                    }
                    else
                    {
                        Console.WriteLine("Ei kelpaa");
                    }

                }
            }
            creator.EndMenu();
        }
        public void DrawMainMenu()
        {
            int menuWidth = 200;
            int menuX = Raylib.GetScreenWidth() / 2 - menuWidth / 2;
            int menuY = 10;
            int rowHeight = Raylib.GetScreenHeight() / 10;
            MenuCreator creator = new MenuCreator(menuX, menuY, rowHeight, menuWidth);

            creator.Label("Main Menu");
            if (creator.Button("Start Game"))
            {
                stateStack.Push(GameState.CharacterCreation);  
            }
            creator.Label("Options Menu");
            if (creator.Button("Options"))
            {
                stateStack.Push(GameState.OptionsMenu); 
            }
        }




        public bool TestName(string nimi)
        {
            if (string.IsNullOrEmpty(nimi))
            {
                Console.WriteLine("Ei Kelpaa");
                return false;
            }
            bool nameOk = true;
            for (int i = 0; i < nimi.Length; i++)
            {
                char kirjain = nimi[i];
                if (Char.IsLetter(kirjain) == false)
                {
                    nameOk = false;
                    Console.WriteLine("Nimessä täytyy olla vain kirjaimia");
                    break;
                }


            }
            return nameOk;
        }
        private PlayerCharacter CreateCharacter()
        {
            PlayerCharacter player = new PlayerCharacter();

           
            return player;
        }

        public void Run()
        {

            Console.Clear();
            Init();
            GameLoop();


        }
        public void onOptionsBackPressed(object sender, EventArgs e)
        {
            stateStack.Pop();
        }

        public void onPauseBackPressed(object sender, EventArgs e)
        {
            stateStack.Pop();
        }
        
        public void onOptionsPressed(object sender, EventArgs e)
        {
            stateStack.Push(GameState.OptionsMenu);
        }
        private void Init()
        {
            stateStack.Push(GameState.MainMenu);
            myPauseMenu = new PauseMenu();
            myPauseMenu.OptionsPressed += onOptionsPressed;
            myPauseMenu.BackButtonPressedEvent += onPauseBackPressed;
            myOptionsMenu = new OptionsMenu();
            myOptionsMenu.BackButtonPressedEvent += onOptionsBackPressed;
            playerNameEntry = new TextBoxEntry(12);
            classChoiceEntry = new MultipleChoiceEntry(new string[] { "Rogue", "Warrior", "Magician" });
            raceChoiceEntry = new MultipleChoiceEntry(new string[] { "Human", "Elf", "Orc" });

            player = CreateCharacter();

            player.paikka = new Vector2(3, 3);

            MapReader reader = new MapReader();
            level1 = reader.LoadMapFromFile("mapfile_layers.json");
            level2 = reader.LoadMapFromFile("mapfile_layers.json");
            Map tiledMap = reader.LoadTiled();
            currentlevel = tiledMap;

            Raylib.InitWindow(480, 270, "rogue");
            Raylib.SetTargetFPS(30);

            Texture imageTexture = Raylib.LoadTexture("tilemap_packed.png");
            player.spritesheet = imageTexture;
            level1.SetSpriteSheet(imageTexture, 12);
            level2.SetSpriteSheet(imageTexture, 12);
            tiledMap.SetSpriteSheet(imageTexture, 12);

            level1.LoadEnemies();
            level2.LoadEnemies();
            tiledMap.LoadEnemies();

            level1.LoadItems();
            level2.LoadItems();
            tiledMap.LoadItems();
        }

        private void DrawGame()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            currentlevel.draw();
            player.draw();




            Raylib.EndDrawing();
        }

        private void UpdateGame()
        {



            // switch(key) ends
            int moveX = 0;
            int moveY = 0;
            // Wait for keypress and compare value to ConsoleKey enum

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
            {
                moveY = -1;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                moveY = 1;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
            {
                moveX = -1;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
            {
                moveX = 1;
            }

            int newX = (int)player.paikka.X + moveX;
            int newY = (int)player.paikka.Y + moveY;
            int tile = currentlevel.getTile(newX, newY);
            if (tile == 1)
            {
                player.paikka.X += moveX;
                player.paikka.Y += moveY;

                Enemy E = currentlevel.GetEnemyAt(newX, newY);
                if (E != null)
                {
                    Console.WriteLine("You found an enemy");
                }
                Item I = currentlevel.GetItemAt(newX, newY);
                if (I != null)
                {
                    Console.WriteLine("You found an Item");
                }
            }
            if (tile == 3)
            {
                Console.Clear();
                player.paikka = new Vector2(1, 1);
                currentlevel = level2;
                Console.SetCursorPosition((int)player.paikka.X, (int)player.paikka.Y);
                Console.Write("@");
            }
            if (player.paikka.X < 0)
            {
                player.paikka.X = 0;
            }
            else if (player.paikka.X > Console.WindowWidth - 1)
            {
                player.paikka.X = Console.WindowWidth - 1;
            }
            if (player.paikka.Y < 0)
            {
                player.paikka.Y = 0;
            }
            else if (player.paikka.Y > Console.WindowHeight - 1)
            {
                player.paikka.Y = Console.WindowHeight - 1;
            }

        }

        private void GameLoop()
        {
            while (Raylib.WindowShouldClose() == false)
            {
                // Kuuntele näppäimiä kuuluuko mennä pause menuun
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
                {
                    stateStack.Push(GameState.PauseMenu);
                }
                switch (stateStack.Peek())
                {
                    case GameState.MainMenu:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.BLACK);
                        DrawMainMenu();
                        Raylib.EndDrawing();
                        break;
                    case GameState.CharacterCreation:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.BLACK);
                        DrawCharaterCreationMenu();
                        Raylib.EndDrawing();
                        break;
                    case GameState.GameLoop:
                        DrawGame();
                        UpdateGame();
                        break;
                    case GameState.OptionsMenu:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.BLACK);
                        myOptionsMenu.DrawMenu();
                        Raylib.EndDrawing();
                        break;
                    case GameState.PauseMenu:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.BLACK);
                        myPauseMenu.DrawMenu();
                        Raylib.EndDrawing();
                        break;



                }

            }
        }


    }
}
