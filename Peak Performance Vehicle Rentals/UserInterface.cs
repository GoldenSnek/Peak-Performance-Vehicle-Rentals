using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class UserInterface
    {
        private int choice;
        private string[] options;
        private string prompt;

        public UserInterface(string prompt, string[] options)
        {
            this.prompt = prompt;
            this.options = options;
            this.choice = 0;
        }
        public int RunUserInterface() //main method
        {

            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    choice--;
                    if (choice == -1)
                        choice = options.Length - 1;
                }
                if (keyPressed == ConsoleKey.DownArrow)
                {
                    choice++;
                    if (choice == options.Length)
                        choice = 0;
                }

            } while (keyPressed != ConsoleKey.Enter);

            return choice;
        }

        public void DisplayOptions() //method supporting RunUseInterface
        {
            Console.WriteLine(prompt);
            for (int i = 0; i < options.Length; i++)
            {
                string currentOption = options[i];
                string[] style = new string[2];

                if (i == choice)
                {
                    style[0] = "<<";
                    style[1] = ">>";
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    style[0] = "  ";
                    style[1] = "  ";
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine($"{style[0]} {currentOption} {style[1]}");
                
            }
            Console.ResetColor();
        }

        public static void WaitForKeyLR() //extra method
        {
            Console.Write("Press any key to return to Login Screen");
            Console.ReadKey(true);
        }
        public static void WaitForKey() //extra method
        {
            Console.Write("Press any key once you are finished reading the details");
            Console.ReadKey(true);
        }


        public static void WaitForSpecificKey() //extra method
        {
            Console.Write("Press any [specific key] to [do this and that]");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            ConsoleKey keyPressed = keyInfo.Key;
            while ( keyPressed != ConsoleKey.Enter) {
                //do nothing
            }
        }






    }
}
