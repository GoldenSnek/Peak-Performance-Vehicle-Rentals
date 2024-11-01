using System;
using System.IO;

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
                if (Username == "")
                {
                    UserInterface.CenterTextMargin(3, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please do not leave the username empty"); Thread.Sleep(1000);
                    Console.ResetColor();
                    UserInterface.ClearLine();
                }
            } while (Username == "");
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter password: ");
                Password = ReadPassword();
                if (Password == "")
                {
                    UserInterface.CenterTextMargin(3, 1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please do not leave the password empty"); Thread.Sleep(1000);
                    Console.ResetColor();
                    UserInterface.ClearLine();
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
                    UserInterface.CenterTextMargin(3, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Username already exists. Please choose a different one."); Thread.Sleep(1000);
                    Console.ResetColor();
                    UserInterface.ClearLine();
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.BufferWidth));
                }
                if (Username == "")
                {
                    UserInterface.CenterTextMargin(3, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please do not leave the username empty"); Thread.Sleep(1000);
                    Console.ResetColor();
                    UserInterface.ClearLine();
                }
            } while (DuplicateUser || Username == "");

            string password;
            do
            {
                UserInterface.CenterTextMargin(3, 0);
                Console.Write("Enter password: ");
                password = ReadPassword();
                if (password == "")
                {
                    UserInterface.CenterTextMargin(3, 1);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please do not leave the password empty"); Thread.Sleep(1000);
                    Console.ResetColor();
                    UserInterface.ClearLine();
                }
            } while (password == "");

            using (StreamWriter writer = new StreamWriter(file.BaseDirectory + "\\Users.txt", true)) //save username and password to the user text file
            {
                writer.WriteLine($"{Username},{password}");
            }

            UserFile user = new UserFile();
            user.CreateUserFile(Username); //create an individual user file

            UserInterface.CenterTextMargin(3, 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = false;
            Console.WriteLine("Registration successful!"); Thread.Sleep(1000);
            Console.ResetColor();
            UserInterface.ClearLine();
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
        internal static string ReadPassword()
        {
            string password = "";

            while (true)
            {
                // Read a key without displaying it
                var keyInfo = Console.ReadKey(intercept: true);

                // Check for Enter key
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                // Check for Backspace key
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        // Remove the last character from password
                        password = password[0..^1]; // Equivalent to password.Remove(password.Length - 1)
                                                    // Move the cursor back, overwrite with space, and move back again
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }
                else if (char.IsLetterOrDigit(keyInfo.KeyChar))
                {
                    // Add the character to the password string
                    password += keyInfo.KeyChar;
                    // Display an asterisk
                    Console.Write("*");
                }
            }

            return password;
        }
    }
}
