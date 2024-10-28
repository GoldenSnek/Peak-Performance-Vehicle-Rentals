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
    public class FilePathManager
    {
        private string baseDirectory;
        public string BaseDirectory { get { return baseDirectory; } set { baseDirectory = value; } }
        public FilePathManager() //create a base directory for storing the data
        {
            BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Peak Performance Vehicle Rentals");
            Directory.CreateDirectory(BaseDirectory);
            Directory.CreateDirectory(BaseDirectory + "\\UserData");
            Directory.CreateDirectory(BaseDirectory + "\\VehicleData");
            if (!File.Exists(BaseDirectory + "\\Users.txt"))
            {
                StreamWriter writer = new StreamWriter(BaseDirectory + "\\Users.txt");
            }
        }
        internal string GetUserFilePath(string username) //for accessing user filepath
        {
            return BaseDirectory + $"\\UserData\\{username}.txt";
        }
        internal string GetVehicleFilePath(string model, string type, string username) //for accessing vehicle filepath
        {
            return BaseDirectory + $"\\VehicleData\\{model}-{type}-{username}.txt";
        }
    }
    internal class UserFile : FilePathManager, IUserFileManagement //class for managing each individual user
    {
        public void CreateUserFile(string username) //create user file
        {
            string filePath = GetUserFilePath(username);

            if (!File.Exists(filePath)) //check if the file already exists
            {
                using (var writer = new StreamWriter(filePath)) // Create the file and write the username and password
                {
                    writer.WriteLine($"Username: {username}");
                    writer.WriteLine($"Email Address: ");
                    writer.WriteLine($"Date of Birth (MM/DD/YY): ");
                    writer.WriteLine($"Address: ");
                    writer.WriteLine($"Account creation date: ");
                }
            }
        }

        public void UpdateUserFile() //update details of the user
        {

        }

        public void DeleteUserFile() //delete user file
        {

        }
        public void DisplayUserFile(string username) //display details of user
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\UserData", $"{username}.txt");
            Console.WriteLine("\nUser Details");
            Console.WriteLine(File.ReadAllText(files[0]));
        }
    }

    internal class VehicleFile : FilePathManager,  IVehicleFileManagement //class for managing each individual vehicle
    {

        public void CreateVehicleFile(string username, string[] details) //create vehicle file
        {
            string filePath = GetVehicleFilePath(details[2], details[0], username); // 2 model, 0 type

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
                    writer.WriteLine($"Mileage: {details[8]}");
                    writer.WriteLine($"Pickup and Drop-off Location: {details[9]}");
                    writer.WriteLine($"Rental Price: {details[10]}");
                    writer.WriteLine($"Status: {details[11]}");
                }
            }
        }

        public void UpdateVehicleFile(string username, FilePathManager file, int choice, string detailchoice, string newdetail) //update details of the vehicle
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + $"\\VehicleData", $"*{username}.txt");
            string tempPath = file.BaseDirectory + "\\Temp.txt";

            using (StreamReader reader = new StreamReader(files[choice]))
            using (StreamWriter writer = new StreamWriter(tempPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Check if the current line starts with the search line
                    if (line.StartsWith(detailchoice))
                    {
                        writer.WriteLine($"{detailchoice}: {newdetail}"); // Write the new line to the temp file
                    }
                    else
                    {
                        writer.WriteLine(line); // Write the original line to the temp file
                    }
                }
            }

            //replace the original file with the updated temp file
            File.Delete(files[choice]); //delete the original file
            File.Move(tempPath, files[choice]);
            File.Delete(tempPath);

            Console.WriteLine("Vehicle has been successfuly updated!"); Thread.Sleep(1000);
        }

        public void DeleteVehicleFile(string username, FilePathManager file, int choice) //delete vehicle file
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\VehicleData", $"*{username}.txt");
                
            //delete the file
            for (int i = 0; i < files.Length; i++)
            {
                if (choice == i)
                if (File.Exists(files[i]))
                {
                    File.Delete(files[i]);
                    Console.WriteLine("Vehicle has been deleted from the inventory!"); Thread.Sleep(1000);
                    break;
                }
            }
        }
        public void DisplayVehicleFile(int DVchoice) //display details of vehicles
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\VehicleData", "*.txt");
            for (int i = 0; i < files.Length; i++)
            {
                if (DVchoice == i)
                {
                    Console.WriteLine("\nVehicle Details");
                    Console.WriteLine(File.ReadAllText(files[i]));
                }
            }

        }

    }
}