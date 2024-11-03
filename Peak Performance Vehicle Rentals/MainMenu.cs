using System;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.Marshalling;

//To-Do List
//1. [top priority] Edit vehicle details (ugma or kung kanusa naay time)
//2. View Rental History
//3. Add user details when displaying vehicle details
//4. Add rental process and application (final part og ang pinakalisod nga part before ko mo proceed sa pa chuychuy sa UI)
//5. details should also accept integers such as the price
//6. rental process - price calculation should include discounts, tax, extra fees, para taas ang reciept
//7. ang car nga gi rent ma wagtang sa view all vehicles

namespace Peak_Performance_Vehicle_Rentals
{
    internal class VehicleManager : VehicleDetailManager, IVehicleManagement
    {
        public void ViewRentalVehicles (string username, FilePathManager file) //MAIN METHOD for view rentable vehicles
        {
            int choice;
            int choiceRent;
            Choice choose = new Choice();
            Inventory inventory = new Inventory();
            do
            {
                choice = choose.ViewAllVehiclesChoice(file);
                if (choice != inventory.ViewVehicles(file).Length - 1)
                {
                    VehicleFile vehicle = new VehicleFile();
                    string[] vehicleRentDetails = vehicle.DisplayVehicleFile(choice);

                    //choose what to do with the vehicle
                    choiceRent = choose.VehicleRentChoice(vehicleRentDetails[2], username);
                    do
                    {
                        switch (choiceRent)
                        {
                            case 0:
                                string[] rentDetails = new string[3];
                                Choice chooseRent = new Choice();

                                //calculations for rent
                                int priceCalculation = chooseRent.RentalTimeChoice();

                                if (priceCalculation == 0)
                                {
                                    string[] tempDetails = CalculatePrice(0, int.Parse(vehicleRentDetails[0]));
                                    rentDetails[0] = tempDetails[0];
                                    rentDetails[1] = tempDetails[1];
                                }
                                else if (priceCalculation == 1)
                                {
                                    string[] tempDetails = CalculatePrice(1, int.Parse(vehicleRentDetails[1]));
                                    rentDetails[0] = tempDetails[0];
                                    rentDetails[1] = tempDetails[1];
                                }
                                //extra
                                rentDetails[2] = AddNote();

                                VehicleFile pending = new VehicleFile();
                                pending.TransferPendingFile(choice, rentDetails, username);
                                choiceRent = 1;
                                break;
                        }
                    } while (choiceRent != 1);
                }
            } while (choice != inventory.ViewVehicles(file).Length - 1);
        }

        public void AddVehicle(string username) //MAIN METHOD 1 for manage vehicles
        {
            string[] details = new string[13];
            Choice choose = new Choice();

            //type
            details[0] = choose.VehicleTypeChoice();
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
            //status
            details[12] = choose.VehicleStatusChoice();

            //create a new vehicle file
            VehicleFile vehicle = new VehicleFile();
            vehicle.CreateVehicleFile(username, details);

            Console.WriteLine("New vehicle has been added!"); Thread.Sleep(1000);

        }

        public void UpdateVehicle(string username, FilePathManager file) //MAIN METHOD 2 for manage vehicles
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
                    if (choice != inventory.ViewVehicles(username, file).Length - 1)
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
                        if (detailchoice == "Status")
                            newdetail = choose.VehicleStatusChoice();
                        if (detailchoice != "")
                        {
                            VehicleFile vehicle = new VehicleFile();
                            vehicle.UpdateVehicleFile(username, choice, detailchoice, newdetail);
                        }
                    }
                } while (detailchoice != "");
            } while (choice != inventory.ViewVehicles(username, file).Length - 1);
        }
        
        public void DeleteVehicle(string username, FilePathManager file) //MAIN METHOD 3 for manage vehicles
        {
            Choice choose = new Choice();
            int choice = choose.ViewOwnedVehiclesChoice(username, file);
            VehicleFile vehicle = new VehicleFile();
            vehicle.DeleteVehicleFile(username, file, choice); //Delete vehicle file
        }

        public void PendingVehicles(string username, FilePathManager file) //MAIN METHOD ???
        {
            Choice choose = new Choice();
            int choice = choose.ViewPendingChoice(username, file);

            Inventory inventory = new Inventory();
            if (choice == inventory.ViewPendingRental(username, file).Length - 1)
                return;

            VehicleFile vehicle = new VehicleFile();
            vehicle.DisplayPendingFile(choice, username, file);

            int approveChoice = choose.ApprovePendingChoice();

            if (approveChoice == 0)
            {
                vehicle.TransferApprovedFile(choice, username, file);
                Console.WriteLine("The vehicle rental application is approved!"); Thread.Sleep(1000);
            }
            else if (approveChoice == 1)
            {
                vehicle.TransferNonApprovedFile(choice, username, file);
                Console.WriteLine("The vehicle rental application is not approved!"); Thread.Sleep(1000);
            }

        }

        //finish later
        public void ApprovedVehicles(string username, FilePathManager file) //MAIN METHOD ???
        {

            Console.Clear();
            UserInterface.CenterVerbatimText(@"
____ ___  ___  ____ ____ _  _ ____ ___     _  _ ____ _  _ _ ____ _    ____ ____ 
|__| |__] |__] |__/ |  | |  | |___ |  \    |  | |___ |__| | |    |    |___ [__  
|  | |    |    |  \ |__|  \/  |___ |__/     \/  |___ |  | | |___ |___ |___ ___] 

