using System;
using System.IO;

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Login : User
    {
        public static bool UserLogin() //MAIN method for login
        {
            string username;
            do
            {
                Console.Write("Enter username: ");
                username = Console.ReadLine();
                if (username == "")
                    Console.WriteLine("Please do not leave the username empty");
            } while (username == "");

            string password;
            do
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine();
                if (password == "")
                    Console.WriteLine("Please do not leave the password empty");
            } while (password == "");

            // Check if the credentials are valid
            bool isValidUser = false;
            using (StreamReader reader = new StreamReader(FilePath))
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
            return isValidUser;
        }
    }

    internal class Register : User
    {
        public static void UserRegister() //MAIN method for register
        {
            string username;
            bool DuplicateUser = true;
            do
            {
                Console.Write("Enter username: ");
                username = Console.ReadLine();
                DuplicateUser = UserExists(username);
                if (DuplicateUser)
                {
                    Console.WriteLine("Username already exists. Please choose a different one.");
                }
                if (username == "")
                    Console.WriteLine("Please do not leave the username empty");
            } while (DuplicateUser || username == "");

            string password;
            do
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine();
                if (password == "")
                    Console.WriteLine("Please do not leave the password empty");
            } while (password == "");

            // Save username and password to the text file
            using (StreamWriter writer = new StreamWriter(FilePath, true))
            {
                writer.WriteLine($"{username},{password}");
            }

            Console.WriteLine("Registration successful!");
        }

        public static bool UserExists(string username) //method to check for duplicate user
        {
            bool DuplicateUser = false;
            using (StreamReader reader = new StreamReader(FilePath))
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
