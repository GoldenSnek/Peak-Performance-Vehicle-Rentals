using System;
using System.IO;

//To-Do List:
//1.dili enter number, but rather arrow keys ra [optional] (done 10/23/24)

namespace Peak_Performance_Vehicle_Rentals
{
    internal class LoginRegister : LoginRegisterBase, ILoginRegister
    {
        public string UserLogin(FilePathManager file) //MAIN METHOD for login
        {
            do
            {
                Console.Write("Enter username: ");
                Username = Console.ReadLine();
                if (Username == "")
                {
                    Console.WriteLine("Please do not leave the username empty");
                    Thread.Sleep(1000);
                }
            } while (Username == "");
            do
            {
                Console.Write("Enter password: ");
                Password = Console.ReadLine();
                if (Password == "")
                {
                    Console.WriteLine("Please do not leave the password empty");
                    Thread.Sleep(1000);
                }
            } while (Password == "");

            // Check if the credentials are valid
            bool isValidUser = false;
            using (StreamReader reader = new StreamReader(file.BaseDirectory + "\\Users.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(','); //parts[0] is username, parts[1] is password
                    if (parts[0] == Username && parts[1] == Password)
                    {
                        isValidUser = true;
                        break;
                    }
                }
            }
            if (isValidUser)
                return Username;
            else
                return "";
        }

        public void UserRegister(FilePathManager file) //MAIN METHOD for register
        {
            bool DuplicateUser = true;
            do
            {
                Console.Write("Enter username: ");
                Username = Console.ReadLine();
                DuplicateUser = UserExists(Username, file);
                if (DuplicateUser)
                {
                    Console.WriteLine("Username already exists. Please choose a different one."); Thread.Sleep(1000);
                }
                if (Username == "")
                {
                    Console.WriteLine("Please do not leave the username empty"); Thread.Sleep(1000);
                }
            } while (DuplicateUser || Username == "");

            string password;
            do
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine();
                if (password == "")
                {
                    Console.WriteLine("Please do not leave the password empty"); Thread.Sleep(1000);
                }
            } while (password == "");

            using (StreamWriter writer = new StreamWriter(file.BaseDirectory + "\\Users.txt", true)) //save username and password to the user text file
            {
                writer.WriteLine($"{Username},{password}");
            }

            UserFile user = new UserFile();
            user.CreateUserFile(Username); //create an individual user file

            Console.WriteLine("Registration successful!"); Thread.Sleep(1000);
        }

        public static bool UserExists(string Username, FilePathManager file) //SUPPORTING METHOD for UserRegister, check for duplicate user
        {
            bool DuplicateUser = false;
            using (StreamReader reader = new StreamReader(file.BaseDirectory + "\\Users.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(','); //parts[0] is username
                    if (parts[0] == Username)
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
