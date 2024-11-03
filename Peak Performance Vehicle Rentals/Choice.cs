using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

//To-Do List:
//1. create separate files for each user (done 10/19/24)
//2. change choices pina nga mag arrow keys ka (done)

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Choice: AbstractChoice, IChoice
    {
        public int LoginRegisterChoice() //CHOICE METHOD 1: login and register
        {
            Prompt = @"
╔═╗┌─┐┌─┐┬┌─  ╔═╗┌─┐┬─┐┌─┐┌─┐┬─┐┌┬┐┌─┐┌┐┌┌─┐┌─┐    ╦  ╦┌─┐┬ ┬┬┌─┐┬  ┌─┐  ╦═╗┌─┐┌┐┌┌┬┐┌─┐┬  ┌─┐
╠═╝├┤ ├─┤├┴┐  ╠═╝├┤ ├┬┘├┤ │ │├┬┘│││├─┤││││  ├┤     ╚╗╔╝├┤ ├─┤││  │  ├┤   ╠╦╝├┤ │││ │ ├─┤│  └─┐
╩  └─┘┴ ┴┴ ┴  ╩  └─┘┴└─└  └─┘┴└─┴ ┴┴ ┴┘└┘└─┘└─┘     ╚╝ └─┘┴ ┴┴└─┘┴─┘└─┘  ╩╚═└─┘┘└┘ ┴ ┴ ┴┴─┘└─┘

|                                                          _________________________     |
|                     /\\      _____          _____       |   |     |     |    | |  \    |
|      ,-----,       /  \\____/__|__\_    ___/__|__\___   |___|_____|_____|____|_|___\   |
|   ,--'---:---`--, /  |  _     |     `| |      |      `| |                    | |    \  |
|  ==(o)-----(o)==J    `(o)-------(o)=   `(o)------(o)'   `--(o)(o)--------------(o)--'  |

""Rent, Ride, Repeat""
(use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] {"Login", "Register", "About", "Exit"};

            UserInterface LR = new UserInterface(Prompt, Options);
            int choice = LR.RunUserInterface("all");
            return choice;
        }
        public int MainMenuChoice(string username) //CHOICE METHOD 2: main menu
        {
            Prompt = @$"
 _______ _______ _____ __   _      _______ _______ __   _ _     _
 |  |  | |_____|   |   | \  |      |  |  | |______ | \  | |     |
 |  |  | |     | __|__ |  \_|      |  |  | |______ |  \_| |_____|

Hello {username}! What would you like to do today?
(use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] { "View rentable vehicles", "View rental details", "Manage vehicles", "Manage User Account", "Logout", "Exit Program" };

            UserInterface MM = new UserInterface(Prompt, Options);
            int choice = MM.RunUserInterface("all");
            return choice;
        }
        public int ViewAllVehiclesChoice(FilePathManager file) //CHOICE METHOD 3: view all vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = @"
____ ____ _    ____ ____ ___    ____    _  _ ____ _  _ _ ____ _    ____
[__  |___ |    |___ |     |     |__|    |  | |___ |__| | |    |    |___
___] |___ |___ |___ |___  |     |  |     \/  |___ |  | | |___ |___ |___

Shown below are the vehicles available for rent. Please select a vehicle to view its details.
(use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = inventory.ViewVehicles(file);

            UserInterface VAV = new UserInterface(Prompt, Options);
            int choice = VAV.RunUserInterface("all");
            return choice;
        }
        public int ViewOwnedVehiclesChoice(string username, FilePathManager file) //CHOICE METHOD 4: view owned vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = @$"
____ ____ _    ____ ____ ___    ____    _  _ ____ _  _ _ ____ _    ____
[__  |___ |    |___ |     |     |__|    |  | |___ |__| | |    |    |___
___] |___ |___ |___ |___  |     |  |     \/  |___ |  | | |___ |___ |___

Only vehicles owned by {username} are displayed.
(use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = inventory.ViewVehicles(username, file);

            UserInterface VOV = new UserInterface(Prompt, Options);
            int choice = VOV.RunUserInterface("all");
            return choice;
        }
        public int VehicleRentChoice(string vehicleOwner, string username) //CHOICE METHOD ?: ????
        {

            UserInterface.CenterTextMargin(3, 0);
            Prompt = "Do you want to rent this vehicle?";
            Options = new string[] { "Rent this vehicle", "Choose another vehicle" };

            UserInterface VR = new UserInterface(Prompt, Options);
            int choice = VR.RunUserInterface("rent");

            if (choice == 0 && vehicleOwner == username)
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You cannot rent a car that you own!"); Thread.Sleep(1000);
                Console.ResetColor();
                UserInterface UI = new UserInterface("Press any key to select another vehicle");
                UI.WaitForKey();
                choice = 1;
            }

            return choice;
        }
        public int ViewPendingChoice(string username, FilePathManager file) //CHOICE METHOD
        {
            Inventory inventory = new Inventory();
            Prompt = @"
____ ____ _    ____ ____ ___    ____    _  _ ____ _  _ _ ____ _    ____
[__  |___ |    |___ |     |     |__|    |  | |___ |__| | |    |    |___
___] |___ |___ |___ |___  |     |  |     \/  |___ |  | | |___ |___ |___

Shown below are the pending vehicles. Please select a vehicle to view its details.
(use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = inventory.ViewPendingRental(username, file);

            UserInterface VAV = new UserInterface(Prompt, Options);
            int choice = VAV.RunUserInterface("all");
            return choice;
        }
        public int RentalTimeChoice() //CHOICE METHOD ?: ????
        {
            UserInterface.CenterTextMargin(3, 0);
            Prompt = "How long do you plan on renting the vehicle?";
            Options = new string[] { "A couple of days", "A few hours" };

            UserInterface RD = new UserInterface(Prompt, Options);
            int choice = RD.RunUserInterface("rent");
            return choice;
        }
        public int RentalDetailsChoice() //CHOICE METHOD ?: ????
        {
            UserInterface.CenterTextMargin(3, 0);
            Prompt = @"
____ ____ _  _ ___ ____ _       ___  ____ ___ ____ _ _    ____ 
|__/ |___ |\ |  |  |__| |       |  \ |___  |  |__| | |    [___
|  \ |___ | \|  |  |  | |___    |__/ |___  |  |  | | |___ ___]

(use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] { "View pending rental applications", "View approved rental applications", "Manage the vehicle you are currently renting", "Go back to main menu" };

            UserInterface RD = new UserInterface(Prompt, Options);
            int choice = RD.RunUserInterface("all");
            return choice;
        }
        public int ApprovePendingChoice() //CHOICE METHOD ?: ????
        {
            UserInterface.CenterTextMargin(3, 0);
            Prompt = "Do you want to approve this request?";
            Options = new string[] { "YES", "NO" };

            UserInterface RD = new UserInterface(Prompt, Options);
            int choice = RD.RunUserInterface("rent");
            return choice;
        }
        public int CurrentlyRentingChoice(string username, FilePathManager file) //CHOICE METHOD ?: ????
        {
            Inventory inventory = new Inventory();
            string vehicle = "";
            vehicle = inventory.CurrentRental(username, file);
            int choice = 2;

            if (vehicle != "")
            {
                Prompt = "What would you like to do your current rental vehicle?";
                Options = new string[] { "View reciept", "Finish renting the vehicle", "Go back to main menu" };

                UserInterface RD = new UserInterface(Prompt, Options);
                choice = RD.RunUserInterface("current");
            }
            else
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You are currently not renting a car"); Thread.Sleep(1000);
                Console.ResetColor();
                choice = 2;
            }
            return choice;
        }

        public int ManageVehiclesChoice() //CHOICE METHOD 5: manage vehicles
        {

            Prompt = @"
_  _ ____ _  _ ____ ____ ____    _  _ ____ _  _ _ ____ _    ____ ____ 
|\/| |__| |\ | |__| | __ |___    |  | |___ |__| | |    |    |___ [___
|  | |  | | \| |  | |__] |___     \/  |___ |  | | |___ |___ |___ ___]           

