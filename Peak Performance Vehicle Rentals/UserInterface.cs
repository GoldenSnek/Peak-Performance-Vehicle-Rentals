using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class UserInterface : UserInterfaceBase, IUserInterface
    {
        internal static int position;

        //CONSTRUCTOR
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
                else if (type == "rent" || type == "delete")
                {
                    if (ctr == 0)
                        CenterTextMargin(3 , 6);
                    ctr++;
                    ClearLineRUI(5);
                    verbatim = false;
                }
                else if (type == "vehicle type" || type == "pending" || type == "register")
                {
                    if (ctr == 0)
                        CenterTextMargin(3, 8);
                    ctr++;
                    ClearLineRUI(7);
                    verbatim = false;
                }
                else if (type == "car type")
                {
                    if (ctr == 0)
                        CenterTextMargin(3, 22);
                    ctr++;
                    ClearLineRUI(21);
                    verbatim = false;
                }
                else if (type == "motorcycle type")
                {
                    if (ctr == 0)
                        CenterTextMargin(3, 18);
                    ctr++;
                    ClearLineRUI(17);
                    verbatim = false;
                }
                else if (type == "vehicle fuel")
                {
                    if (ctr == 0)
                        CenterTextMargin(3, 12);
                    ctr++;
                    ClearLineRUI(11);
                    verbatim = false;
                }
                else if (type == "current")
                {
                    if (ctr == 0)
                        CenterTextMargin(3, 8);
                    ctr++;
                    ClearLineRUI(7);
                    verbatim = true;
                }

                DisplayOptions(verbatim);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                //arrow up/down based menu
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
        public void DisplayOptions(bool verbatim) //SUPPORTING METHOD for RunUserInterface, displays the options to the screen
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
            //Console.WriteLine("\ndecription here chuchuchu"); //Version 2 puhon
            Console.WriteLine();
            Console.ResetColor();
        }
        private static void ClearAllRUI() //SUPPORTING METHOD, clear all lines without flicker
        {
            Console.CursorVisible = false;
            for (int y = Console.WindowTop; y < Console.WindowHeight; y++)
            {
                Console.SetCursorPosition(Console.WindowLeft, y);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 0);
        }
        private static void ClearLineRUI(int ctr) //SUPPORTING METHOD, clear specific amount of lines
        {
            try
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                for (int i = 0; i < ctr; i++)
                {
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
            }
            catch (Exception e) {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        internal static int CenterVerbatimText(string text) //SUPPORTING and EXTRA METHOD, center a verbatim text
        {
            var lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int windowWidth = Console.WindowWidth;
            int position = 0;
            int ctr = 0;
            try
            {
                foreach (var line in lines)
                {
                    string trimmedLine = line.Trim();
                    position = (windowWidth - trimmedLine.Length) / 2;

                    if (position < 0 || position >= Console.WindowWidth) position = 0; //ensure we don't go out of bounds

                    Console.SetCursorPosition(position, Console.CursorTop);

                    if (ctr < 4) Console.ForegroundColor = ConsoleColor.DarkYellow;

                    Console.WriteLine(trimmedLine);
                    ctr++;

                    if (ctr >= 4) Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
            return position;
        }
        internal static void CenterTextMargin(int x, int y) //SUPPORTING and EXTRA METHOD, specify where to write text
        {
            if (position + x > Console.WindowWidth) //make sure system doesn't break
                return;
            try
            {
                Console.SetCursorPosition(position + x, Console.CursorTop + y);
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        internal static void WriteColoredText(int x, int y, string color, string text) //EXTRA METHOD, write a static colored text
        {
            if (position + x > Console.WindowWidth) //make sure system doesn't break
                return;
            try
            {
                Console.CursorVisible = false;
                UserInterface.CenterTextMargin(x, y);
                if (color == "red")
                    Console.ForegroundColor = ConsoleColor.Red;
                if (color == "green")
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(text);
                Console.ResetColor();
                Thread.Sleep(1500);
                ClearLineRUI(1);
                Console.CursorVisible = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        internal static void WaitForKey(int x, int y, string text) //EXTRA METHOD, wait for any key to be pressed
        {
            if (position + x > Console.WindowWidth) //make sure system doesn't break
                return;
            try
            {
                CenterTextMargin(x, y);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(text);
                Console.ResetColor();
                Console.ReadKey(true);
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
    }
}