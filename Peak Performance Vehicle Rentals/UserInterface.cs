using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class UserInterface : AbstractUserInterface
    {
        public UserInterface(string prompt)
        {
            Prompt = prompt;
        }
        public UserInterface(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
            Choice = 0;
        }
        public override int RunUserInterface() //MAIN METHOD for running the interface, returns int
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
                    Choice--;
                    if (Choice == -1)
                        Choice = Options.Length - 1;
                }
                if (keyPressed == ConsoleKey.DownArrow)
                {
                    Choice++;
                    if (Choice == Options.Length)
                        Choice = 0;
                }

            } while (keyPressed != ConsoleKey.Enter);

            return Choice;
        }

        public override string RunUserInterfaceString() //MAIN METHOD for running the interface, returns string
        {
            RunUserInterface();
            return Options[Choice];
        }

        public override void DisplayOptions() //method supporting RunUseInterface
        {
            Console.WriteLine(Prompt);
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string[] style = new string[2];

                if (i == Choice)
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
        public override void WaitForKey() //extra method
        {
            Console.Write(Prompt);
            Console.ReadKey(true);
        }
        public override void WaitForSpecificKey() //extra method
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
