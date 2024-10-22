using System;
using System.IO;

//To-Do List:
//1. create separate files for each user (done 10/19/24)
//2. change choices pina noah style nga mag arrow keys ka

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Choice
    {
        public static int LoginRegisterChoice() //choice method 1: login and register
        {
            bool success = false;
            int choice;
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
            int choice;
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

        public static string VehicleTypeChoice() //choice method ?: view owned vehicles
        {
            bool success = false;
            int choice;

            do
            {
                Console.WriteLine("Choose vehicle type: ");
                Console.WriteLine("Car:");
                Console.WriteLine("(1) Sedan");
                Console.WriteLine("(2) SUV");
                Console.WriteLine("(3) Coupe");
                Console.WriteLine("(4) Convertible");
                Console.WriteLine("(5) Hatchback");
                Console.WriteLine("(6) Minivan");
                Console.WriteLine("(7) Pickup Truck");
                Console.WriteLine("(8) Limousine");
                Console.WriteLine("(9) Sports Car");
                Console.WriteLine("(10) Luxury Car");
                Console.WriteLine("Motorcycle: ");
                Console.WriteLine("(11) Underbone");
                Console.WriteLine("(12) Scooter");
                Console.WriteLine("(13) Naked");
                Console.WriteLine("(14) Off-road");
                Console.WriteLine("(15) Cafe Racer");
                Console.WriteLine("(16) Chopper");
                Console.WriteLine("(17) Tourer");
                Console.WriteLine("(18) Sports Bike");
                Console.Write("Enter Choice: ");
                success = int.TryParse(Console.ReadLine(), out choice);
                if (!success || choice < 1 || choice > 18)
                {
                    Console.WriteLine("\nPlease choose one of the vehicle types!");
                }

            } while (!success || choice < 1 || choice > 18);

            Console.WriteLine("");

            if (choice == 1)
                return "Car-Sedan";
            else if (choice == 2)
                return "Car-SUV";
            else if (choice == 3)
                return "Car-Coupe";
            else if (choice == 4)
                return "Car-Convertible";
            else if (choice == 5)
                return "Car-Hatchback";
            else if (choice == 6)
                return "Car-Minivan";
            else if (choice == 7)
                return "Car-Pickup Truck";
            else if (choice == 8)
                return "Car-Limousine";
            else if (choice == 9)
                return "Car-Sports Car";
            else if (choice == 10)
                return "Car-Luxury Car";
            else if (choice == 11)
                return "Motorcycle-Underbone";
            else if (choice == 12)
                return "Motorcycle-Scooter";
            else if (choice == 13)
                return "Motorcycle-Naked";
            else if (choice == 14)
                return "Motorcycle-Off-road";
            else if (choice == 15)
                return "Motorcycle-Cafer Racer";
            else if (choice == 16)
                return "Motorcyle-Chopper";
            else if (choice == 17)
                return "Motorcycle-Tourer";
            else if (choice == 18)
                return "Motorcycle-Sports Bike";
            else
                return "";
        }

        public static string VehicleFuelChoice() //choice method ?: view owned vehicles
        {
            bool success = false;
            int choice;

            do
            {
                Console.WriteLine("Choose fuel type: ");
                Console.WriteLine("(1) Gasoline");
                Console.WriteLine("(2) Diesel");
                Console.WriteLine("(3) Electric");
                Console.WriteLine("(4) Hybrid");
                Console.WriteLine("(5) Hydrogen");
                Console.Write("Enter Choice: ");
                success = int.TryParse(Console.ReadLine(), out choice);
                if (!success || choice < 1 || choice > 5)
                {
                    Console.WriteLine("\nPlease choose one of the vehicle types!");
                }

            } while (!success || choice < 1 || choice > 5);

            Console.WriteLine("");

            if (choice == 1)
                return "Gasoline";
            else if (choice == 2)
                return "Diesel";
            else if (choice == 3)
                return "Electric";
            else if (choice == 4)
                return "Hybrid";
            else if (choice == 5)
                return "Hydrogen";
            else
                return "";
        }

        public static string VehicleStatusChoice() //choice method ?: view owned vehicles
        {
            bool success = false;
            int choice;

            do
            {
                Console.WriteLine("Choose vehicle status: ");
                Console.WriteLine("(1) Available");
                Console.WriteLine("(2) In Maintenance");
                Console.WriteLine("(3) Reserved");
                Console.Write("Enter Choice: ");
                success = int.TryParse(Console.ReadLine(), out choice);
                if (!success || choice < 1 || choice > 3)
                {
                    Console.WriteLine("\nPlease choose one of the vehicle types!");
                }

            } while (!success || choice < 1 || choice > 3);

            Console.WriteLine("");

            if (choice == 1)
                return "Available";
            else if (choice == 2)
                return "In Maintenance";
            else if (choice == 3)
                return "Reserved";
            else
                return "";
        }

        public static int ViewAllVehiclesChoice(string[] files) //choice method ?: view a specific vehicles
        {
            bool success = false;
            int choice;
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
            int choice;
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