using System;
using System.IO;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

//To-Do List:
//1. change filepath for different devices (fixed 10/19/24)
//2. duplicate vehicle names should be allowed, duplicate files will have vehiclename[1], [2], [3]... [n]
//3. added vehicles should be linked to the user, only the user can delete their own vehicles
//4. added vehicle deletion (unfinished 10/21/24) (super dirty code that must be revised)

//Notes:
//1. for every split, index [3] is username, might change the filenames later so need sad ni i change ang mga splits

namespace Peak_Performance_Vehicle_Rentals
{
    public class FilePathManager : FilePathManagerBase
    {
        public FilePathManager() //CONSTRUCTOR for creating necessary directories for storing the data
        {
            BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Peak Performance Vehicle Rentals");
            Directory.CreateDirectory(BaseDirectory);
            Directory.CreateDirectory(BaseDirectory + "\\UserData");
            Directory.CreateDirectory(BaseDirectory + "\\VehicleData");
            Directory.CreateDirectory(BaseDirectory + "\\RentalData\\PendingRental");
            Directory.CreateDirectory(BaseDirectory + "\\RentalData\\ApprovedRental");
            if (!File.Exists(BaseDirectory + "\\Users.txt"))
            {
                StreamWriter writer = new StreamWriter(BaseDirectory + "\\Users.txt");
            }
        }
        internal string GetUserFilePath(string username) //METHOD for accessing user filepath
        {
            return BaseDirectory + $"\\UserData\\{username}.txt";
        }
        internal string GetVehicleFilePath(string model, string type, string username) //METHOD for accessing vehicle filepath
        {
            return BaseDirectory + $"\\VehicleData\\{model}-{type}-{username}.txt";
        }
        internal string GetRentalFilePath(string model, string type, string username) //METHOD for accessing vehicle filepath
        {
            return BaseDirectory + $"\\RentalData\\{username}.txt";
        }
    }

    internal class UserFile : FilePathManager, IUserFileManagement
    {
        public void CreateUserFile(string username) //METHOD for creating user file
        {
            string filePath = GetUserFilePath(username);

            //get current date and time for account creation
            string accountCreationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string[] creation = accountCreationDate.Split(' ');

            if (!File.Exists(filePath))
            {
                using (var writer = new StreamWriter(filePath)) //create the file and write the username and password
                {
                    writer.WriteLine($"Username: {username}");
                    writer.WriteLine($"Email Address: -no data-");
                    writer.WriteLine($"Date of Birth (MM/DD/YY): -no data-");
                    writer.WriteLine($"Address: -no data-");
                    writer.WriteLine($"Account creation date: {creation[0]}");
                }
            }
        }

        public void UpdateUserFile(string username, string detailchoice, string newdetail) //METHOD for updating the details of the user
        {
            string filePath = GetUserFilePath(username);
            string tempPath = BaseDirectory + "\\Temp.txt";

            using (StreamReader reader = new StreamReader(filePath))
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
            File.Delete(filePath); //delete the original file
            File.Move(tempPath, filePath);
            File.Delete(tempPath);

            Console.WriteLine("User details has been successfuly updated!"); Thread.Sleep(1000);
        }

        public void DeleteUserFile(string username) //METHOD for deleting user file + vehicle files linked with the user
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\VehicleData", $"*{username}.txt");
            string filePath = BaseDirectory + "\\Users.txt";
            string tempPath = BaseDirectory + "\\Temp.txt";

            //delete the user line from the main user file
            using (StreamReader reader = new StreamReader(filePath))
            using (StreamWriter writer = new StreamWriter(tempPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Check if the current line starts with the search line
                    if (!line.StartsWith(username))
                    {
                        writer.WriteLine(line); // Write the original line to the temp file
                    }
                }
            }

            //replace the original file with the updated temp file
            File.Delete(filePath); //delete the original file
            File.Move(tempPath, filePath);
            File.Delete(tempPath);

            //delete the actual user file and details
            if (File.Exists(GetUserFilePath(username)))
            {
                File.Delete(GetUserFilePath(username));
            }

            //delete all the vehicles of the user
            for (int i = 0; i < files.Length; i++)
            {
                if (File.Exists(files[i]))
                {
                    File.Delete(files[i]);
                }
            }
            Console.WriteLine("User Account has been successfully deleted!"); Thread.Sleep(1000);
            Console.WriteLine("Returning to login screen..."); Thread.Sleep(1000);
        }

