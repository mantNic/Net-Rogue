using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO.MemoryMappedFiles;

namespace Rogue
{
    internal class Game
    {
        PlayerCharacter player = new PlayerCharacter();

       
        public string askName()
        {
            while (true)
            {
                Console.WriteLine("What is your characters name?");
                string nimi = Console.ReadLine();
                if (string.IsNullOrEmpty(nimi))
                {
                    Console.WriteLine("Ei Kelpaa");
                    continue;
                }
                bool nameOk = true;
                for (int i = 0; i < nimi.Length; i++)
                {
                    char kirjain = nimi[i];
                    if(Char.IsLetter(kirjain)== false)
                    {
                        nameOk = false;
                        Console.WriteLine("Nimessä täytyy olla vain kirjaimia");
                        break;
                    }
                   
                }

                if (nameOk)
                {
                    return nimi;
                }
            }

        }

       
        public void Run()
        { 
          
            MapReader reader = new MapReader();
            Map level1 = reader.LoadMapFromFile("mapfile.json");
            
            while (true)
            {
                Console.WriteLine("Select Race");
                Console.WriteLine("1 tai "+Race.Human.ToString());
                Console.WriteLine("2 tai "+Race.Elf.ToString());
                Console.WriteLine("3 tai "+Race.Orc.ToString());
                string rotuVastaus = Console.ReadLine();

                if (rotuVastaus == Race.Human.ToString() || rotuVastaus == "1")
                {
                    player.rotu = Race.Human;
                    break;
                }
                else if (rotuVastaus == Race.Elf.ToString() || rotuVastaus =="2")
                {
                    player.rotu = Race.Elf;
                    break;
                }
                else if (rotuVastaus == Race.Orc.ToString()|| rotuVastaus == "3")
                {
                    player.rotu = Race.Orc;
                    break;
                }
                else
                {
                    Console.WriteLine("Ei kelpaa");
                }
            }

            while (true)
            {
                Console.WriteLine("Select Class");
                Console.WriteLine("1 tai "+Class.Rogue.ToString());
                Console.WriteLine("2 tai "+Class.Warrior.ToString());
                Console.WriteLine("3 tai "+Class.Magician.ToString());
                string luokkaVastaus = Console.ReadLine();

                if (luokkaVastaus == Class.Rogue.ToString() || luokkaVastaus == "1")
                {
                    player.luokka = Class.Rogue;
                    break;
                }
                else if (luokkaVastaus == Class.Warrior.ToString() || luokkaVastaus == "2")
                {
                    player.luokka = Class.Warrior;
                    break;
                }
                else if (luokkaVastaus == Class.Magician.ToString() || luokkaVastaus == "3")
                {
                    player.luokka = Class.Magician;
                    break;
                }
                else
                {
                    Console.WriteLine("Ei kelpaa");
                }

            }

            player.name = askName();

            Console.Clear();
            player.paikka = new Vector2(1, 1);
            level1.draw();
            Console.SetCursorPosition((int)player.paikka.X, (int)player.paikka.Y);
            Console.Write("@");

            while (true)
            {
                // ------------Update:
                // Prepare to read movement input
                int moveX = 0;
                int moveY = 0;
                // Wait for keypress and compare value to ConsoleKey enum
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    moveY = -1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    moveY = 1;
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    moveX = -1;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    moveX = 1;
                }

                //
                // TODO: CHECK COLLISION WITH WALLS
                int newX = (int)player.paikka.X + moveX;
                int newY = (int)player.paikka.Y + moveY;
                int tile = level1.getTile(newX, newY);
                if (tile == 1)
                {
                    player.paikka.X += moveX;
                    player.paikka.Y += moveY;
                }
                

             
               
                
                // Prevent player from going outside screen
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

                // -----------Draw:
                // Clear the screen so that player appears only in one place
                Console.Clear();
                // Draw the player
                level1.draw();
                Console.SetCursorPosition((int)player.paikka.X, (int)player.paikka.Y);
                Console.Write("@");
            } // game loop ends


        }
    }
}
