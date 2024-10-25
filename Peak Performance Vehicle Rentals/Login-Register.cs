using System;
using System.IO;

//To-Do List:
//1.dili enter number, but rather arrow keys ra [optional] (done 10/23/24)

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Login : Program
    {
        public static string UserLogin(FilePathManager file) //MAIN METHOD for login
        {
            string username;
            do
            {
                Console.Write("Enter username: ");
                username = Console.ReadLine();
                if (username == "")
                {
                    Console.WriteLine("Please do not leave the username empty");
                    Thread.Sleep(1000);
                }
            } while (username == "");

            string password;
            do
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine();
                if (password == "")
                {
                    Console.WriteLine("Please do not leave the password empty");
                    Thread.Sleep(1000);
                }
            } while (password == "");

            // Check if the credentials are valid
            bool isValidUser = false;
            using (StreamReader reader = new StreamReader(file.BaseDirectory + "\\Users.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(','); //parts[0] is username, parts[1] is [password]
                    if (parts[0] == username && parts[1] == password)
                    {
                        isValidUser = true;
                        break;
                    }
                }
            }
            if (isValidUser)
                return username;
            else
                return "";
        }
    }

    internal class Register : Program
    {
        public static void UserRegister(FilePathManager file) //MAIN METHOD for register
        {
            string username;
            bool DuplicateUser = true;
            do
            {
                Console.Write("Enter username: ");
                username = Console.ReadLine();
                DuplicateUser = UserExists(username, file);
                if (DuplicateUser)
                {
                    Console.WriteLine("Username already exists. Please choose a different one."); Thread.Sleep(1000);
                }
                if (username == "")
                {
                    Console.WriteLine("Please do not leave the username empty");
                    Thread.Sleep(1000);
                }
            } while (DuplicateUser || username == "");

            string password;
            do
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine();
                if (password == "")
                {
                    Console.WriteLine("Please do not leave the password empty");
                    Thread.Sleep(1000);
                }
            } while (password == "");

            //save username and password to the user text file
            using (StreamWriter writer = new StreamWriter(file.BaseDirectory + "\\Users.txt", true))
            {
                writer.WriteLine($"{username},{password}");
            }

            //create an individual user file
            UserFile user = new UserFile();
            user.CreateUserFile(username);

            Console.WriteLine("Registration successful!"); Thread.Sleep(1000);
        }

        public static bool UserExists(string username, FilePathManager file) //method to check for duplicate user
        {
            bool DuplicateUser = false;
            using (StreamReader reader = new StreamReader(file.BaseDirectory + "\\Users.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(','); //parts[0] is username
                    if (parts[0] == username)
                    {
                        DuplicateUser = true;
                        break;
                    }
                }
            }
            return DuplicateUser;
        }
    }
}
