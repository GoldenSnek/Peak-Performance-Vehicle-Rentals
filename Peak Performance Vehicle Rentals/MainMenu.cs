using System;
using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Transactions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

//To-Do List
//1. [top priority] Edit vehicle details (ugma or kung kanusa naay time)
//2. View Rental History
//3. Add user details when displaying vehicle details
//4. Add rental process and application (final part og ang pinakalisod nga part before ko mo proceed sa pa chuychuy sa UI)
//5. details should also accept integers such as the price
//6. rental process - price calculation should include discounts, tax, extra fees, para taas ang reciept
//7. ang car nga gi rent ma wagtang sa view all vehicles
//MAJOR PROBLEM, IF I SPAM ANG SPACES MO COUNT GYAPON SIYA

namespace Peak_Performance_Vehicle_Rentals
{
    internal class VehicleManager : VehicleDetailManager, IVehicleManagement
    {
        //Main Menu Case 0:
        public void SearchRentalVehicles(string username, string type, FilePathManager file) //MAIN METHOD 0 for searching a rentable vehicles
        {
            int choice;
            int choiceRent;
            Choice choose = new Choice();
            Inventory inventory = new Inventory();

            Console.ForegroundColor = ConsoleColor.Cyan;
            UserInterface.CenterTextMargin(3, 0);
            Console.WriteLine("Search for owner, brand, model, year, etc.");
            Prompt("Enter the keyword: ");
            string keyword = Console.ReadLine();

            do
            {
                choice = choose.ViewSearchedVehiclesChoice(keyword, file);
                if (choice != inventory.ViewSearchedVehicles(keyword, "search", file).Length-1)
                {
                    VehicleFile vehicle = new VehicleFile();
                    string[] vehicleRentDetails = vehicle.DisplayVehicleFile(choice, keyword, "search", file);

                    //choose what to do with the vehicle
                    if (type == "Client")
                    {
                        choiceRent = choose.VehicleRentChoice(vehicleRentDetails[2], username);
                        do
                        {
                            switch (choiceRent)
                            {
                                case 0:
                                    choiceRent = SearchViewDetails(username, vehicleRentDetails, choice, keyword, "search", file); //shortened code using a method
                                    break;
                            }
                        } while (choiceRent != 1);
                    }
                    else
                    {
                        UserInterface.WaitForKey(3, 0, "Press any key to select another vehicle.");
                    }
                }
            } while (choice != inventory.ViewSearchedVehicles(keyword, "search", file).Length - 1);
        }

