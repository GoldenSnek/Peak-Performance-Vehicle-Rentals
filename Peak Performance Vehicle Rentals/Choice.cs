using System;
using System.IO;

//To-Do List:
//1. create separate files for each user

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Choice
    {
        public static int LoginRegisterChoice() //choice method 1: login and register
        {
            bool success = false;
            int choice = 1;
            ConsoleKeyInfo key;

            Console.WriteLine("(1) Login");
            Console.WriteLine("(2) Register");
            Console.WriteLine("(0) Exit");
            do
            {
                Console.Write("Enter Choice: ");
                key = Console.ReadKey();

                //quickly exit program
                /*
                if (key.Key == ConsoleKey.Escape)
                {2
                    Console.Clear();
                    System.Environment.Exit(0);
                }
                */

                success = int.TryParse(key.KeyChar.ToString(), out choice);
                if (!success || choice < 0 || choice > 2)
                {
                    Console.WriteLine("\nPlease press (1) (2) (0) keys only!");
                }
                else if (choice == 0)
                {
                    Console.WriteLine("\nThank you for using the program!");
                    Console.WriteLine("I hope that this program helped you!");
                    Console.WriteLine("Now exiting program...");

                    System.Environment.Exit(0);
                }

            } while (!success || choice < 1 || choice > 2);

            Console.WriteLine("");

            return choice;
        }
    }
}