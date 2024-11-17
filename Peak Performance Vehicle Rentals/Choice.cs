using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

//To-Do List:
//1. create separate files for each user (done 10/19/24)
//2. change choices pina nga mag arrow keys ka (done)

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Choice: ChoiceBase, IChoice
    {
        Inventory inventory = new Inventory();
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

                    (use the UP and DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] {"Login", "Register", "About", "Exit"};

            UserInterface LR = new UserInterface(Prompt, Options);
            return LR.RunUserInterface("all");
        }
        public string RegisterTypeChoice() //CHOICE METHOD 2: register type
        {
            Prompt = "Registration Type: ";
            Options = new string[] { "Client", "Vehicle Provider", "Go back to Menu" };

            UserInterface RT = new UserInterface(Prompt, Options);
            return RT.RunUserInterfaceString("register");
        }
        public int MainMenuChoice(string username, string type) //CHOICE METHOD 3: main menu
        {
            Prompt = @$"
                    _______ _______ _____ __   _      _______ _______ __   _ _     _
                    |  |  | |_____|   |   | \  |      |  |  | |______ | \  | |     |
                    |  |  | |     | __|__ |  \_|      |  |  | |______ |  \_| |_____|

                    Hello {username}! What would you like to do today? <{type}>

                    (use the UP and DOWN arrow keys to navigate, press ENTER to select)";

            int choice = 0;
            if (type == "Client")
            {

                Options = new string[] { "View rentable vehicles", "View rental details", "Manage User Account", "Logout", "Exit Program" };
                UserInterface MM = new UserInterface(Prompt, Options);
                choice = MM.RunUserInterface("all");
                if (choice >= 2) //change the choice so that it automatically matches the "skeleton" in the program.cs file
                    choice++;
            }
            else if (type == "Vehicle Provider")
            {
                Options = new string[] { "View rentable vehicles", "View rental details", "Manage vehicles", "Manage User Account", "Logout", "Exit Program" };
                UserInterface MM = new UserInterface(Prompt, Options);
                choice = MM.RunUserInterface("all");
            }
            else if (type == "Admin")
            {
                Options = new string[] { "View and manage ALL VEHICLES", "View and manage ALL USERS",  "Logout", "Exit Program" };
                UserInterface MM = new UserInterface(Prompt, Options);
                choice = MM.RunUserInterface("all");
                if (choice <= 1) //change the choice so that it automatically matches the "skeleton" in the program.cs file
                    choice += 6;
                else if (choice >= 2)
                    choice += 2;
            }
            return choice;
        }
        public int RentalChoice(FilePathManager file) //CHOICE METHOD 4: view rental vehicles
        {
            Prompt = @"
                    ____ ____ _  _ ___ ____ _       _  _ ____ _  _ _ ____ _    ____ ____ 
                    |__/ |___ |\ |  |  |__| |       |  | |___ |__| | |    |    |___ [___
                    |  \ |___ | \|  |  |  | |___     \/  |___ |  | | |___ |___ |___ ___] 

                    Search for specefic vehicles or view all vehicles in the system.";
            Options = new string[] { "Search for a specific vehicle", "View all available vehicles", "Go back to Main Menu" };

            UserInterface RC = new UserInterface(Prompt, Options);
            return RC.RunUserInterface("all");
        }
        public int ViewSearchedVehiclesChoice(string keyword, FilePathManager file) //CHOICE METHOD 5: view searched vehicles
        {
            Prompt = @"
                    ____ ____ _    ____ ____ ___    ____    _  _ ____ _  _ _ ____ _    ____
                    [__  |___ |    |___ |     |     |__|    |  | |___ |__| | |    |    |___
                    ___] |___ |___ |___ |___  |     |  |     \/  |___ |  | | |___ |___ |___

                    Shown below are the vehicles that matches the search. Select a vehicle to view its details.";
            Options = inventory.ViewSearchedVehicles(keyword, "search", file);

            UserInterface VSV = new UserInterface(Prompt, Options);
            return VSV.RunUserInterface("all");
        }
        public int ViewAllVehiclesChoice(string type, FilePathManager file) //CHOICE METHOD 6: view all vehicles
        {
            Inventory inventory = new Inventory();
            Prompt = @"
                    ____ ____ _    ____ ____ ___    ____    _  _ ____ _  _ _ ____ _    ____
                    [__  |___ |    |___ |     |     |__|    |  | |___ |__| | |    |    |___
                    ___] |___ |___ |___ |___  |     |  |     \/  |___ |  | | |___ |___ |___

                    Shown below are the vehicles available for rent. Please select a vehicle to view its details.";
            Options = inventory.ViewAllVehicles(type, file);

            UserInterface VAV = new UserInterface(Prompt, Options);
            return VAV.RunUserInterface("all");
        }
        public int VehicleRentChoice() //CHOICE METHOD 7: rent the vehicle
        {
            Prompt = "Do you want to rent this vehicle?";
            Options = new string[] { "Rent this vehicle", "Choose another vehicle" };

            UserInterface VR = new UserInterface(Prompt, Options);
            return VR.RunUserInterface("rent");
        }
        public int ViewOwnedVehiclesChoice(string username, FilePathManager file) //CHOICE METHOD 8: view owned vehicles
        {
            Prompt = @$"
                    ____ ____ _    ____ ____ ___    ____    _  _ ____ _  _ _ ____ _    ____
                    [__  |___ |    |___ |     |     |__|    |  | |___ |__| | |    |    |___
                    ___] |___ |___ |___ |___  |     |  |     \/  |___ |  | | |___ |___ |___

                    Only vehicles owned by {username} are displayed.";
            Options = inventory.ViewOwnedVehicles(username, file);

            UserInterface VOV = new UserInterface(Prompt, Options);
            return VOV.RunUserInterface("all");
        }
        public int ViewPendingChoice(string username, FilePathManager file) //CHOICE METHOD 9: view pending vehicles
        {
            Prompt = @$"
                    ____ ____ _    ____ ____ ___    ____    _  _ ____ _  _ _ ____ _    ____
                    [__  |___ |    |___ |     |     |__|    |  | |___ |__| | |    |    |___
                    ___] |___ |___ |___ |___  |     |  |     \/  |___ |  | | |___ |___ |___

                    Shown below are the pending vehicles waiting for your approval. Select to view client details.";
            Options = inventory.ViewPendingRentalOwner(username, file);

            UserInterface VAV = new UserInterface(Prompt, Options);
            return VAV.RunUserInterface("all");
        }
        public int RentalTimeChoice() //CHOICE METHOD 10: rental time
        {
            Prompt = "How long do you plan on renting the vehicle?";
            Options = new string[] { "A couple of days", "A few hours" };

            UserInterface RD = new UserInterface(Prompt, Options);
            return RD.RunUserInterface("rent");
        }
        public int RentalDetailsChoice(string username, string type, FilePathManager file) //CHOICE METHOD 11: rental details
        {
            Prompt = @$"
                    ____ ____ _  _ ___ ____ _       ___  ____ ___ ____ _ _    ____ 
                    |__/ |___ |\ |  |  |__| |       |  \ |___  |  |__| | |    [___
                    |  \ |___ | \|  |  |  | |___    |__/ |___  |  |  | | |___ ___]

                    View, manage, and monitor your rental application details.";

            int choice = 0;
            if (type == "Client")
            {
                Options = new string[] { "View pending rental applications", "Manage the vehicle you are currently renting", "Go back to Main Menu" };
                UserInterface RD = new UserInterface(Prompt, Options);
                choice = RD.RunUserInterface("all");
                if (choice >= 1) //change the choice so that it automatically matches the "skeleton" in the program.cs file
                    choice += 2;

            }
            else if (type == "Vehicle Provider")
            {
                Options = new string[] { "View pending rental applications", "View approved rental applications", "Go back to Main Menu" };
                UserInterface RD = new UserInterface(Prompt, Options);
                choice = RD.RunUserInterface("all");
                if (choice <= 1) //change the choice so that it automatically matches the "skeleton" in the program.cs file
                    choice++;
                else if (choice == 2)
                    choice += 2;
            }
            return choice;
        }
        public int ApprovedChoice(string username, FilePathManager file) //CHOICE METHOD 12: approved vehicles
        {
            Prompt = @"
                    ____ ___  ___  ____ ____ _  _ ____ ___     _  _ ____ _  _ _ ____ _    ____ ____ 
                    |__| |__] |__] |__/ |  | |  | |___ |  \    |  | |___ |__| | |    |    |___ [___ 
                    |  | |    |    |  \ |__|  \/  |___ |__/     \/  |___ |  | | |___ |___ |___ ___] 

                    Shown below are vehicles that you own which are currently being rented.";
            Options = inventory.ViewApprovedRental(username, file);

            UserInterface VAV = new UserInterface(Prompt, Options);
            return VAV.RunUserInterface("all");
        }
        public int ApprovePendingChoice() //CHOICE METHOD 13: approve pending vehicles
        {
            Prompt = "Do you want to approve this request?";
            Options = new string[] { "YES", "NO", "Decide later"};

            UserInterface RD = new UserInterface(Prompt, Options);
            int choice = RD.RunUserInterface("pending");
            return choice;
        }
        public int CurrentlyRentingChoice(string username, FilePathManager file) //CHOICE METHOD 14: display currently rented vehicle
        {
            string vehicle = inventory.ViewCurrentRental(username, file);
            int choice = 2;

            if (vehicle != "")
            {
                Console.Clear();
                Prompt = @$"
                            ____ _  _ ____ ____ ____ _  _ ___ _    _   _    ____ ____ _  _ ___ _ _  _ ____ 
                            |    |  | |__/ |__/ |___ |\ |  |  |     \_/     |__/ |___ |\ |  |  | |\ | | __ 
                            |___ |__| |  \ |  \ |___ | \|  |  |___   |      |  \ |___ | \|  |  | | \| |__] 
                                
                                                            {vehicle}

                            What would you like to do your current rental vehicle?";
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
        public int ManageVehiclesChoice() //CHOICE METHOD 15: manage vehicles
        {

            Prompt = @"
                    _  _ ____ _  _ ____ ____ ____    _  _ ____ _  _ _ ____ _    ____ ____ 
                    |\/| |__| |\ | |__| | __ |___    |  | |___ |__| | |    |    |___ [___
                    |  | |  | | \| |  | |__] |___     \/  |___ |  | | |___ |___ |___ ___]

                    Manage your vehicles as a vehicle provider. Add, update, and remove.";
            Options = new string[] { "Add your own rentable vehicle", "Update vehicles", "Remove vehicles", "Go back to Main Menu" };

            UserInterface MV = new UserInterface(Prompt, Options);
            return MV.RunUserInterface("all");
        }
        public string UpdateVehicleDetailsChoice(string username, FilePathManager file, int choice) //CHOICE METHOD 16: update vehicle details
        {
            Prompt = @"
                    _  _ ___  ___  ____ ___ ____    _  _ ____ _  _ _ ____ _    ____ 
                    |  | |__] |  \ |__|  |  |___    |  | |___ |__| | |    |    |___ 
                    |__| |    |__/ |  |  |  |___     \/  |___ |  | | |___ |___ |___ 
                                                                
                    Select a detail of the vehicle that you want to update.";
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
        public string VehicleTypeChoice() //CHOICE METHOD 17: vehicle type
        {
            Prompt = "Choose a vehicle type";
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
            return "";
        }
        public string VehicleFuelChoice() //CHOICE METHOD 18: vehicle fuel
        {
            Prompt = "Choose fuel type";
            Options = new string[] { "Gasoline", "Diesel", "Electric", "Hybrid", "Hydrogen" };

            UserInterface VF = new UserInterface(Prompt, Options);
            return VF.RunUserInterfaceString("vehicle fuel");
        }
        public int ManageUserChoice() //CHOICE METHOD 19: manage user
        {
            Prompt = @"
                    _  _ ____ _  _ ____ ____ ____    _  _ ____ ____ ____ 
                    |\/| |__| |\ | |__| | __ |___    |  | [__  |___ |__/ 
                    |  | |  | | \| |  | |__] |___    |__| ___] |___ |  \ 

                    Manage your user account. View, update, or delete your account.";
            Options = new string[] { "View account details", "Update account details", "Delete account", "Go back to Main Menu" };

            UserInterface MM = new UserInterface(Prompt, Options);
            return MM.RunUserInterface("all");
        }
        public string UpdateUserDetailsChoice(string username, FilePathManager file) //CHOICE METHOD 20: update user details
        {
            Inventory inventory = new Inventory();
            Prompt = @"
                    _  _ ___  ___  ____ ___ ____    ___  ____ ___ ____ _ _    ____ 
                    |  | |__] |  \ |__|  |  |___    |  \ |___  |  |__| | |    [___
                    |__| |    |__/ |  |  |  |___    |__/ |___  |  |  | | |___ ___] 

                    Select a detail that you want to change

                    (use the UP and DOWN arrow keys to navigate, press ENTER to select)";
            Options = inventory.ViewUserDetails(username, file);
            UserInterface UD = new UserInterface(Prompt, Options);
            string detailchoice = UD.RunUserInterfaceString("all");
            if (detailchoice != "Go back to Manage User Menu")
            {
                string[] detailchoicepart = detailchoice.Split(": ");
                return detailchoicepart[0];
            }
            return "";
        }
        public int DeleteUserChoice(string username, FilePathManager file) //CHOICE METHOD 21: delete user
        {
            Inventory inventory = new Inventory();
            Prompt = @"
                    ___  ____ _    ____ ___ ____    _  _ ____ ____ ____ 
                    |  \ |___ |    |___  |  |___    |  | [__  |___ |__/ 
                    |__/ |___ |___ |___  |  |___    |__| ___] |___ |  \ 
        
                    Are you sure that you want to delete your account?
                    This will also remove all vehicles owned by you. PROCEED WITH CAUTION! 

                    (use the UP and DOWN arrow keys to navigate, press ENTER to select)";
            Options = new string[] { "I AM SURE IN DELETING MY ACCOUNT", "Go back to Manage User Menu" };
            UserInterface DU = new UserInterface(Prompt, Options);
            return DU.RunUserInterface("all");
        }
        public int DeleteAdminVehicleChoice() //CHOICE METHOD 22: delete vehicles
        {
            Prompt = "Do you want to remove this vehicle from the system?";
            Options = new string[] { "DELETE THIS VEHICLE", "Go back to viewing other vehicles" };
            UserInterface DU = new UserInterface(Prompt, Options);
            return DU.RunUserInterface("delete");
        }
        public string ViewAdminUserChoice(FilePathManager file) //CHOICE METHOD 23: choose users
        {
            Inventory inventory = new Inventory();
            Prompt = @"
                    ____ ____ _    ____ ____ ___    ____    _  _ ____ ____ ____ 
                    [__  |___ |    |___ |     |     |__|    |  | [__  |___ |__/ 
                    ___] |___ |___ |___ |___  |     |  |    |__| ___] |___ |  \ 
                              
                    Shown below are the list of all users registered in the program.";
            Options = inventory.ViewUsers(file);
            UserInterface DU = new UserInterface(Prompt, Options);
            return DU.RunUserInterfaceString("all");
        }
        public int DeleteAdminUserChoice() //CHOICE METHOD 24: delete users
        {
            Prompt = "Do you want to remove this user account from the system?";
            Options = new string[] { "DELETE THIS ACCOUNT", "Go back to viewing other users" };
            UserInterface DU = new UserInterface(Prompt, Options);
            return DU.RunUserInterface("delete");
        }
    }
}