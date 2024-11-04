using System;
using System.Drawing;
using System.IO;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;

//To-Do List:
//1.dili enter number, but rather arrow keys ra [optional] (done 10/23/24)
//2. idk if bug or feature, but when u press arrow keys, mo gawas ang previous input like username

namespace Peak_Performance_Vehicle_Rentals
{
    internal class LoginRegister : LoginRegisterBase, ILoginRegister
    {
        public string UserLogin(FilePathManager file) //MAIN METHOD for login
        {
            Console.CursorVisible = true;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter username: ");
                Username = Console.ReadLine();
                if (Username.Length >= 20)
                {
                    UserInterface.WriteColoredText(3, 0, "red", "Username is too long. Keep it below 20 characters");
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                }
                if (Username == "")
                    UserInterface.WriteColoredText(3, 0, "red", "Please do not leave the username empty");
            } while (Username == "" || Username.Length >= 20);
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter password: ");
                Password = ReadPassword();
                if (Password == "")
                    UserInterface.WriteColoredText(3, 1, "red", "Please do not leave the password empty");
            } while (Password == "");

            //check if the credentials are valid
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
            Console.CursorVisible = false;
            if (isValidUser)
                return Username;
            else
                return "";
        }

        public void UserRegister(FilePathManager file) //MAIN METHOD for register
        {
            Console.CursorVisible = true;
            bool DuplicateUser = true;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter username: ");
                Username = Console.ReadLine();
                DuplicateUser = UserExists(Username, file);
                if (DuplicateUser)
                {
                    UserInterface.WriteColoredText(3, 0, "red", "Username already exists. Please choose a different one.");
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                }
                if (Username.Length >= 20)
                {

                    UserInterface.WriteColoredText(3, 0, "red", "Username is too long. Keep it below 20 characters");
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                }
                if (Username == "")
                    UserInterface.WriteColoredText(3, 0, "red", "Please do not leave the username empty");
            } while (DuplicateUser || Username == "" || Username.Length >= 20);

            string password;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter password: ");
                password = ReadPassword();
                if (password == "")
                    UserInterface.WriteColoredText(3, 1, "red", "Please do not leave the password empty");
            } while (password == "");

            using (StreamWriter writer = new StreamWriter(file.BaseDirectory + "\\Users.txt", true)) //save username and password to the user text file
            {
                writer.WriteLine($"{Username},{password}");
            }

            UserFile user = new UserFile();
            user.CreateUserFile(Username); //create an individual user file

            UserInterface.WriteColoredText(3, 2, "green", "Registration Successful");
        }
        internal static bool UserExists(string Username, FilePathManager file) //SUPPORTING METHOD for UserRegister, check for duplicate user
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
        internal static string ReadPassword() //SUPPORTING METHOD, hide password with asterisks
        {
            string password = "";
            while (true)
            {
                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password[0..^1];
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }
                else if (char.IsLetterOrDigit(keyInfo.KeyChar))
                {
                    if (password.Length < 20) // maximum password is 20 characters
                    {
                        password += keyInfo.KeyChar;
                        Console.Write("*");
                    }
                }
            }
            return password;
        }
    }
}