        public void DisplayUserFile(string type, string username) //METHOD for displaying info inside the user file
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\UserData", $"{username}.txt");
            string content = File.ReadAllText(files[0]);
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            UserInterface.CenterTextMargin(3, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (type == "user")
                Console.WriteLine("User Details");
            else if (type == "vehicle")
                Console.WriteLine("Vehicle Owner");
            Console.ResetColor();
            for (int j = 0; j < lines.Length; j++)
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.WriteLine(lines[j]);
            }
        }
    }

    internal class VehicleFile : FilePathManager, IVehicleFileManagement
    {
        public void CreateVehicleFile(string username, string[] details) //METHOD creating vehicle file
        {
            string filePath = GetVehicleFilePath(details[2], details[0], username); // 2 model, 0 type

            string[] type = details[0].Split("-");

            // Check if the file already exists
            if (!File.Exists(filePath))
            {
                string accountCreationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                string[] creation = accountCreationDate.Split(' ');

                // Create the file and write the vehicle name
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"Owner: {username}");
                    writer.WriteLine($"Vehicle Type: {type[0]} ({type[1]})");
                    writer.WriteLine($"Brand: {details[1]}");
                    writer.WriteLine($"Model: {details[2]}");
                    writer.WriteLine($"Manufacture Year: {details[3]}");
                    writer.WriteLine($"License Plate: {details[4]}");
                    writer.WriteLine($"Color: {details[5]}");
                    writer.WriteLine($"Fuel Type: {details[6]}");
                    writer.WriteLine($"Seating Capacity: {details[7]}");
                    writer.WriteLine($"Mileage: {details[8]}");
                    writer.WriteLine($"Pickup and Return Location: {details[9]}");
                    writer.WriteLine($"Daily Rental Price: {details[10]}");
                    writer.WriteLine($"Hourly Rental Price: {details[11]}");
                    writer.WriteLine($"Status: {details[12]}");
                    writer.WriteLine($"Vehicle uploaded to system on: {creation[0]}");
                }
            }
        }

        public void UpdateVehicleFile(string username, int choice, string detailchoice, string newdetail) //METHOD for updating the details of the vehicle
        {
            string[] files = Directory.GetFiles(BaseDirectory + $"\\VehicleData", $"*{username}.txt");
            string tempPath = BaseDirectory + "\\Temp.txt";

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

        public void DeleteVehicleFile(string username, FilePathManager file, int choice) //METHOD for deleting vehicle file
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

        public string[] DisplayVehicleFile(int DVchoice) //METHOD for displaying info inside the vehicle file
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\VehicleData", "*.txt");
            string[] vehicleRentDetails = new string[3]; //0 is daily, 1 is hourly, 2 is owner
            for (int i = 0; i < files.Length; i++)
            {
                if (DVchoice == i)
                {
                    Console.Clear();
                    string content = File.ReadAllText(files[i]);
                    string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    string[] details = new string[2];

                    using (StreamReader reader = new StreamReader(files[i]))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            //pa chuy chuy
                            if (line.StartsWith("Brand"))
                            {
                                string[] lineParts = line.Split(": ");
                                details[0] = lineParts[1];
                            }
                            if (line.StartsWith("Model"))
                            {
                                string[] lineParts = line.Split(": ");
                                details[1] = lineParts[1];
                            }
                            if (line.StartsWith("Daily"))
                            {
                                string[] lineParts = line.Split(" ");
                                vehicleRentDetails[0] = lineParts[3];
                            }
                            if (line.StartsWith("Hourly"))
                            {
                                string[] lineParts = line.Split(" ");
                                vehicleRentDetails[1] = lineParts[3];
                            }
                        }
                    }

                    string Prompt = @$"___  ____ ___ ____ _ _    ____ 
                                       |  \ |___  |  |__| | |    [___
                                       |__/ |___  |  |  | | |___ ___]

                                       {details[0]} {details[1]}
                                                                ";

                    UserInterface.CenterVerbatimText(Prompt);

                    UserInterface.CenterTextMargin(3, 0);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Vehicle Details");
                    Console.ResetColor();
                    for (int j = 0; j < lines.Length; j++)
                    {
                        UserInterface.CenterTextMargin(3, 0);
                        Console.WriteLine(lines[j]);
                    }

                    //also display the user details
                    string fileName = Path.GetFileNameWithoutExtension(files[i]);
                    string[] username = fileName.Split("-");
                    UserFile owner = new UserFile();
                    owner.DisplayUserFile("vehicle", username[3]);
                    vehicleRentDetails[2] = username[3];
                }
            }
            return vehicleRentDetails;
        }
        public void TransferPendingFile(int choice, string[] rentDetails, string username)
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\VehicleData", "*.txt");

            for (int i = 0; i < files.Length; i++)
            {
                if (choice == i)
                {
                    //append
                    using (StreamWriter writer = new StreamWriter(files[i], true)) // 'true' for append mode
                    {
                        writer.WriteLine("Details of the client");
                        writer.WriteLine($"Name: {username}");
                        writer.WriteLine($"Number of days/hours the vehicle will be rented: {rentDetails[0]}");
                        writer.WriteLine($"Total price: {rentDetails[1]}");
                        writer.WriteLine($"Additional information: {rentDetails[2]}");
                    }

                    File.Move(files[i], BaseDirectory + $"\\RentalData\\PendingRental\\{Path.GetFileName(files[i])}");
                    Console.WriteLine("waiting for the owners approval"); Thread.Sleep(1000);
                }
            }
        }

        public void TransferApprovedFile(int choice, string username, FilePathManager file)
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\RentalData\\PendingRental", $"*{username}.txt");

            for (int i = 0; i < files.Length; i++)
            {
                if (i == choice)
                {
                    //create a reciept
                    string[] lines = File.ReadAllLines(files[i]);

                    // Assuming we already have the details in the lines variable
                    string owner = ExtractValue(lines, "Owner:");
                    string vehicleType = ExtractValue(lines, "Vehicle Type:");
                    string brand = ExtractValue(lines, "Brand:");
                    string model = ExtractValue(lines, "Model:");
                    string year = ExtractValue(lines, "Manufacture Year:");
                    string licensePlate = ExtractValue(lines, "License Plate:");
                    string color = ExtractValue(lines, "Color:");
                    string fuelType = ExtractValue(lines, "Fuel Type:");
                    string seatingCapacity = ExtractValue(lines, "Seating Capacity:");
                    string mileage = ExtractValue(lines, "Mileage:");
                    string dailyPrice = ExtractValue(lines, "Daily Rental Price:");
                    string clientName = ExtractValue(lines, "Name:"); // Assuming this is the client's name
                    string rentalDuration = ExtractValue(lines, "Number of days/hours the vehicle will be rented:"); // As provided in the client details
                    string totalPrice = ExtractValue(lines, "Total price:"); // As provided
                    string additionalInfo = ExtractValue(lines, "Additional information:"); // Additional information

                    //delete details of the client
                    // Find the index of the line containing "Details of the client"
                    int detailsIndex = Array.IndexOf(lines, "Details of the client");

                    // Create a new array to hold the lines to keep
                    string[] newLines = new string[detailsIndex];

                    // Copy lines up to and including "Details of the client"
                    Array.Copy(lines, newLines, detailsIndex);

                    // Write the updated lines back to the file
                    File.WriteAllLines(files[i], newLines);

                    //append
                    using (StreamWriter writer = new StreamWriter(files[i], true)) // 'true' for append mode
                    {
                        writer.WriteLine("Receipt");
                        writer.WriteLine("   PEAK PERFORMANCE VEHICLE RENTALS    ");
                        writer.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                        writer.WriteLine("          Vehicle Information          ");
                        writer.WriteLine($"Vehicle Owner: {owner}");
                        writer.WriteLine($"Vehicle Name: {brand} {model} {year}");
                        writer.WriteLine($"Type: {vehicleType}");
                        writer.WriteLine($"License Plate: {licensePlate}");
                        writer.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                        writer.WriteLine("            Client Information         ");
                        writer.WriteLine($"Name: {clientName}");
                        writer.WriteLine($"Rental Duration: {rentalDuration}");
                        writer.WriteLine($"Total Price: {totalPrice}");
                        writer.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                        writer.WriteLine("   Show the receipt when you pickup    ");
                        writer.WriteLine("  or return the vehicle to the owner   ");
                    }

                    File.Move(files[i], BaseDirectory + $"\\RentalData\\ApprovedRental\\{Path.GetFileName(files[i])}");
                }
            }
        }
        static string ExtractValue(string[] lines, string key) //supporting
        {
            foreach (var line in lines)
            {
                if (line.StartsWith(key))
                {
                    return line.Substring(key.Length).Trim();
                }
            }
            return string.Empty;
        }

        public void TransferNonApprovedFile(int choice, string username, FilePathManager file)
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\RentalData\\PendingRental", $"*{username}.txt");

            for (int i = 0; i < files.Length; i++)
            {
                if (i == choice)
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(files[i]);

                    // Find the index of the line containing "Details of the client"
                    int detailsIndex = Array.IndexOf(lines, "Details of the client");

                    // Create a new array to hold the lines to keep
                    string[] newLines = new string[detailsIndex];

                    // Copy lines up to and including "Details of the client"
                    Array.Copy(lines, newLines, detailsIndex);

                    // Write the updated lines back to the file
                    File.WriteAllLines(files[i], newLines);

                    File.Move(files[i], BaseDirectory + $"\\VehicleData\\{Path.GetFileName(files[i])}");
                }
            }
        }

        public void DisplayPendingFile(int choice, string username, FilePathManager file)
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\RentalData\\PendingRental", $"*{username}.txt");
            string[] lines = new string[20];

            // Read all lines from the file
            for (int i = 0; i < files.Length; i++)
            {
                if (i == choice)
                {
                    lines = File.ReadAllLines(files[i]);
                }
            }
            // Find the index of the line containing "Details of the client"
            int detailsIndex = Array.IndexOf(lines, "Details of the client");

            UserInterface.CenterTextMargin(3, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Details of the client");
            Console.ResetColor();
            for (int i = detailsIndex + 1; i < lines.Length; i++)
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.WriteLine(lines[i]);
            }
            Console.WriteLine("");
        }

        public void DisplayRecieptFile(string username, FilePathManager file)
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\ApprovedRental", $"*.txt");
            string client = "";

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string[] parts = fileName.Split('-'); //split the name and get the vehicle name (first part a.k.a. index 0)

                string line;
                using (var reader = new StreamReader(files[i])) //find the brand to be included in the display
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Name:"))
                        {
                            string[] clientParts = line.Split(": ");
                            client = clientParts[1];
                            if (client == username) //car is owned by user a.k.a. me
                            {
                                string[] lines = new string[20];

                                lines = File.ReadAllLines(files[i]);

                                // Find the index of the line containing "Details of the client"
                                int detailsIndex = Array.IndexOf(lines, "Receipt");

                                UserInterface.CenterTextMargin(3, 0);
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Receipt: \n");
                                Console.ResetColor();
                                for (int j = detailsIndex + 1; j < lines.Length; j++)
                                {
                                    UserInterface.CenterTextMargin(3, 0);
                                    Console.WriteLine(lines[j]);
                                }
                                Console.WriteLine("");
                                UserInterface UI = new UserInterface("Press any key if you are done looking at the receipt");
                                UI.WaitForKey();
                            }
                        }
                    }
                }
            }
        }
        public void TransferFinishRentFile(string username, FilePathManager file)
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\ApprovedRental", $"*.txt");
            string client = "";
            int index = 0;

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string[] parts = fileName.Split('-'); //split the name and get the vehicle name (first part a.k.a. index 0)

                string line;
                using (var reader = new StreamReader(files[i])) //find the brand to be included in the display
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Name:"))
                        {
                            string[] clientParts = line.Split(": ");
                            client = clientParts[1];
                            if (client == username) //car is owned by user a.k.a. me
                            {
                                index = i;
                                break;
                            }
                        }
                    }
                }
            }
            // Read all lines from the file
            string[] lines = File.ReadAllLines(files[index]);

            // Find the index of the line containing "Details of the client"
            int detailsIndex = Array.IndexOf(lines, "Receipt");

            // Create a new array to hold the lines to keep
            string[] newLines = new string[detailsIndex];

            // Copy lines up to and including "Details of the client"
            Array.Copy(lines, newLines, detailsIndex);

            // Write the updated lines back to the file
            File.WriteAllLines(files[index], newLines);

            File.Move(files[index], BaseDirectory + $"\\VehicleData\\{Path.GetFileName(files[index])}");
        }
    }
}