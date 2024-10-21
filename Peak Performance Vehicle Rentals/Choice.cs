using System;
using System.IO;

//To-Do List:
//1. create separate files for each user (done 10/19/24)

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

                //quickly exit program [optional]
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    Console.WriteLine("xThank you for using the program!");
                    Console.WriteLine("Now exiting program...");
                    System.Environment.Exit(0);
                }

                success = int.TryParse(key.KeyChar.ToString(), out choice);
                if (!success || choice < 0 || choice > 2)
                {
                    Console.WriteLine("\nPlease press (1) (2) (0) (Esc) keys only!");
                }

            } while (!success || choice < 0 || choice > 2);

            Console.WriteLine("");

            return choice;
        }

        public static int MainMenuChoice() //choice method 2: main menu
        {
            bool success = false;
            int choice = 1;
            ConsoleKeyInfo key;


            Console.WriteLine("MAIN MENU");
            Console.WriteLine("(1) View vehicles");
            Console.WriteLine("(2) View rental details");
            Console.WriteLine("(3) Manage vehicles");
            Console.WriteLine("(4) Help");
            Console.WriteLine("(0) Go back to Login screen");
            Console.WriteLine("(Esc) Exit program");
            do
            {
                Console.Write("Enter Choice: ");
                key = Console.ReadKey();

                //quickly exit program [optional]
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    Console.WriteLine("xThank you for using the program!");
                    Console.WriteLine("Now exiting program...");
                    System.Environment.Exit(0);
                }

                success = int.TryParse(key.KeyChar.ToString(), out choice);
                if (!success || choice < 0 || choice > 4)
                {
                    Console.WriteLine("\nPlease press (1) (2) (3) (4) (0) (Esc) keys only!");
                }

            } while (!success || choice < 0 || choice > 4);

            Console.WriteLine("");

            return choice;
        }

        public static int ManageVehiclesChoice() //choice method ?: manage vehicles
        {
            bool success = false;
            int choice = 1;
            ConsoleKeyInfo key;

            Console.WriteLine("(1) Add your own rentable vehicle");
            Console.WriteLine("(2) Update vehicles");
            Console.WriteLine("(3) Delete vehicles");
            Console.WriteLine("(0) Go back to main menu");
            do
            {
                Console.Write("Enter Choice: ");
                key = Console.ReadKey();

                //quickly exit program [optional]
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    Console.WriteLine("xThank you for using the program!");
                    Console.WriteLine("Now exiting program...");
                    System.Environment.Exit(0);
                }

                success = int.TryParse(key.KeyChar.ToString(), out choice);
                if (!success || choice < 0 || choice > 3)
                {
                    Console.WriteLine("\nPlease press (1) (2) (3) (0) keys only!");
                }

            } while (!success || choice < 0 || choice > 3);

            Console.WriteLine("");

            return choice;
        }

        public static int ViewAllVehiclesChoice(string[] files) //choice method ?: view a specific vehicles
        {
            bool success = false;
            int choice = 1;
            ConsoleKeyInfo key;

            do
            {
                Console.WriteLine("(0) Go back to main menu");
                Console.Write("Enter Choice: ");
                key = Console.ReadKey();

                //quickly exit program [optional]
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    System.Environment.Exit(0);
                }

                success = int.TryParse(key.KeyChar.ToString(), out choice);
                if (!success || choice < 0 || choice > files.Length)
                {
                    Console.WriteLine("\nVehicle does not exist!");
                }

            } while (!success || choice < 0 || choice > files.Length);

            Console.WriteLine("");

            return choice;
        }

        public static int ViewOwnedVehiclesChoice(int ctr) //choice method ?: view owned vehicles
        {
            bool success = false;
            int choice = 1;
            ConsoleKeyInfo key;

            do
            {
                Console.WriteLine("(0) Go back to main menu");
                Console.Write("Enter Choice: ");
                key = Console.ReadKey();

                //quickly exit program [optional]
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    System.Environment.Exit(0);
                }

                success = int.TryParse(key.KeyChar.ToString(), out choice);
                if (!success || choice < 0 || choice > ctr)
                {
                    Console.WriteLine("\nVehicle does not exist!");
                }

            } while (!success || choice < 0 || choice > ctr);

            Console.WriteLine("");

            return choice;
        }

    }
}