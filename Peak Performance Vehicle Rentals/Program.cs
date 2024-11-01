using System;
using System.IO;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

//Notes for self:
//Date Started: 10/18/24
//1. User Data file path may vary depending on device used (fixed 10/19/24)
//2. Started creating main menu 10/20/24
//3. Viewing and adding cars (unifnished)
//4. unya ra ang pachuychuy
//5. Annoying bug 10/24/24
//6. Day 9 (10/26/24) - No changes has been made today

//To-Do List:
//1. Program Proper, after maka login
//2. passwords should be hidden or protected sa txt file.
//3. Main menu should have an option to go back to login ang register screen
//4. passwords should be ******

//FINISH THIS WEEK ASAP
//5. Edit User Details
//6. Delete User File (and also the vehicle files with the same username)
//7. Rent a vehicle and the vehicle rented will be removed from the "view all rental vehicles" and be moved to "rental details"

//Sir Semblante suggestions
//1. Reciept
//2. Include User details in vehicle display
//3. Improve UI
//4. Rental rates should be per hour, days, weeks
//5. Try Catch error blocks
//6. passwords are hidden ***********

namespace Peak_Performance_Vehicle_Rentals
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            do //entire program
            {
                //variable declarations
                FilePathManager file = new FilePathManager();
                Choice choice = new Choice();
                string username = "";

                //Login Register
                bool LRrunning = true;
                LoginRegister LR = new LoginRegister();  
                do
                {
                    int LRchoice = choice.LoginRegisterChoice();
                    switch (LRchoice)
                    {
                        case 0:
                            username = LR.UserLogin(file);
                            if (username != "")
                            {
                                UserInterface.CenterTextMargin(3, 2);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Login Successful!"); Thread.Sleep(1000);
                                Console.ResetColor();
                                LRrunning = false;
                            }
                            else
                            {
                                UserInterface.CenterTextMargin(3, 2);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid credentials!"); Thread.Sleep(1000);
                                Console.ResetColor();
                                LRrunning = true;
                            }
                            break;

                        case 1:
                            LR.UserRegister(file);
                            break;

                        case 2:
                            string text = @"This is my Final Project for CPE261!
                                            Program created by: John Michael A. Nave
                                            Press any key to return to the Login and Register screen";
                            UserInterface.CenterVerbatimText(text);
                            UserInterface UI = new UserInterface();
                            UI.WaitForKey();
                            break;

                        case 3:
                            Console.Clear();
                            Console.WriteLine("Thank you for using the program!");
                            Console.WriteLine("Now exiting program...");
                            System.Environment.Exit(0);
                            break;

                        default:
                            break;
                    }
                } while (LRrunning);

                //Main Menu
                if (username != "")
                {
                    bool MMrunning = true;
                    do
                    {
                        VehicleManager MV = new VehicleManager();
                        UserManager UR = new UserManager();
                        int MMchoice = choice.MainMenuChoice(username);
                        switch (MMchoice)
                        {
                            case 0:
                                MV.ViewRentalVehicles(file);
                                break;

                            case 1:
                                break;

                            case 2:
                                bool MVrunning = true;
                                do
                                {
                                    int MVchoice = choice.ManageVehiclesChoice();
                                    switch (MVchoice)
                                    {
                                        case 0:
                                            MV.AddVehicle(username);
                                            break;

                                        case 1:
                                            MV.UpdateVehicle(username, file);
                                            break;

                                        case 2:
                                            MV.DeleteVehicle(username, file);
                                            break;

                                        case 3:
                                            MVrunning = false;
                                            break;

                                        default:
                                            break;
                                    }
                                } while (MVrunning);

                                break;

                            case 3:
                                bool MUrunning = true;
                                do
                                {
                                    int MUchoice = choice.ManageUserChoice();
                                    switch (MUchoice)
                                    {
                                        case 0:
                                            UR.ViewUserDetails(username, file);
                                            break;

                                        case 1:
                                            UR.UpdateUser(username, file);
                                            break;

                                        case 2:
                                            MUrunning = UR.DeleteUser(username, file);
                                            if (MUrunning == false)
                                                MMrunning = false;
                                            break;

                                        case 3:
                                            MUrunning = false;
                                            break;

                                        default:
                                            break;
                                    }
                                } while (MUrunning);
                                break;

                            case 4:
                                MMrunning = false;
                                break;
                            case 5:
                                Console.Clear();
                                Console.WriteLine("Thank you for using the program!");
                                Console.WriteLine("Now exiting program...");
                                System.Environment.Exit(0);
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