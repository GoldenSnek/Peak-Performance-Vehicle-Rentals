using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Inventory : IInventoryManagement
    {
        public string[] ViewAllVehicles(string type, FilePathManager file) //MAIN METHOD 1, view all
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\VehicleData", "*.txt");
            string[] vehicles = new string[files.Length + 1];
            string brand = "";

            //display the names of the text files
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string[] parts = fileName.Split('-'); //split the name and get the vehicle name (first part a.k.a. index 0)

                string line;
                using (var reader = new StreamReader(files[i])) //find the brand to be included in the display
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

            if (type == "Admin")
                vehicles[files.Length] = "Go back to Main  Menu";
            else
                vehicles[files.Length] = "Go back to Rental Vehicles Menu";
            return vehicles;
        }
        public string[] ViewOwnedVehicles(string username, FilePathManager file) //MAIN METHOD 2, view owned
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\VehicleData", $"*{username}.txt");
            string brand = "";
            string[] vehicles = new string[files.Length + 1]; //identify vehicles

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string[] parts = fileName.Split('-'); //split the name and get the vehicle name (first part a.k.a. index 0)
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

            vehicles[files.Length] = "Go back to Manage Vehicles Menu";
            return vehicles;
        }
        public string[] ViewSearchedVehicles(string keyword, string type, FilePathManager file) //MAIN METHOD 3, OP search method
        {
            List<string> vehicles = new List<string>();
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\VehicleData", "*.txt");

            for (int i = 0; i < files.Length; i++)
            {
                string[] lines = File.ReadAllLines(files[i]);

                string[] details = new string[13];

                //store values for searching
                details[0] = ExtractValue(lines, "Owner:");
                details[1] = ExtractValue(lines, "Vehicle Type:");
                details[2] = ExtractValue(lines, "Brand:");
                details[3] = ExtractValue(lines, "Model:");
                details[4] = ExtractValue(lines, "Manufacture Year:");
                details[5] = ExtractValue(lines, "License Plate:");
                details[6] = ExtractValue(lines, "Color:");
                details[7] = ExtractValue(lines, "Fuel Type:");
                details[8] = ExtractValue(lines, "Seating Capacity:");
                details[9] = ExtractValue(lines, "Mileage:");
                details[10] = ExtractValue(lines, "Pickup and Return Location:");
                details[11] = ExtractValue(lines, "Daily Rental Price:");
                details[12] = ExtractValue(lines, "Hourly Rental Price:");

                for (int j = 0; j < details.Length; j++)
                {
                    if (type == "search")
                    {
                        if (details[j].ToLower() == keyword.ToLower()) //to lower para di mo matter ang capitalization
                        {
                            vehicles.Add($"{details[2]} {details[3]}");
                            break;
                        }
                    }
                    if (type == "path")
                    {
                        if (details[j].ToLower() == keyword.ToLower()) //to lower para di mo matter ang capitalization
                        {
                            vehicles.Add(Path.GetFileName(files[i]));
                            break;
                        }
                    }
                }
            }

            if (type == "search")
                vehicles.Add("Go back to Rental Vehicles Menu");

            return vehicles.ToArray();
        }

        public string[] ViewVehicleDetails(string username, FilePathManager file, int choice) //MAIN METHOD 4, view vehicle details
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\VehicleData", $"*{username}.txt");
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
                for (int i = 0; i < details.Length; i++) //start at 4 to skip to the important details
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
                details[9] = "Go back and select another vehicle";
            }
            else
                details[0] = "";
            return details;
        }

        public string[] ViewUserDetails(string username, FilePathManager file) //MAIN METHOD 5, view user details
        {
            string directory = file.BaseDirectory + $"\\UserData\\{username}.txt";

            string[] details = new string[5]; //identify details of user
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
            details[4] = "Go back to Manage User Menu";
            return details;
        }
        public string ViewPendingRentalClient(string username, FilePathManager file) //MAIN METHOD 6, view pending vehicle of client
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\PendingRental", "*.txt");
            string vehicleClient = "";
            string name = "";

            for (int i = 0; i < files.Length; i++)
            {
                string line;
                using (var reader = new StreamReader(files[i]))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Name:"))
                        {
                            string[] nameParts = line.Split(": ");
                            name = nameParts[1];

                            if (name == username)
                            {
                                vehicleClient = name;
                                break;
                            }
                        }
                    }
                }
            }
            return vehicleClient;
        }
        public string[] ViewPendingRentalOwner(string username, FilePathManager file) //MAIN METHOD 7, view pending vehicles of user
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\PendingRental", "*.txt");
            List<string> vehicles = new List<string>(); //identify vehicles
            string name = "";
            string brand = "";

            for (int i = 0; i < files.Length; i++)
            {
                string line;
                using (var reader = new StreamReader(files[i]))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Owner:"))
                        {
                            string[] nameParts = line.Split(": ");
                            name = nameParts[1];

                            if (name == username)
                            {
                                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                                string[] parts = fileName.Split('-'); //split the name and get the vehicle name (first part a.k.a. index 0)
                                string lineBrand;
                                using (var readerBrand = new StreamReader(files[i]))
                                {
                                    while ((lineBrand = readerBrand.ReadLine()) != null)
                                    {
                                        if (lineBrand.StartsWith("Brand:"))
                                        {
                                            string[] brandParts = lineBrand.Split(": ");
                                            brand = brandParts[1];
                                        }
                                    }
                                    vehicles.Add($"{brand} {parts[0]}");
                                }
                            }
                        }
                    }
                }
            }
            vehicles.Add("Go back to Rental Details Menu");
            return vehicles.ToArray();
        }

        public string[] ViewApprovedRental(string username, FilePathManager file) //MAIN METHOD 8, view all approved vehicles
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\ApprovedRental", $"*{username}.txt");
            string[] vehicles = new string[files.Length+1];
            string brand = "";
            string client = "";

            //display the names of the text files
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string[] parts = fileName.Split('-'); //split the name and get the vehicle name (first part a.k.a. index 0)

                string line;
                using (var reader = new StreamReader(files[i])) //find the brand to be included in the display
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Brand:"))
                        {
                            string[] brandparts = line.Split(": ");
                            brand = brandparts[1];
                        }
                        if (line.StartsWith("Name:"))
                        {
                            string[] clientParts = line.Split(": ");
                            client = clientParts[1];
                        }
                    }
                }
                vehicles[i] = $"{brand} {parts[0]}";
            }
            vehicles[files.Length] = "Go back to Rental Details Menu";
            return vehicles;
        }
        public string ViewCurrentRental(string username, FilePathManager file) //MAIN METHOD 9: view current rental vehicle
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\ApprovedRental", "*.txt");
            string vehicle = "";
            string brand = "";
            string client = "";

            //display the names of the text files
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string[] parts = fileName.Split('-'); //split the name and get the vehicle name (first part a.k.a. index 0)

                string line;
                using (var reader = new StreamReader(files[i])) //find the brand to be included in the display
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Brand:"))
                        {
                            string[] brandparts = line.Split(": ");
                            brand = brandparts[1];
                        }
                        if (line.StartsWith("Name:"))
                        {
                            string[] clientParts = line.Split(": ");
                            client = clientParts[1];
                            if (client == username) //car is owned by user a.k.a. me
                            {
                                vehicle = $"{brand} {parts[0]}";
                            }
                        }
                    }
                }
            }
            return vehicle;
        }

        public string[] ViewUsers(FilePathManager file) //MAIN METHOD 10: view all users
        {
            List<string> users = new List<string>();
            string[] details = new string[3];

            string line;
            using (StreamReader reader = new StreamReader(file.BaseDirectory + "\\Users.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith("ADMIN"))
                    {
                        details = line.Split(",");
                        users.Add($"{details[0]} | {details[2]}");
                    }
                }
            }
            users.Add("Go back to Main Menu");
            return users.ToArray();
        }

        internal static string ExtractValue(string[] lines, string key) //SUPPORTING and EXTRA METHOD, extracts the data from each line
        {
            foreach (string line in lines)
            {
                if (line.StartsWith(key))
                {
                    return line.Substring(key.Length).Trim();
                }
            }
            return string.Empty; //nothing
        }
    }
}