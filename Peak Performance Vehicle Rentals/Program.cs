using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                                UserInterface.WriteColoredText(3, 2, "green", "Login Successful!");
                                LRrunning = false;
                            }
                            else
                            {
                                UserInterface.WriteColoredText(3, 2, "red", "Invalid Credentials!");
                                LRrunning = true;
                            }
                            break;

                        case 1:
                            LR.UserRegister(file);
                            break;

                        case 2:
                            string text = @$"This is my Final Project for CPE261 - H2!
                                            Program created by: John Michael A. Nave

                                            Special thanks:
                                            Mga barkada sa CPE
                                            Sources (YouTube, GitHub, ChatGPT, Microsoft C# tutorial, etc.)
                                            Engr. Julian N. Semblante

                                            Links:
                                            Text Art: https://patorjk.com/software/taag/#p=display&f=Graffiti&t=
                                            Car Art: https://www.asciiart.eu/vehicles/cars
                            ";
                            UserInterface.CenterVerbatimText(text);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            UserInterface.CenterVerbatimText("Press any key to return to the Login and Register screen");
                            Console.ResetColor();
                            UserInterface.WaitForKey(0, 0, "");
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
                                MV.ViewRentalVehicles(username, file);
                                break;

                            case 1:
                                bool VRrunning = true;
                                do
                                {
                                    int VRchoice = choice.RentalDetailsChoice();
                                    switch (VRchoice)
                                    {
                                        case 0:
                                            MV.PendingVehicles(username, file);
                                            break;

                                        case 1:
                                            MV.ApprovedVehicles(username, file);
                                            break;

                                        case 2:
                                            MV.CurrentlyRentingVehicles(username, file);
                                            break;

                                        case 3:
                                            VRrunning = false;
                                            break;

                                        default:
                                            break;
                                    }
                                } while (VRrunning);

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