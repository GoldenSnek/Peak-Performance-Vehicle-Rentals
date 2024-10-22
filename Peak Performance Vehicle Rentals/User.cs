using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

//To-Do List:
//1. change filepath for different devices (fixed 10/19/24)
//2. duplicate vehicle names should be allowed, duplicate files will have vehiclename[1], [2], [3]... [n]
//3. added vehicles should be linked to the user, only the user can delete their own vehicles
//4. added vehicle deletion (unfinished 10/21/24) (super dirty code that must be revised)

namespace Peak_Performance_Vehicle_Rentals
{

    public class User
    {

    }

    public class FilePathManager
    {
        //create a base directory for storing the data
        private string baseDirectory;
        public string BaseDirectory { get { return baseDirectory; } set { baseDirectory = value; } }
        public FilePathManager()
        {
            BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Peak Performance Vehicle Rentals");
            Directory.CreateDirectory(BaseDirectory);
            Directory.CreateDirectory(BaseDirectory + "\\UserData");
            Directory.CreateDirectory(BaseDirectory + "\\VehicleData");
            if (!File.Exists(BaseDirectory + "\\Users.txt"))
            {
                // Create the file and write the username and password
                StreamWriter writer = new StreamWriter(BaseDirectory + "\\Users.txt");
            }
        }
        //for creating individual user/vehicle files
        public string GetUserFilePath(string username)
        {
            return BaseDirectory + $"\\UserData\\{username}.txt";
        }
        //for creating individual vehicle files
        public string GetVehicleFilePath(string model, string type, string username)
        {
            return BaseDirectory + $"\\VehicleData\\{model}-{type}-{username}.txt";
        }
    }

    public class UserFile : FilePathManager //class for managing each individual user
    {
        private FilePathManager file = new FilePathManager();
        public void CreateUserFile(string username, string password) //create user file
        {
            string filePath = file.GetUserFilePath(username);

            // Check if the file already exists
            if (!File.Exists(filePath))
            {
                // Create the file and write the username and password
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"Username: {username}");
                    writer.WriteLine($"Password: {password}");
                }
            }
        }

        public void UpdateUserFile() //update details of the user
        {

        }

        public void DeleteUserFile() //delete user file
        {

        }
    }

    public class VehicleFile : FilePathManager //class for managing each individual vehicle
    {
        private FilePathManager file = new FilePathManager();

        public void CreateVehicleFile(string username, string[] details) //create vehicle file
        {
            string filePath = file.GetVehicleFilePath(details[2], details[0], username); // 2 model, 0 type

            string[] type = details[0].Split("-");

            // Check if the file already exists
            if (!File.Exists(filePath))
            {
                // Create the file and write the vehicle name
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"Owner: {username}");
                    writer.WriteLine($"Vehicle Type: {type[0]} ({type[1]})");
                    writer.WriteLine($"Brand: {details[1]}");
                    writer.WriteLine($"Model: {details[2]}");
                    writer.WriteLine($"Year: {details[3]}");
                    writer.WriteLine($"License Plate: {details[4]}");
                    writer.WriteLine($"Color: {details[5]}");
                    writer.WriteLine($"Fuel Type: {details[6]}");
                    writer.WriteLine($"Seating Capacity: {details[7]}");
                    writer.WriteLine($"Mileage: {details[8]} km");
                    writer.WriteLine($"Rental Price: {details[9]}");
                    writer.WriteLine($"Status: {details[10]}");
                }
            }
        }

        public void UpdateVehicleFile(string username) //update details of the vehicle
        {

        }

        public void DeleteVehicleFile(string username, FilePathManager file) //delete vehicle file
        {
            bool DVrunning = true;
            do
            {
                int ctr = Inventory.ViewOwnedVehicles(username, file);
                string[] files = Directory.GetFiles(BaseDirectory + $"\\VehicleData", "*.txt");
                
                //choose from owned vehicles
                string model = "";
                string type = "";
                int DVchoice;
                DVchoice = Choice.ViewOwnedVehiclesChoice(ctr);

                //delete the file
                for (int i = 0; i < files.Length; i++)
                {
                    //get the file name without extension
                    string fileName = Path.GetFileNameWithoutExtension(files[i]);

                    //split the name and get the vehicle name (second part)
                    string[] parts = fileName.Split('-');

                    if (i + 1 == DVchoice)
                    {
                        model = parts[0];
                        type = $"{parts[1]}-{parts[2]}";
                    }
                }
                string filePath = file.GetVehicleFilePath(model, type, username);
                for (int i = 0; i < files.Length; i++)
                {
                    if (DVchoice == i + 1)
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            Console.WriteLine("Vehicle deleted from the available rentable vehicles!");
                        }
                    }
                }
                if (DVchoice == 0)
                    DVrunning = false;
            } while (DVrunning);
           
        }
        public void DisplayVehicleFile() //choose from all available vehicles
        {
            bool DVrunning = true;
            do
            {
                string[] files = Directory.GetFiles(BaseDirectory + $"\\VehicleData", "*.txt");

                int DVchoice;
                DVchoice = Choice.ViewAllVehiclesChoice(files);
                for (int i = 0; i < files.Length; i++)
                {
                    if (DVchoice == i + 1)
                    {
                        Console.WriteLine("\nVehicle Details:");
                        Console.WriteLine(File.ReadAllText(files[i]));
                    }
                }
                if (DVchoice == 0)
                    DVrunning = false;
            } while (DVrunning);

        }

    }
}