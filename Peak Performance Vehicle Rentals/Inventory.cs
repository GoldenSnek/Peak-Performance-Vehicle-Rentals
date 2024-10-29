using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Inventory : IInventoryManagement
    {
        public string[] ViewVehicles(FilePathManager file) //return all cars in inventory
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + $"\\VehicleData", "*.txt");
            string[] vehicles = new string[files.Length + 1];
            string brand = "";

            //display the names of the text files
            for (int i = 0; i < files.Length; i++)
            {
                //get the file name without extension
                string fileName = Path.GetFileNameWithoutExtension(files[i]);

                //split the name and get the vehicle name (first part a.k.a. index 0)
                string[] parts = fileName.Split('-');

                string line;
                using (var reader = new StreamReader(files[i])) //find the brand
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Brand:"))
                        {
                            string[] brandparts = line.Split(": ");
                            brand = brandparts[1];
                        }
                    }
                }
                vehicles[i] = $"{brand} {parts[0]}";
            }

            vehicles[files.Length] = "Go back to Main Menu";
            return vehicles;
        }

        public string[] ViewVehicles(string username, FilePathManager file) //overload method that returns the cars that are linked to the user
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + $"\\VehicleData", "*.txt");
            int length = 0;
            string brand = "";

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

            string[] vehicles = new string[length + 1]; //identify vehicles
            int ctr = 0;
            for (int i = 0; i < files.Length; i++)
            {
                //get the file name without extension
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                //split the name and match the username
                string[] parts = fileName.Split('-');

                if (parts[3] == username)
                {
                    string line;
                    using (var reader = new StreamReader(files[i])) //find the brand
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.StartsWith("Brand:"))
                            {
                                string[] brandparts = line.Split(": ");
                                brand = brandparts[1];
                            }
                        }
                    }
                    vehicles[ctr++] = $"{brand} {parts[0]}";
                }
            }

            vehicles[length] = "Go back to Main Menu";
            return vehicles;
        }

        public string[] ViewVehicleDetails(string username, FilePathManager file, int choice)
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + $"\\VehicleData", $"*{username}.txt");
            int index = 0;

            for (int i = 0; i < files.Length; i++)
            {
                if (i == choice)
                {
                    index = i;
                    break;
                }
            }

            string[] details = new string[10]; //identify details of specific vehicle
            if (index < files.Length)
            {
                for (int i = 0; i < details.Length; i++) //start at 4 to skip changing the important details
                {
                    string line;
                    using (var reader = new StreamReader(files[index]))
                    {
                        int ctr = 0;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (ctr == i + 4)
                                details[i] = line;
                            ctr++;
                        }
                    }
                }
                details[9] = "Go back to Main Menu";
            }
            else
                details[0] = "";
            return details;
        }
        public string[] ViewUserDetails(string username, FilePathManager file)
        {
            string directory = file.BaseDirectory + $"\\UserData\\{username}.txt";

            string[] details = new string[4]; //identify details of user
            for (int i = 0; i < details.Length-1; i++)
            {
                string line;
                using (var reader = new StreamReader(directory))
                {
                    int ctr = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (ctr == i + 1)
                            details[i] = line;
                        ctr++;
                    }
                }
            }
            details[3] = "Go back to Main Menu";
            return details;
        }
    }
}
