using System;
using System.IO;

//To-Do List:
//1. create separate files for each user (done 10/19/24)
//2. change choices pina nga mag arrow keys ka (done)

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Choice: AbstractChoice, IChoice
    {

        public int LoginRegisterChoice() //choice method 1: login and register
        {
            Prompt = "Welcome to the program";
            Options = new string[] {"Login", "Register", "About", "Exit"};

            UserInterface LR = new UserInterface(Prompt, Options);
            int choice = LR.RunUserInterface();
            return choice;
        }

        public int MainMenuChoice() //choice method 2: main menu
        {
            Prompt = "MAIN MENU";
            Options = new string[] { "View rentable vehicles", "View rental details", "Manage vehicles", "Manage User Account", "Logout", "Exit Program" };

            UserInterface MM = new UserInterface(Prompt, Options);
            int choice = MM.RunUserInterface();
            return choice;
        }
        public int ViewAllVehiclesChoice(FilePathManager file) //choice method ?: view all vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = "Select a vehicle that you want to view the details";
            Options = inventory.ViewVehicles(file);

            UserInterface VAV = new UserInterface(Prompt, Options);
            int choice = VAV.RunUserInterface();
            return choice;
        }
        public int ViewOwnedVehiclesChoice(string username, FilePathManager file) //choice method ?: view owned vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = "Select a vehicle that you want to manage";
            Options = inventory.ViewVehicles(username, file);

            UserInterface VOV = new UserInterface(Prompt, Options);
            int choice = VOV.RunUserInterface();
            return choice;
        }
        public int ManageVehiclesChoice() //choice method ?: manage vehicles
        {
            Prompt = "Manage Vehicles";
            Options = new string[] { "Add your own rentable vehicle", "Update vehicles", "Delete vehicles", "Go back to main menu" };

            UserInterface MV = new UserInterface(Prompt, Options);
            int choice = MV.RunUserInterface();
            return choice;
        }
        public string UpdateVehicleDetailsChoice(string username, FilePathManager file, int choice) //choice method ?: view all vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = "Select a detail that you want to change";
            Options = inventory.ViewVehicleDetails(username, file, choice);
            if (Options[0] != "")
            {
                UserInterface VAV = new UserInterface(Prompt, Options);
                string detailchoice = VAV.RunUserInterfaceString();
                if (detailchoice != "Go back to Main Menu")
                {
                    string[] detailchoicepart = detailchoice.Split(": ");
                    return detailchoicepart[0];
                }
                else return "";
            }
            else
                return "";
        }
        public string VehicleTypeChoice() //choice method ?: view owned vehicles
        {
            Prompt = "Choose vehicle type";
            Options = new string[] { "Car", "Motorcycle" };

            UserInterface VT = new UserInterface(Prompt, Options);
            int choiceVT = VT.RunUserInterface();

            if (choiceVT == 0)
            {
                Prompt = "Choose Car type";
                string[] optionVTCar = { "Sedan", "SUV", "Coupe", "Convertible", "Hatchback", "Minivan", "Pickup Truck", "Limousine", "Sports Car", "Luxury Car" };

                UserInterface VTCar = new UserInterface(Prompt, optionVTCar);
                return $"Car-{VTCar.RunUserInterfaceString()}";

            }
            else if (choiceVT == 1)
            {
                Prompt = "Choose Motorcycle type: ";
                string[] optionVTMotorcycle = { "Underbone", "Scooter", "Naked", "Motocross", "Cafe Racer", "Chopper", "Tourer", "Sports Bike" };

                UserInterface VTMotorcycle = new UserInterface(Prompt, optionVTMotorcycle);
                return $"Motorcycle-{VTMotorcycle.RunUserInterfaceString()}";

            }
            else
                return "";
        }
        public string VehicleFuelChoice() //choice method ?: view owned vehicles
        {
            Prompt = "Choose fuel type";
            Options = new string[] { "Gasoline", "Diesel", "Electric", "Hybrid", "Hydrogen" };

            UserInterface VF = new UserInterface(Prompt, Options);
            return VF.RunUserInterfaceString();
        }
        public string VehicleStatusChoice() //choice method ?: view owned vehicles
        {
            Prompt = "Choose vehicle status";
            Options = new string[] { "Available", "In Maintenance", "Reserved" };

            UserInterface VS = new UserInterface(Prompt, Options);
            return VS.RunUserInterfaceString();
        }
        public int ManageUserChoice() //choice method 2: main menu
        {
            Prompt = "User Management";
            Options = new string[] { "View account details", "Update account details", "Delete account", "Go back to main menu" };

            UserInterface MM = new UserInterface(Prompt, Options);
            int choice = MM.RunUserInterface();
            return choice;
        }
        public string UpdateUserDetailsChoice(string username, FilePathManager file) //choice method ?: view all vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = "Select a detail that you want to change";
            Options = inventory.ViewUserDetails(username, file);
            UserInterface UD = new UserInterface(Prompt, Options);
            string detailchoice = UD.RunUserInterfaceString();
            if (detailchoice != "Go back to Main Menu")
            {
                string[] detailchoicepart = detailchoice.Split(": ");
                return detailchoicepart[0];
            }
            else return "";
        }
        public int DeleteUserChoice(string username, FilePathManager file) //choice method ?: view all vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = "Are you sure that you want to delete your account? This will also remove all vehicles owned by you.";
            Options = new string[] { "I AM SURE IN DELETING MY ACCOUNT", "Go back to main menu" };
            UserInterface DU = new UserInterface(Prompt, Options);
            int choice = DU.RunUserInterface();
            return choice;
        }
    }
}