using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class UserInterface : UserInterfaceBase, IUserInterface
    {
        internal static int position;

        //CONSTRUCTORS
        public UserInterface()
        {
        }
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

        //METHODS
        public int RunUserInterface(string type) //MAIN METHOD for running the interface, returns int
        {
            ConsoleKey keyPressed;
            int ctr = 0;
            bool verbatim = true;
            do
            {
                if (type == "all") //clear all
                    ClearAllRUI();
                else if (type == "rent")
                {
                    if (ctr == 0)
                        CenterTextMargin(3 , 6);
                    ctr++;
                    ClearLineRUI(5);
                    verbatim = false;
                }
                else if (type == "current")
                {
                    if (ctr == 0)
                        CenterTextMargin(3, 8);
                    ctr++;
                    ClearLineRUI(7);
                    verbatim = false;
                }

                DisplayOptions(verbatim);
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
        public string RunUserInterfaceString(string type) //MAIN METHOD for running the interface, returns string
        {
            RunUserInterface(type);
            return Options[Choice];
        }
        public void DisplayOptions(bool verbatim) //SUPPORTING MAIN METHOD for RunUserInterface, displays the options to the screen
        {
            if (verbatim)
                position = CenterVerbatimText(Prompt);
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                CenterTextMargin(3, 0);
                Console.WriteLine(Prompt);
                Console.ResetColor();
            }

            for (int i = 0; i < Options.Length; i++)
            {
                Console.WriteLine();
                string currentOption = Options[i];
                string[] style = new string[2];

                if (i == Choice)
                {
                    style[0] = "<<";
                    style[1] = ">>";
                    Console.SetCursorPosition(position, Console.CursorTop);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                {
                    style[0] = "  ";
                    style[1] = "  ";
                    Console.SetCursorPosition(position, Console.CursorTop);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine($"{style[0]} {currentOption} {style[1]}");
            }
            Console.WriteLine();
            Console.ResetColor();
        }
        internal static void ClearVisibleRegion() //SUPPORTING METHOD, reduces screen flicker
        {
            int cursorTop = Console.CursorTop;
            int cursorLeft = Console.CursorLeft;
            for (int y = Console.WindowTop; y < Console.WindowTop + Console.WindowHeight; y++)
            {
                Console.SetCursorPosition(Console.WindowLeft, y);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
        }
        internal static int CenterVerbatimText(string text) //SUPPORTING METHOD, center a verbatim text
        {
            // Split the text into lines
            var lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Get the console window width
            int windowWidth = Console.WindowWidth;
            int position = 0;

            foreach (var line in lines)
            {
                // Trim the line to avoid extra spaces
                string trimmedLine = line.Trim();
                // Calculate the starting position for centering the line
                position = (windowWidth - trimmedLine.Length) / 2;

                // Ensure we don't go out of bounds
                if (position < 0) position = 0;

                // Set the cursor position and write the line
                Console.SetCursorPosition(position, Console.CursorTop);
                Console.WriteLine(trimmedLine);
            }
            return position;
        }
        internal static void CenterTextMargin(int x, int y) //SUPPORTING METHOD, specify where to write text
        {
            Console.SetCursorPosition(position + x, Console.CursorTop + y);
        }
        public static void ClearAllRUI() //SUPPORTING METHOD, clear all lines
        {
            Console.CursorVisible = false;
            ClearVisibleRegion();
        }
        public static void ClearLineRUI(int ctr) //SUPPORTING and EXTRA METHOD, clear specific amount of lines
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            for (int i = 0; i < ctr; i++)
            {
                Console.Write(new string(' ', Console.BufferWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
        public static void WriteColoredText(int x, int y, string color, string text) //EXTRA METHOD, write a static text
        {
            Console.CursorVisible = false;
            UserInterface.CenterTextMargin(x, y);
            if (color == "red")
                Console.ForegroundColor = ConsoleColor.Red;
            if (color == "green")
                Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
            Thread.Sleep(1000);
            UserInterface.ClearLineRUI(1);
            Console.CursorVisible = true;
        }
        public static void WaitForKey(int x, int y, string text) //EXTRA METHOD, wait for any key to be pressed
        {
            CenterTextMargin(x, y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(text);
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}