using System;
using System.IO;

//change filepath for different devices

namespace Peak_Performance_Vehicle_Rentals
{

    public class User
    {
        private static List<User> users = new List<User>();
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

        //for creating individual user files
        public string GetUserFilePath(string username)
        {
            return BaseDirectory + $"\\UserData\\{username}.txt";
        }


        /*
        public string GetVehicleFilePath(string vehiclename)
        {
            return baseDirectory + $"\\VehicleData\\{vehiclename}.txt");
        }
        */

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

        public void DeleteUserFile() //delete user file
        {

        }
    }

    public class VehicleFile : FilePathManager //class for managing each individual vehicle
    {
    }
    
}