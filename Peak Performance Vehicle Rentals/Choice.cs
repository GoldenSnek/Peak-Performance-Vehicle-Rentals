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
        public int RentalChoice(FilePathManager file) //CHOICE METHOD 3: view all vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = @"
                    ____ ____ _  _ ___ ____ _       _  _ ____ _  _ _ ____ _    ____ ____ 
                    |__/ |___ |\ |  |  |__| |       |  | |___ |__| | |    |    |___ [___
                    |  \ |___ | \|  |  |  | |___     \/  |___ |  | | |___ |___ |___ ___] 

                    (use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] { "Search for a specific vehicle", "View all available vehicles", "Go back to Main Menu" };

            UserInterface VAV = new UserInterface(Prompt, Options);
            int choice = VAV.RunUserInterface("all");
            return choice;
        }
        public string ViewSearchedVehiclesChoice(string keyword, FilePathManager file) //CHOICE METHOD 3: view all vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = @"
                    ____ ____ _    ____ ____ ___    ____    _  _ ____ _  _ _ ____ _    ____
                    [__  |___ |    |___ |     |     |__|    |  | |___ |__| | |    |    |___
                    ___] |___ |___ |___ |___  |     |  |     \/  |___ |  | | |___ |___ |___

                    Shown below are the vehicles that matches the search. Please select a vehicle to view its details.

                    (use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = inventory.ViewSearchedVehicles(keyword, file);

            UserInterface VAV = new UserInterface(Prompt, Options);
            string choice = VAV.RunUserInterfaceString("all");
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
                Console.WriteLine("You cannot rent a vehicle that you own!"); Thread.Sleep(1500);
                UserInterface.WaitForKey(3, 0, "Press any key to select another vehicle");
                choice = 1;
            }

            return choice;
        }
        public int ViewPendingChoice(string username, FilePathManager file) //CHOICE METHOD
        {
            Inventory inventory = new Inventory();
            Prompt = @$"
                    ____ ____ _    ____ ____ ___    ____    _  _ ____ _  _ _ ____ _    ____
                    [__  |___ |    |___ |     |     |__|    |  | |___ |__| | |    |    |___
                    ___] |___ |___ |___ |___  |     |  |     \/  |___ |  | | |___ |___ |___

                    Shown below are the pending vehicles waiting for your approval. Select to view client details.

                    (use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = inventory.ViewPendingRentalOwner(username, file);

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
        public int RentalDetailsChoice(string username, FilePathManager file) //CHOICE METHOD ?: ????
        {
            Inventory inventory = new Inventory();
            string vehicle = inventory.ViewPendingRentalClient(username, file);
            Prompt = @$"
                    ____ ____ _  _ ___ ____ _       ___  ____ ___ ____ _ _    ____ 
                    |__/ |___ |\ |  |  |__| |       |  \ |___  |  |__| | |    [___
                    |  \ |___ | \|  |  |  | |___    |__/ |___  |  |  | | |___ ___]

                    Vehicle that you plan on renting currently waiting for approval: {vehicle}

                    (use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] { "View pending rental applications", "View approved rental applications", "Manage the vehicle you are currently renting", "Go back to Main Menu" };

            UserInterface RD = new UserInterface(Prompt, Options);
            int choice = RD.RunUserInterface("all");
            return choice;
        }
        public int ApprovePendingChoice() //CHOICE METHOD ?: ????
        {
            UserInterface.CenterTextMargin(3, 0);
            Prompt = "Do you want to approve this request?";
            Options = new string[] { "YES", "NO", "Decide later"};

            UserInterface RD = new UserInterface(Prompt, Options);
            int choice = RD.RunUserInterface("pending");
            return choice;
        }
        public int CurrentlyRentingChoice(string username, FilePathManager file) //CHOICE METHOD ?: ????
        {
            Inventory inventory = new Inventory();
            string vehicle = "";
            vehicle = inventory.ViewCurrentRental(username, file);
            int choice = 2;

            if (vehicle != "")
            {
                Console.Clear();
                Prompt = @$"
                                ____ _  _ ____ ____ ____ _  _ ___ _    _   _    ____ ____ _  _ ___ _ _  _ ____ 
                                |    |  | |__/ |__/ |___ |\ |  |  |     \_/     |__/ |___ |\ |  |  | |\ | | __ 
                                |___ |__| |  \ |  \ |___ | \|  |  |___   |      |  \ |___ | \|  |  | | \| |__] 
                                
                                                                {vehicle}

                                What would you like to do your current rental vehicle?

                                (use the UP or DOWN arrow keys to navigate, press ENTER to select)
";
                Options = new string[] { "View reciept", "Finish renting the vehicle", "Go back to Main Menu" };
                UserInterface RD = new UserInterface(Prompt, Options);
                choice = RD.RunUserInterface("all");
            }
            else
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You are currently not renting a vehicle"); Thread.Sleep(1500);
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
            Options = new string[] { "Add your own rentable vehicle", "Update vehicles", "Delete vehicles", "Go back to Main Menu" };

            UserInterface MV = new UserInterface(Prompt, Options);
            int choice = MV.RunUserInterface("all");
            return choice;
        }
        public string UpdateVehicleDetailsChoice(string username, FilePathManager file, int choice) //CHOICE METHOD 6: update vehicle details
        {
            Inventory inventory = new Inventory();
            Prompt = @"
                    _  _ ___  ___  ____ ___ ____    _  _ ____ _  _ _ ____ _    ____ 
                    |  | |__] |  \ |__|  |  |___    |  | |___ |__| | |    |    |___ 
                    |__| |    |__/ |  |  |  |___     \/  |___ |  | | |___ |___ |___ 
                                                                
                    Select a detail that you want to update.

                    (use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = inventory.ViewVehicleDetails(username, file, choice);
            if (Options[0] != "")
            {
                UserInterface VAV = new UserInterface(Prompt, Options);
                string detailchoice = VAV.RunUserInterfaceString("all");
                if (detailchoice != "Go back and select another vehicle")
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
            Options = new string[] { "Car", "Motorcycle", "Go back to Manage Vehicles Menu"};

            UserInterface VT = new UserInterface(Prompt, Options);
            int choiceVT = VT.RunUserInterface("vehicle type");

            if (choiceVT == 0)
            {
                Prompt = "Choose car type";
                string[] optionVTCar = { "Sedan", "SUV", "Coupe", "Convertible", "Hatchback", "Minivan", "Pickup Truck", "Limousine", "Sports Car", "Luxury Car" };

                UserInterface VTCar = new UserInterface(Prompt, optionVTCar);
                return $"Car-{VTCar.RunUserInterfaceString("car type")}";

            }
            else if (choiceVT == 1)
            {
                Prompt = "Choose motorcycle type";
                string[] optionVTMotorcycle = { "Underbone", "Scooter", "Naked", "Motocross", "Cafe Racer", "Chopper", "Tourer", "Sports Bike" };

                UserInterface VTMotorcycle = new UserInterface(Prompt, optionVTMotorcycle);
                return $"Motorcycle-{VTMotorcycle.RunUserInterfaceString("motorcycle type")}";

            }
            else
                return "";
        }
        public string VehicleFuelChoice() //CHOICE METHOD 8: vehicle fuel
        {
            Prompt = "Choose fuel type";
            Options = new string[] { "Gasoline", "Diesel", "Electric", "Hybrid", "Hydrogen" };

            UserInterface VF = new UserInterface(Prompt, Options);
            return VF.RunUserInterfaceString("vehicle fuel");
        }
        public int ManageUserChoice() //CHOICE METHOD 10: manage user
        {
            Prompt = @"
                    _  _ ____ _  _ ____ ____ ____    _  _ ____ ____ ____ 
                    |\/| |__| |\ | |__| | __ |___    |  | [__  |___ |__/ 
                    |  | |  | | \| |  | |__] |___    |__| ___] |___ |  \ 

                    (use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] { "View account details", "Update account details", "Delete account", "Go back to Main Menu" };

            UserInterface MM = new UserInterface(Prompt, Options);
            int choice = MM.RunUserInterface("all");
            return choice;
        }
        public string UpdateUserDetailsChoice(string username, FilePathManager file) //CHOICE METHOD 11: update user details
        {
            Inventory inventory = new Inventory();
            Prompt = @"
                    _  _ ___  ___  ____ ___ ____    ___  ____ ___ ____ _ _    ____ 
                    |  | |__] |  \ |__|  |  |___    |  \ |___  |  |__| | |    [___
                    |__| |    |__/ |  |  |  |___    |__/ |___  |  |  | | |___ ___] 

                    Select a detail that you want to change

                    (use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = inventory.ViewUserDetails(username, file);
            UserInterface UD = new UserInterface(Prompt, Options);
            string detailchoice = UD.RunUserInterfaceString("all");
            if (detailchoice != "Go back to Manage User Menu")
            {
                string[] detailchoicepart = detailchoice.Split(": ");
                return detailchoicepart[0];
            }
            else return "";
        }
        public int DeleteUserChoice(string username, FilePathManager file) //CHOICE METHOD 12: delete user
        {
            Inventory inventory = new Inventory();
            Prompt = @"
                    ___  ____ _    ____ ___ ____    _  _ ____ ____ ____ 
                    |  \ |___ |    |___  |  |___    |  | [__  |___ |__/ 
                    |__/ |___ |___ |___  |  |___    |__| ___] |___ |  \ 
        
                    Are you sure that you want to delete your account? This will also remove all vehicles owned by you.
                    PROCEED WITH CAUTION! 

                    (use the UP or DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] { "I AM SURE IN DELETING MY ACCOUNT", "Go back to Manage User Menu" };
            UserInterface DU = new UserInterface(Prompt, Options);
            int choice = DU.RunUserInterface("all");
            return choice;
        }
    }
}