Shown below are vehicles that you own which are currently being rented
");

            Inventory inventory = new Inventory();
            string[] vehicles = inventory.ViewApprovedRental(username, file);

            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < vehicles.Length; i++)
            {
                UserInterface.CenterTextMargin(0, 0);
                Console.WriteLine(">> " +vehicles[i] + "\n");
            }
            Console.ResetColor();

            UserInterface UI = new UserInterface("Press any key if you are done looking at the vehicles");
            UI.WaitForKey();

        }

        public void CurrentlyRentingVehicles(string username, FilePathManager file)
        {
            Choice choose = new Choice();
            int choice;
            do
            {
                choice = choose.CurrentlyRentingChoice(username, file);
                if (choice == 0)
                {
                    VehicleFile vehicle = new VehicleFile();
                    vehicle.DisplayRecieptFile(username, file);
                }
                else if (choice == 1)
                {
                    VehicleFile vehicle = new VehicleFile();
                    vehicle.TransferFinishRentFile(username, file);
                    choice = 2;
                }
            } while (choice != 2);
        }
    }

    internal class VehicleDetailManager : IVehicleDetailManagement
    {
        public string VehicleName() //SUPPORTING METHOD 1 for manage vehicles
        {
            string name;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter vehicle brand: ");
                name = Console.ReadLine();
                if (name == "")
                    Console.WriteLine("Please do not leave the vehicle brand empty");
            } while (name == "");
            return name;
        }
        public string VehicleModel() //SUPPORTING METHOD 2 for manage vehicles
        {
            string model;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter vehicle model: ");
                model = Console.ReadLine();
                if (model == "")
                    Console.WriteLine("Please do not leave the vehicle model empty");
            } while (model == "");
            return model;
        }
        public string VehicleYear() //SUPPORTING METHOD 3 for manage vehicles
        {
            string year = "";
            int tempyear;
            bool success = false;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter the manufacture year of the vehicle: ");
                year = Console.ReadLine();

                success = int.TryParse(year, out tempyear);

                if (!success || tempyear < 0 || tempyear > 3000)
                    Console.WriteLine("Please enter a proper year!");
            } while (!success || tempyear < 0 || tempyear > 3000);
            return year;
        }
        public string VehicleLicensePlate() //SUPPORTING METHOD 4 for manage vehicles
        {
            string licenseplate;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter license plate #: ");
                licenseplate = Console.ReadLine();
                if (licenseplate == "")
                    Console.WriteLine("Please do not leave the license plate # empty");
            } while (licenseplate == "");
            return licenseplate;
        }
        public string VehicleColor() //SUPPORTING METHOD 5 for manage vehicles
        {
            string color;
            int tempcolor;
            bool success = false;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter color of the vehicle: ");
                color = Console.ReadLine();

                success = int.TryParse(color, out tempcolor);

                if (success || color == "")
                    Console.WriteLine("Please enter a proper color!");
            } while (success || color == "");
            return color;
        }
        public string VehicleSeatingCapacity() //SUPPORTING METHOD 6 for manage vehicles
        {
            string seats;
            int tempseats;
            bool success = false;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter the number of seats of the vehicle: ");
                seats = Console.ReadLine();

                success = int.TryParse(seats, out tempseats);

                if (!success || tempseats < 1 || tempseats > 50)
                    Console.WriteLine("Please enter a proper amount of seats!");
            } while (!success || tempseats < 1 || tempseats > 50);
            return seats;
        }
        public string VehicleMileage() //SUPPORTING METHOD 7 for manage vehicles
        {
            int tempmileage;
            string mileage;
            bool success = false;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter mileage of the vehicle in kilometers: ");
                mileage = Console.ReadLine();

                success = int.TryParse(mileage, out tempmileage);

                if (!success || tempmileage < 0)
                    Console.WriteLine("Please enter a realistic mileage!");
            } while (!success || tempmileage < 0);
            return mileage + " km";
        }
        public string VehicleLocation() //SUPPORTING METHOD 8 for manage vehicles
        {
            string location;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter vehicle pickup and return location: ");
                location = Console.ReadLine();
                if (location == "")
                    Console.WriteLine("Please do not leave the vehicle pickup and return location empty");
            } while (location == "");
            return location;
        }
        public string VehiclePriceDay() //SUPPORTING METHOD 9 for manage vehicles
        {
            string price;
            int tempPrice;
            bool success = false;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter the DAILY rental rate of the vehicle in PHP/day: ");
                price = Console.ReadLine();

                success = int.TryParse(price, out tempPrice);

                if (!success || tempPrice < 100 || tempPrice > 100000)
                    Console.WriteLine("Minimum should be 100 Php/day and max 100,000 Php/day!");
            } while (!success || tempPrice < 100 || tempPrice > 100000);
            return price + " PHP/day";
        }
        public string VehiclePriceHour() //SUPPORTING METHOD 10 for manage vehicles
        {
            string price;
            int tempPrice;
            bool success = false;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter the HOURLY rental rate of the vehicle in PHP/hr: ");
                price = Console.ReadLine();

                success = int.TryParse(price, out tempPrice);

                if (!success || tempPrice < 10 || tempPrice > 10000)
                    Console.WriteLine("Minimum should be 10 Php/hr and max 1,0000 Php/hr");
            } while (!success || tempPrice < 10 || tempPrice > 10000);
            return price + " PHP/hr";
        }
        public string[] CalculatePrice(int type, int price)
        {
            int time;
            string tempTime;
            string[] priceDetails = new string[2];
            bool success = false;

            if (type == 0)
            {
                do
                {
                    UserInterface.CenterTextMargin(3, 0);
                    Console.Write("How many days would you rent the vehicle: ");
                    tempTime = Console.ReadLine();

                    success = int.TryParse(tempTime, out time);

                    if (!success || time < 1 || time > 30)
                        Console.WriteLine("You can only rent the vehicle for a minimum of 1 day and a maximum of 30 days");
                } while (!success || time < 1 || time > 30);

                priceDetails[0] = time + " day(s)";
                priceDetails[1] = price * time + " PHP";

            }
            else if (type == 1)
            {
                do
                {
                    UserInterface.CenterTextMargin(3, 0);
                    Console.Write("How many hours would you rent the vehicle: ");
                    tempTime = Console.ReadLine();

                    success = int.TryParse(tempTime, out time);

                    if (!success || time < 1 || time > 24)
                        Console.WriteLine("You can only rent the vehicle for a minimum of 1 hour and a maximum of 24 hours");
                } while (!success || time < 1 || time > 24);
                price *= time;

                priceDetails[0] = time + " hour(s)";
                priceDetails[1] = price * time + " PHP";
            }

            return priceDetails;
        }
        public string AddNote()
        {
            string note = "";
            UserInterface.CenterTextMargin(3, 0);
            Console.Write("Additional notes: ");
            note = Console.ReadLine();

            if (note == "")
                return "N/A";
            else
                return note;
        }
    }

    internal class UserManager : UserDetailManager, IUserManagement
    {
        public void ViewUserDetails(string username, FilePathManager file) //MAIN METHOD 1 for manage user account
        {
            UserFile user = new UserFile();
            user.DisplayUserFile("user", username);
            UserInterface UI = new UserInterface("Press any key if you are done reading the details");
            UI.WaitForKey();
        }
        public void UpdateUser(string username, FilePathManager file) //MAIN METHOD 2 for manage user account
        {
            //Update vehicle file
            string detailchoice = "";
            string newdetail = "";
            Choice choose = new Choice();
            do
            {
                detailchoice = choose.UpdateUserDetailsChoice(username, file);

                if (detailchoice == "Email Address")
                    newdetail = UserEmail();
                if (detailchoice == "Date of Birth (MM/DD/YY)")
                    newdetail = UserBirth();
                if (detailchoice == "Address")
                    newdetail = UserAddress();
                if (detailchoice != "")
                {
                    UserFile user = new UserFile();
                    user.UpdateUserFile(username, detailchoice, newdetail); //update user file
                }
            } while (detailchoice != "");
        }

        public bool DeleteUser(string username, FilePathManager file) //MAIN METHOD 3 for manage user account
        {
            int choice;
            Choice choose = new Choice();
            choice = choose.DeleteUserChoice(username, file);

            if (choice == 0)
            {
                UserFile user = new UserFile();
                user.DeleteUserFile(username); //delete user file
                return false;
            }
            else
                return true;
        }
    }

    internal class UserDetailManager : IUserDetailManagement
    {
        public string UserEmail() //SUPPORTING METHOD 1 for manage user account
        {
            string email;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter your email address: ");
                email = Console.ReadLine();
                if (email == "")
                    Console.WriteLine("Please do not leave the email address empty!");
            } while (email == "");
            return email;
        }

        public string UserBirth() //SUPPORTING METHOD 2 for manage user account
        {
            string[] details = new string[3];
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Month: ");
                details[1] = Console.ReadLine();
                if (details[1] == "")
                    Console.WriteLine("Please enter a proper month");
            } while (details[1] == "");
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Day: ");
                details[2] = Console.ReadLine();
                if (details[2] == "")
                    Console.WriteLine("Please enter a proper day");
            } while (details[2] == "");
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Year: ");
                details[0] = Console.ReadLine();
                if (details[0] == "")
                    Console.WriteLine("Please enter a proper year");
            } while (details[0] == "");
            return $"{details[1]}/{details[2]}/{details[0]}";
        }

        public string UserAddress() //SUPPORTING METHOD 3 for manage user account
        {
            string address;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter your home address: ");
                address = Console.ReadLine();
                if (address == "")
                    Console.WriteLine("Please do not leave the address empty!");
            } while (address == "");
            return address;
        }
    }
}