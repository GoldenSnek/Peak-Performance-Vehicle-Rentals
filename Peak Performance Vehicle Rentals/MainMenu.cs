using System;
using System.IO;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class ViewVehicles
    {
        public static void ViewRentalVehicles()
        {
            VehicleFile vehicle = new VehicleFile();
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

            string vehiclename;
            do
            {
                Console.Write("Enter Vehicle Name: ");
                vehiclename = Console.ReadLine();
                if (vehiclename == "")
                    Console.WriteLine("Please do not leave the Vehicle Name empty");
            } while (vehiclename == "");

            //create a new vehicle file
            VehicleFile vehicle = new VehicleFile();
            vehicle.CreateVehicleFile(username, vehiclename);

            Console.WriteLine("New car added!");
        }

        public static void UpdateVehicle(string username)
        {
            Console.WriteLine("hello update vehicle");

        }

        public static void DeleteVehicle(string username)
        {
            string vehiclename;
            do
            {
                Console.Write("Enter Vehicle Name: ");
                vehiclename = Console.ReadLine();
                if (vehiclename == "")
                    Console.WriteLine("Please do not leave the Vehicle Name empty");
            } while (vehiclename == "");

            //create a new vehicle file
            VehicleFile vehicle = new VehicleFile();
            vehicle.DeleteVehicleFile(username, vehiclename);

            Console.WriteLine("Car deleted!");

        }
    }





}