        public void ViewRentalVehicles(string username, string type, FilePathManager file) //MAIN METHOD 1 for viewing all rentable vehicles
        {
            int choice;
            int choiceRent;
            int choiceDelete;
            Choice choose = new Choice();
            Inventory inventory = new Inventory();
            do
            {
                choice = choose.ViewAllVehiclesChoice(type, file);
                if (choice != inventory.ViewAllVehicles(type, file).Length - 1)
                {
                    VehicleFile vehicle = new VehicleFile();
                    string[] vehicleRentDetails = vehicle.DisplayVehicleFile(choice, "N A", "all", file);

                    //choose what to do with the vehicle
                    if (type == "Client")
                    {
                        choiceRent = choose.VehicleRentChoice(vehicleRentDetails[2], username);
                        do
                        {
                            switch (choiceRent)
                            {
                                case 0:
                                    choiceRent = SearchViewDetails(username, vehicleRentDetails, choice, "N A", "all", file); //shortened code using a method
                                    break;
                            }
                        } while (choiceRent != 1);
                    }
                    else if (type == "Admin")
                    {
                        choiceDelete = choose.DeleteAdminVehicleChoice();
                        switch (choiceDelete)
                        {
                            case 0:
                                VehicleFile allVehicle = new VehicleFile();
                                allVehicle.DeleteVehicleFile(username, choice, "Admin", file);
                                break;
                        }
                    }
                    else
                    {
                        UserInterface.WaitForKey(3, 0, "Press any key to select another vehicle.");
                    }
                }
            } while (choice != inventory.ViewAllVehicles(type, file).Length - 1);
        }
        private int SearchViewDetails(string username, string[] vehicleRentDetails, int choice, string search, string type, FilePathManager file) //SUPPORTING METHOD for Search/View
        {
            Inventory inventory = new Inventory();
            string pendingVehicle = inventory.ViewPendingRentalClient(username, file);
            string currentVehicle = inventory.ViewCurrentRental(username, file);
            if (pendingVehicle == "" && currentVehicle == "")
            {
                string[] rentDetails = new string[3];
                Choice chooseRent = new Choice();

                //calculations for rent
                int priceCalculation = chooseRent.RentalTimeChoice();

                if (priceCalculation == 0)
                {
                    string[] tempDetails = CalculatePrice(0, double.Parse(vehicleRentDetails[0]));
                    rentDetails[0] = tempDetails[0];
                    rentDetails[1] = tempDetails[1];
                }
                else if (priceCalculation == 1)
                {
                    string[] tempDetails = CalculatePrice(1, double.Parse(vehicleRentDetails[1]));
                    rentDetails[0] = tempDetails[0];
                    rentDetails[1] = tempDetails[1];
                }
                //extra
                rentDetails[2] = AddNote();

                VehicleFile pending = new VehicleFile();
                pending.TransferPendingFile(choice, rentDetails, username, search, type, file);
                return 1;
            }
            else
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You can only rent one vehicle at a time!"); Thread.Sleep(1000);
                UserInterface.WaitForKey(3, 0, "Press any key to select another vehicle.");
                return 1;
            }
        }

