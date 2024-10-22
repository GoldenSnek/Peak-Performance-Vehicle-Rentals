using System;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.Marshalling;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class ViewVehicles
    {
        public static void ViewRentalVehicles(FilePathManager file)
        {
            VehicleFile vehicle = new VehicleFile();
            Inventory.ViewAllVehicles(file);
            vehicle.DisplayVehicleFile();
        }
    }

    internal class ViewRental
    {
    }

    internal class ManageVehicles
    {
        public static void AddVehicle(string username)
        {

            bool success = false;
            string[] details = new string[11];

            //type
            details[0] = Choice.VehicleTypeChoice();
            //brand
            do
            {
                Console.Write("Enter vehicle brand: ");
                details[1] = Console.ReadLine();
                if (details[1] == "")
                    Console.WriteLine("Please do not leave the vehicle brand empty");
            } while (details[1] == "");
            //model
            do
            {
                Console.Write("Enter vehicle model: ");
                details[2] = Console.ReadLine();
                if (details[2] == "")
                    Console.WriteLine("Please do not leave the vehicle model empty");
            } while (details[2] == "");
            //year
            int year;
            do
            {
                Console.Write("Enter year of the vehicle: ");
                details[3] = Console.ReadLine();

                success = int.TryParse(details[3], out year);

                if (!success || year < 0 || year > 3000)
                    Console.WriteLine("Please enter a proper year!");
            } while (!success || year < 0 || year > 3000);
            //license plate
            do
            {
                Console.Write("Enter license plate #: ");
                details[4] = Console.ReadLine();
                if (details[4] == "")
                    Console.WriteLine("Please do not leave the license plate # empty");
            } while (details[4] == "");
            //color
            int color;
            do
            {
                Console.Write("Enter color of the vehicle: ");
                details[5] = Console.ReadLine();

                success = int.TryParse(details[5], out color);

                if (success || details[5] == "")
                    Console.WriteLine("Please enter a proper color!");
            } while (success || details[5] == "");
            //fuel
            details[6] = Choice.VehicleFuelChoice();
            //seating capacity
            string[] parts = details[0].Split("-");
            int seats;
            if (parts[0] == "Car")
            {
                do
                {
                    Console.Write("Enter the number of seats of the car: ");
                    details[7] = Console.ReadLine();

                    success = int.TryParse(details[7], out seats);

                    if (!success || seats < 1 || seats > 50)
                        Console.WriteLine("Please enter the proper amount of seats!");
                } while (!success || seats < 1 || seats > 50);
            }
            else if (parts[0] == "Motorcycle")
            {
                do
                {
                    Console.Write("Enter the number of seats of the motorcycle: ");
                    details[7] = Console.ReadLine();

                    success = int.TryParse(details[7], out seats);

                    if (!success || seats < 1 || seats > 3)
                        Console.WriteLine("Please enter the proper amount of seats!");
                } while (!success || seats < 1 || seats > 3);
            }
            //mileage
            int mileage;
            do
            {
                Console.Write("Enter mileage of the vehicle in kilometers: ");
                details[8] = Console.ReadLine();

                success = int.TryParse(details[8], out mileage);

                if (!success || mileage < 0)
                    Console.WriteLine("Please enter a realistic mileage!");
            } while (!success || mileage < 0);
            //rental price
            int price;
            do
            {
                Console.Write("Enter rental rate of the vehicle in Php/hr (minimum should be 100 Php/hr): ");
                details[9] = Console.ReadLine();

                success = int.TryParse(details[9], out price);

                if (!success || price < 100)
                    Console.WriteLine("Please enter a rate higher than 1001!");
            } while (!success || mileage < 100);
            //status
            details[10] = Choice.VehicleStatusChoice();

            //create a new vehicle file
            VehicleFile vehicle = new VehicleFile();
            vehicle.CreateVehicleFile(username, details);

            Console.WriteLine("New car added!");
        }

        //finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later
        public static void UpdateVehicle(string username)
        {
            //Update vehicle file
            VehicleFile vehicle = new VehicleFile();
            vehicle.UpdateVehicleFile(username);

        }
        //finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later finish later

        public static void DeleteVehicle(string username, FilePathManager file)
        {

            //Delete vehicle file
            VehicleFile vehicle = new VehicleFile();
            vehicle.DeleteVehicleFile(username, file);
        }
    }

}