(use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] { "Add your own rentable vehicle", "Update vehicles", "Delete vehicles", "Go back to main menu" };

            UserInterface MV = new UserInterface(Prompt, Options);
            int choice = MV.RunUserInterface("all");
            return choice;
        }
        public string UpdateVehicleDetailsChoice(string username, FilePathManager file, int choice) //CHOICE METHOD 6: update vehicle details
        {
            Inventory inventory = new Inventory();
            Prompt = "Select a detail that you want to change";
            Options = inventory.ViewVehicleDetails(username, file, choice);
            if (Options[0] != "")
            {
                UserInterface VAV = new UserInterface(Prompt, Options);
                string detailchoice = VAV.RunUserInterfaceString("all");
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
        public string VehicleTypeChoice() //CHOICE METHOD 7: vehicle type
        {
            Prompt = "Choose vehicle type";
            Options = new string[] { "Car", "Motorcycle" };

            UserInterface VT = new UserInterface(Prompt, Options);
            int choiceVT = VT.RunUserInterface("all");

            if (choiceVT == 0)
            {
                Prompt = "Choose Car type";
                string[] optionVTCar = { "Sedan", "SUV", "Coupe", "Convertible", "Hatchback", "Minivan", "Pickup Truck", "Limousine", "Sports Car", "Luxury Car" };

                UserInterface VTCar = new UserInterface(Prompt, optionVTCar);
                return $"Car-{VTCar.RunUserInterfaceString("all")}";

            }
            else if (choiceVT == 1)
            {
                Prompt = "Choose Motorcycle type: ";
                string[] optionVTMotorcycle = { "Underbone", "Scooter", "Naked", "Motocross", "Cafe Racer", "Chopper", "Tourer", "Sports Bike" };

                UserInterface VTMotorcycle = new UserInterface(Prompt, optionVTMotorcycle);
                return $"Motorcycle-{VTMotorcycle.RunUserInterfaceString("all")}";

            }
            else
                return "";
        }
        public string VehicleFuelChoice() //CHOICE METHOD 8: vehicle fuel
        {
            Prompt = "Choose fuel type";
            Options = new string[] { "Gasoline", "Diesel", "Electric", "Hybrid", "Hydrogen" };

            UserInterface VF = new UserInterface(Prompt, Options);
            return VF.RunUserInterfaceString("all");
        }
        public string VehicleStatusChoice() //CHOICE METHOD 9: vehicle status
        {
            Prompt = "Choose vehicle status";
            Options = new string[] { "Available", "In Maintenance", "Reserved" };

            UserInterface VS = new UserInterface(Prompt, Options);
            return VS.RunUserInterfaceString("all");
        }
        public int ManageUserChoice() //CHOICE METHOD 10: manage user
        {
            Prompt = @"
_  _ ____ _  _ ____ ____ ____    _  _ ____ ____ ____ 
|\/| |__| |\ | |__| | __ |___    |  | [__  |___ |__/ 
|  | |  | | \| |  | |__] |___    |__| ___] |___ |  \ 

(use the UP or DOWN arrow keys to navigate, press ENTER to select)
";
            Options = new string[] { "View account details", "Update account details", "Delete account", "Go back to main menu" };

            UserInterface MM = new UserInterface(Prompt, Options);
            int choice = MM.RunUserInterface("all");
            return choice;
        }
        public string UpdateUserDetailsChoice(string username, FilePathManager file) //CHOICE METHOD 11: update user details
        {
            Inventory inventory = new Inventory();
            Prompt = "Select a detail that you want to change";
            Options = inventory.ViewUserDetails(username, file);
            UserInterface UD = new UserInterface(Prompt, Options);
            string detailchoice = UD.RunUserInterfaceString("all");
            if (detailchoice != "Go back to Main Menu")
            {
                string[] detailchoicepart = detailchoice.Split(": ");
                return detailchoicepart[0];
            }
            else return "";
        }
        public int DeleteUserChoice(string username, FilePathManager file) //CHOICE METHOD 12: delete user
        {
            Inventory inventory = new Inventory();
            Prompt = "Are you sure that you want to delete your account? This will also remove all vehicles owned by you.";
            Options = new string[] { "I AM SURE IN DELETING MY ACCOUNT", "Go back to main menu" };
            UserInterface DU = new UserInterface(Prompt, Options);
            int choice = DU.RunUserInterface("all");
            return choice;
        }
    }
}