        //Main Menu Case 1:
        public void PendingVehiclesClient(string username, FilePathManager file) //MAIN METHOD 0 for viewing pending vehicles client
        {
            Inventory inventory = new Inventory();
            string vehicleClient = inventory.ViewPendingRentalClient(username, file);
            if (vehicleClient != "")
            {
                VehicleFile vehicle = new VehicleFile();
                vehicle.DisplayPendingFile(0, username, "Client", file);
                UserInterface.WaitForKey(3, 0, "Press any key to go back to Rental Details Menu.");
            }
            else
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You currently do not have a pending application."); Thread.Sleep(1500);
                Console.ResetColor();
            }
        }

        public void PendingVehiclesOwner(string username, FilePathManager file) //MAIN METHOD 0 for viewing pending vehicles provider
        {
            Choice choose = new Choice();
            Inventory inventory = new Inventory();
            do
            {
                int choice = choose.ViewPendingChoice(username, file);

                if (choice == inventory.ViewPendingRentalOwner(username, file).Length - 1)
                    return;

                VehicleFile vehicle = new VehicleFile();
                vehicle.DisplayPendingFile(choice, username, "Vehicle Provider", file);

                int approveChoice = choose.ApprovePendingChoice();

                if (approveChoice == 0)
                {
                    vehicle.TransferApprovedFile(choice, username, file);
                    UserInterface.WriteColoredText(3, 0, "green", "The vehicle rental application is approved!");
                }
                else if (approveChoice == 1)
                {
                    vehicle.TransferNonApprovedFile(choice, username, file);
                    UserInterface.WriteColoredText(3, 0, "red", "The vehicle rental application is not approved!");
                }
            } while (true);

        }

        public void ApprovedVehicles(string username, FilePathManager file) //MAIN METHOD 1 for viewing approved vehicles
        {
            Choice choose = new Choice();
            Inventory inventory = new Inventory();
            int choice;
            do
            {
                choice = choose.ApprovedChoice(username, file);

                VehicleFile vehicle = new VehicleFile();
                vehicle.DisplayVehicleFile(choice, "N A", "approved", file);

            } while (choice != inventory.ViewApprovedRental(username, file).Length - 1);

        }

        public void CurrentlyRentingVehicle(string username, FilePathManager file) //MAIN METHOD 2 for viewing currently rented vehicle
        {
            Choice choose = new Choice();
            int choice;
            do
            {
                choice = choose.CurrentlyRentingChoice(username, file);
                if (choice == 0)
                {
                    VehicleFile vehicle = new VehicleFile();
                    vehicle.DisplayReceiptFile(username, file);
                    UserInterface.WaitForKey(3, 0, "Press any key if you are done looking at the receipt");
                }
                else if (choice == 1)
                {
                    VehicleFile vehicle = new VehicleFile();
                    vehicle.TransferFinishRentFile(username, file);

                    UserInterface.CenterTextMargin(3, 0);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Thank you trusting \"Peak Performance Vehicle Rentals\" and the vehicle owner"); Thread.Sleep(1000);
                    UserInterface.CenterTextMargin(3, 0);
                    Console.WriteLine("Rent from us again next time! \"Rent, Ride, Repeat\"\n"); Thread.Sleep(1000);
                    UserInterface.WaitForKey(3, 0, "Press any key to return to the Rental Details Menu");
                    choice = 2;
                }
            } while (choice != 2);
        }

        //Main Menu Case 2:
        public void AddVehicle(string username) //MAIN METHOD 0 for adding vehicles
        {

            Console.Clear();
            UserInterface.CenterVerbatimText(@"
                                            ____ ___  ___     _  _ ____ _  _ _ ____ _    ____ 
                                            |__| |  \ |  \    |  | |___ |__| | |    |    |___ 
                                            |  | |__/ |__/     \/  |___ |  | | |___ |___ |___ 
                                                  
                                            Enter the necessary details to add a vehicle to the rentable list
                                            ");

            string[] details = new string[12];
            Choice choose = new Choice();

            //type
            details[0] = choose.VehicleTypeChoice();
            if (details[0] == "")
                return;
            //brand
            details[1] = VehicleName();
            //model
            details[2] = VehicleModel();
            //year
            details[3] = VehicleYear();
            //license plate
            details[4] = VehicleLicensePlate();
            //color
            details[5] = VehicleColor();
            //fuel
            Console.WriteLine();
            details[6] = choose.VehicleFuelChoice();
            //seating capacity
            details[7] = VehicleSeatingCapacity();
            //mileage
            details[8] = VehicleMileage();
            //location
            details[9] = VehicleLocation();
            //rental price
            details[10] = VehiclePriceDay();
            //rental price
            details[11] = VehiclePriceHour();

            //create a new vehicle file
            VehicleFile vehicle = new VehicleFile();
            vehicle.CreateVehicleFile(username, details);
        }
        public void UpdateVehicle(string username, FilePathManager file) //MAIN METHOD 1 for updating vehicles
        {
            //Update vehicle file
            int choice;
            string detailchoice = "";
            string newdetail = "";
            Choice choose = new Choice();
            Inventory inventory = new Inventory();
            do {
                choice = choose.ViewOwnedVehiclesChoice(username, file);
                do
                {
                    if (choice != inventory.ViewOwnedVehicles(username, file).Length - 1)
                    {
                        detailchoice = choose.UpdateVehicleDetailsChoice(username, file, choice);

                        if (detailchoice == "Manufacture Year")
                            newdetail = VehicleYear();
                        if (detailchoice == "License Plate")
                            newdetail = VehicleLicensePlate();
                        if (detailchoice == "Color")
                            newdetail = VehicleColor();
                        if (detailchoice == "Fuel Type")
                            newdetail = choose.VehicleFuelChoice();
                        if (detailchoice == "Seating Capacity")
                            newdetail = VehicleSeatingCapacity();
                        if (detailchoice == "Mileage")
                            newdetail = VehicleMileage();
                        if (detailchoice == "Pickup and Return Location")
                            newdetail = VehicleLocation();
                        if (detailchoice == "Daily Rental Price")
                            newdetail = VehiclePriceDay();
                        if (detailchoice == "Hourly Rental Price")
                            newdetail = VehiclePriceHour();
                        if (detailchoice != "")
                        {
                            VehicleFile vehicle = new VehicleFile();
                            vehicle.UpdateVehicleFile(username, choice, detailchoice, newdetail);
                        }
                    }
                } while (detailchoice != "");
            } while (choice != inventory.ViewOwnedVehicles(username, file).Length - 1);
        }
        
        public void DeleteVehicle(string username, FilePathManager file) //MAIN METHOD 2 for deleting vehicles
        {
            Choice choose = new Choice();
            Inventory inventory = new Inventory();
            int choice;
            do
            {
                choice = choose.ViewOwnedVehiclesChoice(username, file);
                VehicleFile vehicle = new VehicleFile();
                vehicle.DeleteVehicleFile(username, choice, "", file); //Delete vehicle file
            } while (choice != inventory.ViewOwnedVehicles(username, file).Length - 1);
        }
    }

    internal class VehicleDetailManager //SUPPORTING CLASS for manage vehicles
    {
        private protected static string VehicleName() //SUPPORTING METHOD 1 for manage vehicles
        {
            string name;
            do
            {
                Prompt("Enter vehicle brand: ");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    InvalidVehicleDetail(3, 0, "red", "Please do not leave the vehicle brand empty");
                else if (name.Length >= 20)
                    InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");

            } while (string.IsNullOrWhiteSpace(name) | name.Length >= 20);
            return name;
        }
        private protected static string VehicleModel() //SUPPORTING METHOD 2 for manage vehicles
        {
            string model;
            do
            {
                Prompt("Enter vehicle model: ");
                model = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(model))
                    InvalidVehicleDetail(3, 0, "red", "Please do not leave the vehicle brand empty");
                else if (model.Length >= 20)
                    InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
            } while (string.IsNullOrWhiteSpace(model) || model.Length >= 20);
            return model;
        }
        private protected static string VehicleYear() //SUPPORTING METHOD 3 for manage vehicles
        {
            string year = "";
            int tempyear;
            bool success = false;
            do
            {
                Prompt("Enter the manufacture year of the vehicle: ");
                year = Console.ReadLine();
                success = int.TryParse(year, out tempyear);
                if (!success || tempyear < 0 || tempyear > 3000)
                    InvalidVehicleDetail(3, 0, "red", "Please enter a proper year!");
            } while (!success || tempyear < 0 || tempyear > 3000);
            return year;
        }
        private protected static string VehicleLicensePlate() //SUPPORTING METHOD 4 for manage vehicles
        {
            string licenseplate;
            do
            {
                Prompt("Enter license plate #: ");
                licenseplate = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(licenseplate))
                    InvalidVehicleDetail(3, 0, "red", "Please do not leave the license plate # empty");
                else if (licenseplate.Length >= 20)
                    InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
            } while (string.IsNullOrWhiteSpace(licenseplate) || licenseplate.Length >= 20);
            return licenseplate;
        }
        private protected static string VehicleColor() //SUPPORTING METHOD 5 for manage vehicles
        {
            string color;
            do
            {
                Prompt("Enter color of the vehicle: ");
                color = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(color))
                    InvalidVehicleDetail(3, 0, "red", "Please do not leave the color empty!");
                else if (color.Any(char.IsDigit))
                    InvalidVehicleDetail(3, 0, "red", "Please enter a proper color!");
                else if (color.Length >= 20)
                    InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
            } while (string.IsNullOrWhiteSpace(color) || color.Any(char.IsDigit) || color.Length >= 20);
            return color;
        }
        private protected static string VehicleSeatingCapacity() //SUPPORTING METHOD 6 for manage vehicles
        {
            string seats;
            int tempseats;
            bool success = false;
            do
            {
                Prompt("Enter the number of seats of the vehicle: ");
                seats = Console.ReadLine();
                success = int.TryParse(seats, out tempseats);
                if (!success || tempseats < 1 || tempseats > 50)
                    InvalidVehicleDetail(3, 0, "red", "Please enter a proper amount of seats!");
                else if (seats.Length >= 20)
                    InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
            } while (!success || tempseats < 1 || tempseats > 50 | seats.Length >= 20);
            return seats;
        }
        private protected static string VehicleMileage() //SUPPORTING METHOD 7 for manage vehicles
        {
            double tempmileage;
            string mileage;
            bool success = false;
            do
            {
                Prompt("Enter mileage of the vehicle in kilometers: ");
                mileage = Console.ReadLine();
                success = double.TryParse(mileage, out tempmileage);
                if (!success || tempmileage < 0 || tempmileage > 999999999)
                    InvalidVehicleDetail(3, 0, "red", "Please enter a realistic mileage!");
                else if (mileage.Length >= 20)
                    InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
            } while (!success || tempmileage < 0 || tempmileage > 999999999 || mileage.Length >= 20);
            mileage = $"{Math.Round(tempmileage, 2)}";
            return mileage + " km";
        }
        private protected static string VehicleLocation() //SUPPORTING METHOD 8 for manage vehicles
        {
            string location;
            do
            {
                Prompt("Enter vehicle pickup and return location: ");
                location = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(location))
                    InvalidVehicleDetail(3, 0, "red", "Please do not leave the vehicle pickup and return location empty!");
                else if (location.Length >= 20)
                    InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
            } while (string.IsNullOrWhiteSpace(location) || location.Length >= 20);
            return location;
        }
        private protected static string VehiclePriceDay() //SUPPORTING METHOD 9 for manage vehicles
        {
            string price;
            double tempPrice;
            bool success = false;
            do
            {
                Prompt("Enter the DAILY rental rate of the vehicle in PHP/day: ");

                price = Console.ReadLine();

                success = double.TryParse(price, out tempPrice);
                if (!success || tempPrice < 100 || tempPrice > 100000)
                    InvalidVehicleDetail(3, 0, "red", "Minimum should be 100 Php/day and max 100,000 Php/day");
                else if (price.Length >= 20)
                    InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
            } while (!success || tempPrice < 100 || tempPrice > 100000 || price.Length >= 20);
            price = $"{Math.Round(tempPrice, 2)}";
            return price + " PHP/day";
        }
        private protected static string VehiclePriceHour() //SUPPORTING METHOD 10 for manage vehicles
        {
            string price;
            double tempPrice;
            bool success = false;
            do
            {
                Prompt("Enter the HOURLY rental rate of the vehicle in PHP/hr: ");
                price = Console.ReadLine();
                success = double.TryParse(price, out tempPrice);
                if (!success || tempPrice < 10 || tempPrice > 10000)
                    InvalidVehicleDetail(3, 0, "red", "Minimum should be 10 Php/hr and max 10,000 Php/hr");
                else if (price.Length >= 20)
                    InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
            } while (!success || tempPrice < 10 || tempPrice > 10000 || price.Length >= 20);
            price = $"{Math.Round(tempPrice, 2)}";
            return price + " PHP/hr";
        }
        private protected static string[] CalculatePrice(int type, double price) //SUPPORTING METHOD 11 for manage vehicles
        {
            int time = 0;
            string tempTime;
            string[] priceDetails = new string[2];
            bool success = false;

            if (type == 0)
            {
                do
                {
                    Prompt("How many days would you rent the vehicle: ");
                    tempTime = Console.ReadLine();
                    success = int.TryParse(tempTime, out time);
                    if (!success || time < 1 || time > 30)
                        InvalidVehicleDetail(3, 0, "red", "You can only rent the vehicle for a minimum of 1 day and a maximum of 30 days");
                    else if (tempTime.Length >= 20)
                        InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
                } while (!success || time < 1 || time > 30 || tempTime.Length >= 20);
                priceDetails[0] = time + " day(s)";
            }
            else if (type == 1)
            {
                do
                {
                    Prompt("How many hours would you rent the vehicle: ");
                    tempTime = Console.ReadLine();
                    success = int.TryParse(tempTime, out time);
                    if (!success || time < 1 || time > 24)
                        InvalidVehicleDetail(3, 0, "red", "You can only rent the vehicle for a minimum of 1 hour and a maximum of 24 hours");
                    else if (tempTime.Length >= 20)
                        InvalidVehicleDetail(3, 0, "red", "Please keep it below 20 characters");
                } while (!success || time < 1 || time > 24 || tempTime.Length >= 20);
                price *= time;
                priceDetails[0] = time + " hour(s)";
            }
            priceDetails[1] = $"{Math.Round(price * (double)time, 2)} PHP";

            return priceDetails;
        }
        private protected static string AddNote() //SUPPORTING METHOD 12 for manage vehicles
        {
            string note = "";
            Prompt("Additional notes: ");
            note = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(note))
                return "N/A";
            else
                return note;
        }
        private protected static void Prompt(string text) //SUPPORTING METHOD for vehicle detail manager, prints out a prompt
        {
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
            UserInterface.CenterTextMargin(3, 0);
            Console.Write(text);
            Console.ResetColor();
        }
        private static void InvalidVehicleDetail(int x, int y, string color, string text) //SUPPORTING METHOD for vehicle detail, prints out a message
        {
            UserInterface.WriteColoredText(x, y, color, text);
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
        }
    }

    internal class UserManager : UserDetailManager, IUserManagement
    {
        //Main Menu Case 3:
        public void ViewUserDetails(string username, FilePathManager file) //MAIN METHOD 0 for managing user account
        {
            UserFile user = new UserFile();
            user.DisplayUserFile("user", username);
            UserInterface.WaitForKey(3, 0, "Press any key if you are done reading the details");
        }
        public void UpdateUser(string username, FilePathManager file) //MAIN METHOD 1 for updating user account
        {
            string detailchoice = "";
            string newdetail = "";
            Choice choose = new Choice();
            do
            {
                detailchoice = choose.UpdateUserDetailsChoice(username, file);

                if (detailchoice == "Email Address")
                    newdetail = UserEmail();
                if (detailchoice == "Contact Number")
                    newdetail = UserContact();
                if (detailchoice == "Date of Birth (MM/DD/YYYY)")
                    newdetail = UserBirth();
                if (detailchoice == "Home Address")
                    newdetail = UserAddress();
                if (detailchoice != "")
                {
                    UserFile user = new UserFile();
                    user.UpdateUserFile(username, detailchoice, newdetail); //update user file
                }
            } while (detailchoice != "");
        }
        public bool DeleteUser(string username, string type, FilePathManager file) //MAIN METHOD 2 for deleting user account
        {
            string choice;
            int option;
            bool result = true;
            Choice choose = new Choice();

            if (type == "Admin")
            {
                do
                {
                    choice = choose.ViewAdminUserChoice(file);
                    if (choice != "Go back to Main Menu")
                    {
                        string[] details = choice.Split(" | "); //index 0 is username

                        UserFile user = new UserFile();
                        user.DisplayUserFile("user", details[0]);

                        option = choose.DeleteAdminUserChoice();
                        if (option == 0)
                        {
                            int deleteChoice = user.DeleteUserFile(details[0], file); //delete user file
                            if (deleteChoice == 0)
                            {
                                result = true;

                            }
                            else
                            {
                                result = false;
                            }
                        }
                    }
                } while (choice != "Go back to Main Menu");
            }
            else
            {
                do
                {
                    option = choose.DeleteUserChoice(username, file);

                    if (option == 0)
                    {
                        UserFile user = new UserFile();
                        int deleteChoice = user.DeleteUserFile(username, file); //delete user file
                        if (deleteChoice == 0)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                } while (option != 1);
            }
            return result;
        }
    }

    internal class UserDetailManager //SUPPORTING CLASS
    {
        private protected static string UserEmail() //SUPPORTING METHOD 1 for manage user account
        {
            string email;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Prompt("Enter your email address: ");
                email = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(email))
                    InvalidUserDetail(3, 0, "red", "Please do not leave the email address empty!");
            } while (string.IsNullOrWhiteSpace(email));
            return email;
        }
        private protected static string UserContact() //SUPPORTING METHOD 2 for manage user account
        {
            string contact;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Prompt("Enter your contact number: ");
                contact = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(contact))
                    InvalidUserDetail(3, 0, "red", "Please do not leave the contact number empty!");
            } while (string.IsNullOrWhiteSpace(contact));
            return contact;
        }

        private protected static string UserBirth() //SUPPORTING METHOD 3 for manage user account
        {
            string[] details = new string[3];
            int tempDetails;
            bool success = false;
            bool dateSuccess = false;

            do
            {
                do
                {
                    Prompt("Enter Month (MM): ");
                    details[0] = Console.ReadLine();
                    success = int.TryParse(details[0], out tempDetails);
                    if (!success || tempDetails < 1 || tempDetails > 12)
                        InvalidUserDetail(3, 0, "red", "Please enter a proper month!");
                } while (!success || tempDetails < 1 || tempDetails > 12);
                do
                {
                    Prompt("Enter Day (DD): ");
                    details[1] = Console.ReadLine();
                    success = int.TryParse(details[1], out tempDetails);
                    if (!success || tempDetails < 1 || tempDetails > 31)
                        InvalidUserDetail(3, 0, "red", "Please enter a proper day!");
                } while (!success || tempDetails < 1 || tempDetails > 31);
                do
                {
                    Prompt("Enter Year (YYYY): ");
                    details[2] = Console.ReadLine();
                    success = int.TryParse(details[2], out tempDetails);
                    if (!success || tempDetails < 1 || tempDetails > 3000)
                        InvalidUserDetail(3, 0, "red", "Please enter a proper year!");
                } while (!success || tempDetails < 1 || tempDetails > 3000);

                dateSuccess = DateChecker(details);
                if (!dateSuccess)
                {
                    InvalidUserDetail(3, 0, "red year", "Date does not exist. Please enter a proper month, day, and year.");
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                }

            } while (!dateSuccess);

            return $"{details[0]}/{details[1]}/{details[2]}";
        }
        private protected static string UserAddress() //SUPPORTING METHOD 4 for manage user account
        {
            string address;
            do
            {
                Prompt("Enter your home address: ");
                address = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(address))
                    InvalidUserDetail(3, 0, "red", "Please do not leave the home address empty!");
            } while (string.IsNullOrWhiteSpace(address));
            return address;
        }
        private static bool DateChecker(string[] d) //SUPPORTING METHOD for UserBirth, checks if date is valid
        {
            if (d[0] == "1" || d[0] == "3" || d[0] == "5" || d[0] == "7" || d[0] == "8" || d[0] == "10" || d[0] == "12")
            {
                int tempDay = int.Parse(d[1]);
                if (tempDay > 31)
                    return false;
            }
            if (d[0] == "4" || d[0] == "6" || d[0] == "9" || d[0] == "11")
            {
                int tempDay = int.Parse(d[1]);
                if (tempDay > 30)
                    return false;
            }
            if (d[0] == "2") //february leap year
            {
                int tempDay = int.Parse(d[1]);
                int tempYear = int.Parse(d[2]);
                if (tempYear % 4 == 0) //is leap year
                {
                    if (tempDay > 29)
                        return false;
                }
                else
                {
                    if (tempDay > 28)
                        return false;
                }
            }

            return true;
        }
        private static void Prompt(string text) //SUPPORTING METHOD, prints out a prompt
        {
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
            UserInterface.CenterTextMargin(3, 0);
            Console.Write(text);
            Console.ResetColor();
        }
        private static void InvalidUserDetail(int x, int y, string color, string text) //SUPPORTING METHOD, prints out a message
        {
            if (color == "red year")
            {
                color = "red";
                UserInterface.WriteColoredText(x, y, color, text);
                for (int i = 0; i < 2; i++)
                {
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
            }
            else
            {
                UserInterface.WriteColoredText(x, y, color, text);
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.BufferWidth));
            }
        }
    }
}