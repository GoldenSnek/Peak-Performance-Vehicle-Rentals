using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

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
            try
            {
                BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Peak Performance Vehicle Rentals");
                Directory.CreateDirectory(BaseDirectory);
                Directory.CreateDirectory(BaseDirectory + "\\UserData");
                Directory.CreateDirectory(BaseDirectory + "\\VehicleData");
                Directory.CreateDirectory(BaseDirectory + "\\RentalData\\PendingRental");
                Directory.CreateDirectory(BaseDirectory + "\\RentalData\\ApprovedRental");
                if (!File.Exists(BaseDirectory + "\\Users.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(BaseDirectory + "\\Users.txt")) {
                        writer.WriteLine("ADMIN,01312005,Admin"); //create admin account
                    }
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        internal string GetUserFilePath(string username) //METHOD for accessing user filepath
        {
            return BaseDirectory + $"\\UserData\\{username}.txt";
        }
    }

    internal class UserFile : FilePathManager, IUserFileManagement
    {
        public void CreateUserFile(string username) //METHOD 1: for creating user file
        {
            string filePath = GetUserFilePath(username);

            //get current date and time for account creation
            string accountCreationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string[] creation = accountCreationDate.Split(' ');

            try {
                if (!File.Exists(filePath))
                {
                    using (var writer = new StreamWriter(filePath)) //create the file and write the username and password
                    {
                        writer.WriteLine($"Username: {username}");
                        writer.WriteLine($"Email Address: -no data-");
                        writer.WriteLine($"Contact Number: -no data-");
                        writer.WriteLine($"Date of Birth (MM/DD/YYYY): -no data-");
                        writer.WriteLine($"Home Address: -no data-");
                        writer.WriteLine($"Account creation date: {creation[0]}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        public void UpdateUserFile(string username, string detailchoice, string newdetail) //METHOD 2: for updating the details of the user
        {
            string filePath = GetUserFilePath(username);
            string tempPath = BaseDirectory + "\\Temp.txt";

            try {
                using (StreamReader reader = new StreamReader(filePath))
                using (StreamWriter writer = new StreamWriter(tempPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        //check if the current line starts with the search line
                        if (line.StartsWith(detailchoice))
                            writer.WriteLine($"{detailchoice}: {newdetail}"); //write the new line to the temp file
                        else
                            writer.WriteLine(line); //write the original line to the temp file
                    }
                }
                //replace the original file with the updated temp file
                File.Delete(filePath); //delete the original file
                File.Move(tempPath, filePath);
                File.Delete(tempPath);
                UserInterface.WriteColoredText(3, 1, "green", "User details has been successfuly updated!");
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        public int DeleteUserFile(string username, FilePathManager file) //METHOD 3: for deleting user file + vehicle files linked with the user
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\VehicleData", $"*{username}.txt");
            string filePath = BaseDirectory + "\\Users.txt";
            string tempPath = BaseDirectory + "\\Temp.txt";

            try {

                //initial check kung naay vehicle in the renting process from the side of the owner, or the client
                Inventory inventory = new Inventory();
                //owner
                string[] pending = inventory.ViewPendingRentalOwner(username, file);
                string[] approved = inventory.ViewApprovedRental(username, file);
                //client
                string currentPending = inventory.ViewPendingRentalClient(username, file);
                string currentApproved = inventory.ViewCurrentRental(username, file);

                if (pending.Length <= 1 && approved.Length <= 1 && currentPending == "" && currentApproved == "")
                {
                    //delete the user line from the main user file
                    using (StreamReader reader = new StreamReader(filePath))
                    using (StreamWriter writer = new StreamWriter(tempPath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            //check if the current line starts with the search line
                            if (!line.StartsWith(username))
                                writer.WriteLine(line); //write the original line to the temp file
                        }
                    }

                    //replace the original file with the updated temp file
                    File.Delete(filePath); //delete the original file
                    File.Move(tempPath, filePath);
                    File.Delete(tempPath);
                    //delete the actual user file and details
                    if (File.Exists(GetUserFilePath(username)))
                        File.Delete(GetUserFilePath(username));
                    //delete all the vehicles of the user
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (File.Exists(files[i]))
                            File.Delete(files[i]);
                    }
                    UserInterface.WriteColoredText(3, 0, "green", "User Account has been successfully deleted!");
                    return 1;
                }
                else
                {
                    UserInterface.WriteColoredText(3, 0, "red", "Make sure that there is no ongoing rental process before deleting an account!");
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
                return 0;
            }
        }
        public void DisplayUserFile(string type, string username) //METHOD 4: for displaying info inside the user file
        {
            string file = BaseDirectory + $"\\UserData\\{username}.txt";
            string content = File.ReadAllText(file);
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
    }

    internal class VehicleFile : FilePathManager, IVehicleFileManagement
    {
        public void CreateVehicleFile(string username, string[] details) //METHOD 1: creating vehicle file
        {
            string vehicleFilePath = BaseDirectory + $"\\VehicleData\\{details[2]}-{details[0]}-{username}.txt";
            string pendingFilePath = BaseDirectory + $"\\RentalData\\PendingRental\\{details[2]}-{details[0]}-{username}.txt";
            string approvedFilePath = BaseDirectory + $"\\RentalData\\ApprovedRental\\{details[2]}-{details[0]}-{username}.txt";
            string[] type = details[0].Split("-");
            
            try
            {
                if (!File.Exists(vehicleFilePath) && !File.Exists(pendingFilePath) && !File.Exists(approvedFilePath))
                {
                    string accountCreationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    string[] creation = accountCreationDate.Split(' ');

                    // Create the file and write the vehicle name
                    using (var writer = new StreamWriter(vehicleFilePath))
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
                        writer.WriteLine($"Vehicle uploaded to system on: {creation[0]}");
                    }
                    UserInterface.WriteColoredText(3, 1, "green", "New vehicle has been added!");
                }
                else
                {
                    UserInterface.CenterTextMargin(3, 1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You already have a vehicle with the same model and type in the system!"); Thread.Sleep(1500);
                    UserInterface.WaitForKey(3, 0, "Press any key to return to Manage Vehicles Menu");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        public void UpdateVehicleFile(string username, int choice, string detailchoice, string newdetail) //METHOD 2: for updating the details of the vehicle
        {
            string[] files = Directory.GetFiles(BaseDirectory + $"\\VehicleData", $"*{username}.txt");
            string tempPath = BaseDirectory + "\\Temp.txt";

            try
            {
                using (StreamReader reader = new StreamReader(files[choice]))
                using (StreamWriter writer = new StreamWriter(tempPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        //check if the current line starts with the search line
                        if (line.StartsWith(detailchoice))
                            writer.WriteLine($"{detailchoice}: {newdetail}"); //write the new line to the temp file
                        else
                            writer.WriteLine(line); //write the original line to the temp file
                    }
                }
                //replace the original file with the updated temp file
                File.Delete(files[choice]); //delete the original file
                File.Move(tempPath, files[choice]);
                File.Delete(tempPath);

                UserInterface.WriteColoredText(3, 1, "green", "Vehicle details has been successfuly updated!");
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        public void DeleteVehicleFile(string username, int choice, string type, FilePathManager file) //METHOD 3: for deleting vehicle file
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\VehicleData", $"*{username}.txt");
            string[] adminFiles = Directory.GetFiles(BaseDirectory + "\\VehicleData", $"*.txt");

            if (type == "Admin")
            {
                try
                {
                    for (int i = 0; i < adminFiles.Length; i++) //delete the file
                    {
                        if (choice == i)
                            if (File.Exists(adminFiles[i]))
                            {
                                File.Delete(adminFiles[i]);
                                UserInterface.WriteColoredText(3, 0, "green", "Vehicle has been successfully deleted from the inventory!");
                                break;
                            }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
                }
            }
            else
            {
                try
                {
                    for (int i = 0; i < files.Length; i++) //delete the file
                    {
                        if (choice == i)
                            if (File.Exists(files[i]))
                            {
                                File.Delete(files[i]);
                                UserInterface.WriteColoredText(3, 0, "green", "Vehicle has been successfully deleted from the inventory!");
                                break;
                            }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
                }
            }
        }
        public string[] DisplayVehicleFile(int DVchoice, string search, string type, FilePathManager file) //METHOD 4: for displaying info inside the vehicle file
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\VehicleData", $"*.txt");
            Inventory inventory = new Inventory();
            string[] searchFiles = inventory.ViewSearchedVehicles(search, "path", file);
            string[] approvedFiles = Directory.GetFiles(BaseDirectory + "\\RentalData\\ApprovedRental", $"*.txt");
            string[] vehicleRentDetails = new string[3]; //0 is daily, 1 is hourly, 2 is owner

            try
            {
                if (type == "search")
                {
                    for (int i = 0; i < searchFiles.Length; i++)
                    {
                        if (DVchoice == i)
                        {
                            DisplayDetails(BaseDirectory + $"\\VehicleData\\{searchFiles[i]}", vehicleRentDetails);

                            string content = File.ReadAllText(BaseDirectory + $"\\VehicleData\\{searchFiles[i]}");
                            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                            for (int j = 0; j < lines.Length; j++)
                            {
                                UserInterface.CenterTextMargin(3, 0);
                                Console.WriteLine(lines[j]);
                            }

                            //also display the user details
                            string fileName = Path.GetFileNameWithoutExtension(BaseDirectory + $"\\VehicleData\\{searchFiles[i]}");
                            string[] username = fileName.Split("-");
                            UserFile owner = new UserFile();
                            owner.DisplayUserFile("vehicle", username[3]);
                            vehicleRentDetails[2] = username[3];
                        }
                    }
                }
                else if (type == "all")
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (DVchoice == i)
                        {
                            DisplayDetails(files[i], vehicleRentDetails);

                            string content = File.ReadAllText(files[i]);
                            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
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
                }
                else if (type == "approved")
                {
                    for (int i = 0; i < approvedFiles.Length; i++)
                    {
                        if (DVchoice == i)
                        {
                            DisplayDetails(approvedFiles[i], vehicleRentDetails);

                            string content = File.ReadAllText(approvedFiles[i]);
                            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                            for (int j = 0; j < 14; j++)
                            {
                                UserInterface.CenterTextMargin(3, 0);
                                Console.WriteLine(lines[j]);
                            }

                            //also display the client details
                            UserInterface.CenterTextMargin(3, 1);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Client Details");
                            Console.ResetColor();
                            for (int j = 25; j < 30; j++)
                            {
                                UserInterface.CenterTextMargin(3, 0);
                                Console.WriteLine(lines[j]);
                            }
                            Console.WriteLine();
                            UserInterface.WaitForKey(3, 0, "Press any key to go back to Rental Details Menu.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
            return vehicleRentDetails;
        }
        private void DisplayDetails(string file, string[] vehicleRentDetails) //SUPPORTING METHOD for DisplayVehicleFile
        {
            Console.Clear();
            string[] details = new string[2];

            using (StreamReader reader = new StreamReader(file))
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
            string Prompt = @$"
                            ___  ____ ___ ____ _ _    ____ 
                            |  \ |___  |  |__| | |    [___
                            |__/ |___  |  |  | | |___ ___]

                              {details[0]} {details[1]}
                                                           ";

            UserInterface.CenterVerbatimText(Prompt);

            UserInterface.CenterTextMargin(3, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Vehicle Details");
            Console.ResetColor();
        }
        public void TransferPendingFile(int choice, string[] rentDetails, string username, string search, string type, FilePathManager file) //METHOD 5: transfer to pending
        {
            string[] parts = search.Split(" ");
            string[] files = Directory.GetFiles(BaseDirectory + "\\VehicleData", $"*.txt");
            Inventory inventory = new Inventory();
            string[] searchFiles = inventory.ViewSearchedVehicles(search, "path", file);

            //find email for contact
            string[] userFile = Directory.GetFiles(BaseDirectory + "\\UserData", $"{username}.txt");
            string[] userDetails = new string[2];
            using (StreamReader reader = new StreamReader(userFile[0])) // 'true' for append mode
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    //pa chuy chuy
                    if (line.StartsWith("Email Address"))
                    {
                        string[] lineParts = line.Split(": ");
                        userDetails[0] = lineParts[1];
                    }
                    if (line.StartsWith("Contact Number"))
                    {
                        string[] lineParts = line.Split(": ");
                        userDetails[1] = lineParts[1];
                    }
                }
            }
            try
            {
                if (type == "search")
                {
                    for (int i = 0; i < searchFiles.Length; i++)
                    {
                        if (choice == i)
                        {
                            //append
                            using (StreamWriter writer = new StreamWriter(BaseDirectory + $"\\VehicleData\\{searchFiles[i]}", true)) // 'true' for append mode
                            {
                                writer.WriteLine("Details of the client");
                                writer.WriteLine($"Name: {username}");
                                writer.WriteLine($"Email Address: {userDetails[0]}");
                                writer.WriteLine($"Contact Number: {userDetails[1]}");
                                writer.WriteLine($"Number of days/hours the vehicle will be rented: {rentDetails[0]}");
                                writer.WriteLine($"Total price: {rentDetails[1]}");
                                writer.WriteLine($"Additional information: {rentDetails[2]}");
                            }

                            File.Move(BaseDirectory + $"\\VehicleData\\{searchFiles[i]}", BaseDirectory + $"\\RentalData\\PendingRental\\{Path.GetFileName(BaseDirectory + $"\\VehicleData\\{searchFiles[i]}")}");
                            UserInterface.WriteColoredText(3, 1, "green", "Please wait patiently for the owners approval.");
                        }
                    }
                }
                else if (type == "all")
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (choice == i)
                        {
                            //append
                            using (StreamWriter writer = new StreamWriter(files[i], true)) // 'true' for append mode
                            {
                                writer.WriteLine("Details of the client");
                                writer.WriteLine($"Name: {username}");
                                writer.WriteLine($"Email Address: {userDetails[0]}");
                                writer.WriteLine($"Contact Number: {userDetails[1]}");
                                writer.WriteLine($"Number of days/hours the vehicle will be rented: {rentDetails[0]}");
                                writer.WriteLine($"Total price: {rentDetails[1]}");
                                writer.WriteLine($"Additional information: {rentDetails[2]}");
                            }

                            File.Move(files[i], BaseDirectory + $"\\RentalData\\PendingRental\\{Path.GetFileName(files[i])}");
                            UserInterface.WriteColoredText(3, 1, "green", "Please wait patiently for the owners approval.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        public void TransferApprovedFile(int choice, string username, FilePathManager file) //METHOD 6: transfer approved files
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\RentalData\\PendingRental", $"*{username}.txt");

            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (i == choice)
                    {
                        //create a receipt
                        string[] lines = File.ReadAllLines(files[i]);

                        //extract info
                        string owner = Inventory.ExtractValue(lines, "Owner:");
                        string vehicleType = Inventory.ExtractValue(lines, "Vehicle Type:");
                        string brand = Inventory.ExtractValue(lines, "Brand:");
                        string model = Inventory.ExtractValue(lines, "Model:");
                        string year = Inventory.ExtractValue(lines, "Manufacture Year:");
                        string licensePlate = Inventory.ExtractValue(lines, "License Plate:");
                        string color = Inventory.ExtractValue(lines, "Color:"); //
                        string fuelType = Inventory.ExtractValue(lines, "Fuel Type:"); //
                        string seatingCapacity = Inventory.ExtractValue(lines, "Seating Capacity:"); //
                        string mileage = Inventory.ExtractValue(lines, "Mileage:"); //
                        string location = Inventory.ExtractValue(lines, "Pickup and Return Location:");
                        string dailyPrice = Inventory.ExtractValue(lines, "Daily Rental Price:"); //
                        string hourlyPrice = Inventory.ExtractValue(lines, "Hourly Rental Price:"); //
                        string clientName = Inventory.ExtractValue(lines, "Name:");
                        string email = Inventory.ExtractValue(lines, "Email Address:");
                        string contact = Inventory.ExtractValue(lines, "Contact Number:");
                        string rentalDuration = Inventory.ExtractValue(lines, "Number of days/hours the vehicle will be rented:");
                        string totalPrice = Inventory.ExtractValue(lines, "Total price:");
                        string additionalInfo = Inventory.ExtractValue(lines, "Additional information:"); //

                        //delete details of the client
                        int detailsIndex = Array.IndexOf(lines, "Details of the client");
                        string[] newLines = new string[detailsIndex];
                        Array.Copy(lines, newLines, detailsIndex);
                        File.WriteAllLines(files[i], newLines);

                        //append
                        using (StreamWriter writer = new StreamWriter(files[i], true)) //write a receipt
                        {
                            writer.WriteLine("Receipt");
                            writer.WriteLine("  PEAK PERFORMANCE | VEHICLE RENTALS   ");
                            writer.WriteLine("         \"Rent, Ride, Repeat\"        ");
                            writer.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                            writer.WriteLine("          Vehicle Information          ");
                            writer.WriteLine($"Vehicle Owner: {owner}");
                            writer.WriteLine($"Vehicle Name: {brand} {model} {year}");
                            writer.WriteLine($"Type: {vehicleType}");
                            writer.WriteLine($"License Plate: {licensePlate}");
                            writer.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                            writer.WriteLine("            Client Information         ");
                            writer.WriteLine($"Name: {clientName}");
                            writer.WriteLine($"Email Address: {email}");
                            writer.WriteLine($"Contact Number: {contact}");
                            writer.WriteLine($"Rental Duration: {rentalDuration}");
                            writer.WriteLine($"Total Price: {totalPrice}");
                            writer.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                            writer.WriteLine("Pickup and Return Location:");
                            writer.WriteLine($"{location}");
                            writer.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                            writer.WriteLine("   Show the receipt when you pickup    ");
                            writer.WriteLine("  or return the vehicle to the owner   ");
                        }

                        File.Move(files[i], BaseDirectory + $"\\RentalData\\ApprovedRental\\{Path.GetFileName(files[i])}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        public void TransferNonApprovedFile(int choice, string username, FilePathManager file) //METHOD 7: transfer non-approved files
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\RentalData\\PendingRental", $"*{username}.txt");

            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    if (i == choice)
                    {
                        string[] lines = File.ReadAllLines(files[i]);
                        int detailsIndex = Array.IndexOf(lines, "Details of the client");
                        string[] newLines = new string[detailsIndex];

                        Array.Copy(lines, newLines, detailsIndex);
                        File.WriteAllLines(files[i], newLines);
                        File.Move(files[i], BaseDirectory + $"\\VehicleData\\{Path.GetFileName(files[i])}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        public void DisplayPendingFile(int choice, string username, string type, FilePathManager file) //METHOD 8: display pending files
        {
            string[] files = Directory.GetFiles(BaseDirectory + "\\RentalData\\PendingRental", $"*{username}.txt");
            string[] filesClient = Directory.GetFiles(BaseDirectory + "\\RentalData\\PendingRental", $"*.txt");
            string[] lines = new string[20];

            if (type == "Vehicle Provider")
            {
                try
                {
                    // Read all lines from the file
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (i == choice)
                            lines = File.ReadAllLines(files[i]);
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
                catch (Exception e)
                {
                    Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
                }
            }
            else if (type == "Client")
            {
                try
                {
                    // Read all lines from the file
                    for (int i = 0; i < filesClient.Length; i++)
                    {
                        string line;
                        using (var reader = new StreamReader(filesClient[i]))
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.StartsWith("Name:"))
                                {
                                    string[] nameParts = line.Split(": ");
                                    string name = nameParts[1];
                                    if (name == username)
                                    {
                                        lines = File.ReadAllLines(filesClient[i]);
                                        Console.Clear();

                                        string content = File.ReadAllText(filesClient[i]);
                                        string[] displayLines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                                        string[] details = new string[2];

                                        string line2;
                                        using (var lineReader = new StreamReader(filesClient[i]))
                                        {
                                            while ((line2 = lineReader.ReadLine()) != null)
                                            {
                                                //pa chuy chuy
                                                if (line2.StartsWith("Brand"))
                                                {
                                                    string[] lineParts = line2.Split(": ");
                                                    details[0] = lineParts[1];
                                                }
                                                if (line2.StartsWith("Model"))
                                                {
                                                    string[] lineParts = line2.Split(": ");
                                                    details[1] = lineParts[1];
                                                }
                                            }
                                        }

                                        string Prompt = @$"
                                       ___  ____ ___ ____ _ _    ____ 
                                       |  \ |___  |  |__| | |    [___
                                       |__/ |___  |  |  | | |___ ___]

                                       {details[0]} {details[1]}
                                                                ";

                                        UserInterface.CenterVerbatimText(Prompt);

                                        UserInterface.CenterTextMargin(3, 0);
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("Vehicle Details");
                                        Console.ResetColor();
                                        for (int j = 0; j < displayLines.Length; j++)
                                        {
                                            UserInterface.CenterTextMargin(3, 0);
                                            Console.WriteLine(displayLines[j]);
                                        }

                                        //also display the owner details
                                        string fileName = Path.GetFileNameWithoutExtension(filesClient[i]);
                                        string[] ownerName = fileName.Split("-");
                                        UserFile owner = new UserFile();
                                        owner.DisplayUserFile("vehicle", ownerName[3]);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //find the index of the line containing "Details of the client"
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
                catch (Exception e)
                {
                    Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
                }
            }
        }
        public void DisplayReceiptFile(string username, FilePathManager file) //METHOD 9: display receipt
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\ApprovedRental", $"*.txt");
            string client;

            try
            {
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
                                if (client == username) //car is rented by user a.k.a. me
                                {
                                    string[] lines = new string[20];

                                    lines = File.ReadAllLines(files[i]);

                                    //find the index of the line containing "Details of the client"
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
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
        public void TransferFinishRentFile(string username, FilePathManager file) //METHOD 10: finish rental
        {
            string[] files = Directory.GetFiles(file.BaseDirectory + "\\RentalData\\ApprovedRental", $"*.txt");
            string client;
            int index = 0;

            try
            {
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
                string[] lines = File.ReadAllLines(files[index]);
                int detailsIndex = Array.IndexOf(lines, "Receipt");
                string[] newLines = new string[detailsIndex];

                Array.Copy(lines, newLines, detailsIndex);
                File.WriteAllLines(files[index], newLines);
                File.Move(files[index], BaseDirectory + $"\\VehicleData\\{Path.GetFileName(files[index])}");
            }
            catch (Exception e)
            {
                Console.WriteLine("There is an unexpected error in the program. Please try again.\nError message: " + e);
            }
        }
    }
}