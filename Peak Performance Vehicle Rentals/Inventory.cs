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

        public static string[] ViewOwnedVehicles(string username, FilePathManager file) //returns the cars cars the user has
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + $"\\VehicleData", "*.txt");
            int length = 0;

            for (int i = 0; i < files.Length; i++) //identify size
            {
                //get the file name without extension
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                //split the name and match the username
                string[] parts = fileName.Split('-');

                if (parts[3] == username)
                {
                    length++;
                }
            }

            string[] vehicles = new string[length+1]; //identify vehicles
            int ctr = 0;
            for (int i = 0; i < files.Length; i++)
            {
                //get the file name without extension
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                //split the name and match the username
                string[] parts = fileName.Split('-');

                if (parts[3] == username)
                {
                    vehicles[ctr++] = parts[0];
                }
            }

            vehicles[length] = "Go back to Main Menu";
            return vehicles;
        }

    }
}
