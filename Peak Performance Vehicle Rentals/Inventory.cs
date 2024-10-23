using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Inventory
    {

        public static string[] ViewAllVehicles(FilePathManager file)
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + $"\\VehicleData", "*.txt");
            string[] vehicles = new string[files.Length+1];   

            //display the names of the text files
            for (int i = 0; i < files.Length; i++)
            {
                //get the file name without extension
                string fileName = Path.GetFileNameWithoutExtension(files[i]);

                //split the name and get the vehicle name (first part a.k.a. index 0)
                string[] parts = fileName.Split('-');

                vehicles[i] = parts[0];

            }

            vehicles[files.Length] = "Go back to Main Menu";
            return vehicles;
        }

        public static int ViewOwnedVehicles(string username, FilePathManager file) //returns how many cars the user has
        {
            int ctr = 0;
            string[] files = Directory.GetFiles(file.BaseDirectory + $"\\VehicleData", "*.txt");

            //display the names of the text files
            Console.WriteLine("Select a vehicle that you own which you want to remove from the rentable vehicles: ");
            for (int i = 0; i < files.Length; i++)
            {
                //get the file name without extension
                string fileName = Path.GetFileNameWithoutExtension(files[i]);

                //split the name and get the vehicle name (second part)
                string[] parts = fileName.Split('-');

                if (parts[3] == username)
                {
                    Console.WriteLine($"({i + 1}) {parts[0]}"); //show only the vehicle name
                    ctr++;
                }
            }

            return ctr;
        }

    }
}
