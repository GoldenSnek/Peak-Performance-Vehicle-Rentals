using System;
using System.IO;

//Notes for self:
//Date Started: 10/18/24
//1. User Data file path may vary depending on device used (fixed 10/19/24)
//2. Started creating main menu 10/20/24
//3. Viewing and adding cars (unifnished)

//To-Do List:
//1. Program Proper, after maka login
//2. passwords should be hidden or protected sa txt file.
//3. Main menu should have an option to go back to login ang register screen


namespace Peak_Performance_Vehicle_Rentals
{
    internal class Program
    {
        static void Main(string[] args)
        {
            do
            {
                //variable declarations
                FilePathManager file = new FilePathManager();
                string username = "";

                //Login Register
                bool LRrunning = true; //LR = LoginRegister
                do
                {
                    int LRchoice = Choice.LoginRegisterChoice();
                    switch (LRchoice)
                    {
                        case 1:
                            username = Login.UserLogin(file);
                            if (username != "")
                            {
                                Console.WriteLine("Login Successful!\n");
                                LRrunning = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid credentials!\n");
                                LRrunning = true;
                            }
                            break;

                        case 2:
                            Register.UserRegister(file);
                            break;

                        default:
                            break;
                    }
                } while (LRrunning);

                //Main Menu
                if (username != "")
                {
                    Console.WriteLine($"Welcome user: {username}");
                    bool MMrunning = true;
                    do
                    {
                        int MMchoice = Choice.MainMenuChoice();
                        switch (MMchoice)
                        {
                            case 1:
                                Console.WriteLine("");
                                ViewVehicles.ViewRentalVehicles();
                                break;

                            case 2:
                                Console.WriteLine("");
                                break;

                            case 3:
                                Console.WriteLine("");
                                bool MVrunning = true;
                                do
                                {
                                    int MVchoice = Choice.ManageVehiclesChoice();
                                    switch (MVchoice)
                                    {
                                        case 1:
                                            ManageVehicles.AddVehicle(username);
                                            break;

                                        case 2:
                                            ManageVehicles.UpdateVehicle(username);
                                            break;

                                        case 3:
                                            ManageVehicles.DeleteVehicle(username);
                                            break;

                                        case 0:
                                            MVrunning = false;
                                            break;

                                        default:
                                            break;
                                    }
                                } while (MVrunning);

                                break;

                            case 4:
                                Console.WriteLine("");
                                break;

                            case 0:
                                MMrunning = false;
                                break;
                            default:
                                break;
                        }
                    } while (MMrunning);
                }
            } while (true);
        }
    }
}