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

namespace Peak_Performance_Vehicle_Rentals
{
    internal class VehicleManager : VehicleDetailManager, IVehicleManagement
    {
        public void ViewRentalVehicles(FilePathManager file) //MAIN METHOD for view rentable vehicles
        {
            int choice;
            Choice choose = new Choice();
            Inventory inventory = new Inventory();
            do
            {
                choice = choose.ViewAllVehiclesChoice(file);
                if (choice != inventory.ViewVehicles(file).Length - 1)
                {
                    VehicleFile vehicle = new VehicleFile();
                    vehicle.DisplayVehicleFile(choice);
                    UserInterface UI = new UserInterface("Press any key if you are done reading the details");
                    UI.WaitForKey();
                }
            } while (choice != inventory.ViewVehicles(file).Length - 1);
        }

        public void AddVehicle(string username) //MAIN METHOD 1 for manage vehicles
        {
            string[] details = new string[12];
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
            details[10] = VehiclePrice();
            //status
            details[11] = choose.VehicleStatusChoice();

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

                        if (detailchoice == "Year")
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
                        if (detailchoice == "Pickup and Drop-off Location")
                            newdetail = VehicleLocation();
                        if (detailchoice == "Rental Price")
                            newdetail = VehiclePrice();
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
    }

    internal class VehicleDetailManager : IVehicleDetailManagement
    {
        public string VehicleName() //SUPPORTING METHOD 1 for manage vehicles
        {
            string name;
            do
            {
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
                Console.Write("Enter year of the vehicle: ");
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
                Console.Write("Enter vehicle pickup/drop-off location: ");
                location = Console.ReadLine();
                if (location == "")
                    Console.WriteLine("Please do not leave the vehicle location empty");
            } while (location == "");
            return location;
        }
        public string VehiclePrice() //SUPPORTING METHOD 9 for manage vehicles
        {
            string price;
            int tempprice;
            bool success = false;
            do
            {
                Console.Write("Enter rental rate of the vehicle in PHP/hr (minimum should be 100 Php/hr and max 100,000 Php/hr): ");
                price = Console.ReadLine();

                success = int.TryParse(price, out tempprice);

                if (!success || tempprice < 100 || tempprice > 100000)
                    Console.WriteLine("Please enter a rate between 100 PHP/hr and 100,000 Php/hr!");
            } while (!success || tempprice < 100 || tempprice > 100000);
            return price + " PHP/hr";
        }
    }

    internal class UserManager : UserDetailManager, IUserManagement
    {
        public void ViewUserDetails(string username, FilePathManager file) //MAIN METHOD 1 for manage user account
        {
            UserFile user = new UserFile();
            user.DisplayUserFile(username);
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
                Console.Write("Month: ");
                details[1] = Console.ReadLine();
                if (details[1] == "")
                    Console.WriteLine("Please enter a proper month");
            } while (details[1] == "");
            do
            {
                Console.Write("Day: ");
                details[2] = Console.ReadLine();
                if (details[2] == "")
                    Console.WriteLine("Please enter a proper day");
            } while (details[2] == "");
            do
            {
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
                Console.Write("Enter your home address: ");
                address = Console.ReadLine();
                if (address == "")
                    Console.WriteLine("Please do not leave the address empty!");
            } while (address == "");
            return address;
        }
    }
}
