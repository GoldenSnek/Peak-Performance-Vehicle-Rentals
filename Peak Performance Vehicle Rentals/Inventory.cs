using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Inventory : IInventoryManagement
    {
        public string[] ViewVehicles(FilePathManager file) //MAIN METHOD 1 (overload 1), view all vehicles
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

            vehicles[files.Length] = "Go back to Main Menu";
            return vehicles;
        }

        public string[] ViewVehicles(string username, FilePathManager file) //MAIN METHOD 1 (overload 2), view owned vehicles
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

            vehicles[files.Length] = "Go back to Main Menu";
            return vehicles;
        }

        public string[] ViewVehicleDetails(string username, FilePathManager file, int choice) //MAIN METHOD 2, view vehicle details
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

            string[] details = new string[11]; //identify details of specific vehicle
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
                details[10] = "Go back to Main Menu";
            }
            else
                details[0] = "";
            return details;
        }

        public string[] ViewUserDetails(string username, FilePathManager file) //MAIN METHOD 3, view user details
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
        public string[] ViewPendingRental(string username, FilePathManager file) //MAIN METHOD ?
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\PendingRental", $"*.txt");
            List<string> vehicles = new List<string>(); //identify vehicles
            string name = "";
            string brand = "";
            int ctr = 0;

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
                                ctr++;
                            }
                        }
                    }
                }
            }

            vehicles.Add("Go back to Main Menu");
            return vehicles.ToArray();
        }

        public string[] ViewApprovedRental(string username, FilePathManager file) //MAIN METHOD ?
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\ApprovedRental", $"*{username}.txt");
            string[] vehicles = new string[files.Length];
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
                vehicles[i] = $"\"{brand} {parts[0]}\" is currently being rented by \"{client}\"";
            }
            return vehicles;
        }
        public string CurrentRental(string username, FilePathManager file) //MAIN METHOD ?
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\ApprovedRental", $"*.txt");
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
                                Console.Clear();
                                string Prompt = @$"
                                                ____ _  _ ____ ____ ____ _  _ ___ _    _   _    ____ ____ _  _ ___ _ _  _ ____ 
                                                |    |  | |__/ |__/ |___ |\ |  |  |     \_/     |__/ |___ |\ |  |  | |\ | | __ 
                                                |___ |__| |  \ |  \ |___ | \|  |  |___   |      |  \ |___ | \|  |  | | \| |__] 

                                                                               {vehicle}
                                                                ";

                                UserInterface.CenterVerbatimText(Prompt);
                            }
                        }
                    }
                }
            }
            return vehicle;
        }
    }
}