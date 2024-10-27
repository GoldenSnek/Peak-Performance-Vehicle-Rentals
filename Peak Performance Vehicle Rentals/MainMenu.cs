using System;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.Marshalling;

//To-Do List
//1. [top priority] Edit vehicle details (ugma or kung kanusa naay time)
//2. View Rental History
//3. Add user details when displaying vehicle details
//4. Add rental process and application (final part og ang pinakalisod nga part before ko mo proceed sa pa chuychuy sa UI)

namespace Peak_Performance_Vehicle_Rentals
{
    internal class VehicleManager : VehicleDetailManager, IVehicleManagement
    {
        public void ViewRentalVehicles(FilePathManager file)
        {
            int choice;
            Choice choose = new Choice();
            Inventory inventory = new Inventory();
            do
            {
                choice = choose.ViewAllVehiclesChoice(file); //problem here
                
                if (choice != inventory.ViewVehicles(file).Length - 1)
                {
                    VehicleFile vehicle = new VehicleFile();
                    vehicle.DisplayVehicleFile(choice);
                    UserInterface UI = new UserInterface("Press any key if you are done reading the details");
                    UI.WaitForKey();
                }
            } while (choice != inventory.ViewVehicles(file).Length - 1);
        }
        public void AddVehicle(string username)
        {

            bool success = false;
            string[] details = new string[11];
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
            //rental price
            details[9] = VehiclePrice();
            //status
            details[10] = choose.VehicleStatusChoice();

            //create a new vehicle file
            VehicleFile vehicle = new VehicleFile();
            vehicle.CreateVehicleFile(username, details);

            Console.WriteLine("New car added!"); Thread.Sleep(1000);
        }

        //finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later
        public void UpdateVehicle(string username, FilePathManager file)
        {
            //Update vehicle file
            int choice;
            string newdetail = "";
            Choice choose = new Choice();
            Inventory inventory = new Inventory();
            do {
                choice = choose.ViewOwnedVehiclesChoice(username, file);
                if (choice != inventory.ViewVehicles(username, file).Length - 1)
                {
                    string detailchoice = choose.UpdateVehiclesDetailsChoice(username, file, choice);

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
                    if (detailchoice == "Rental Price")
                        newdetail = VehiclePrice();
                    if (detailchoice == "Status")
                        newdetail = choose.VehicleStatusChoice();
                    if (detailchoice != "")
                    {
                        VehicleFile vehicle = new VehicleFile();
                        vehicle.UpdateVehicleFile(username, file, choice, detailchoice, newdetail);
                    }
                }
            } while (choice != inventory.ViewVehicles(username, file).Length - 1);
        }
        //finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later

        public void DeleteVehicle(string username, FilePathManager file)
        {

            //Delete vehicle file
            Choice choose = new Choice();
            int choice = choose.ViewOwnedVehiclesChoice(username, file);
            VehicleFile vehicle = new VehicleFile();
            vehicle.DeleteVehicleFile(username, file, choice);
        }
    }

    internal class VehicleDetailManager : IVehicleDetailManagement
    {
        public string VehicleName()
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
        public string VehicleModel()
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
        public string VehicleYear()
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
        public string VehicleLicensePlate()
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
        public string VehicleColor()
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
        public string VehicleSeatingCapacity()
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
        public string VehicleMileage()
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
            return mileage;
        }
        public string VehiclePrice()
        {
            string price;
            int tempprice;
            bool success = false;
            do
            {
                Console.Write("Enter rental rate of the vehicle in Php/hr (minimum should be 100 Php/hr and max 100,000 Php/hr): ");
                price = Console.ReadLine();

                success = int.TryParse(price, out tempprice);

                if (!success || tempprice < 100 || tempprice > 100000)
                    Console.WriteLine("Please enter a rate between 100 Php/hr and 100,000 Php/hr!");
            } while (!success || tempprice < 100 || tempprice > 100000);
            return price;
        }
    }
}
