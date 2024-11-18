using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

//VERSION 1

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
//1. Receipt
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
            FilePathManager file = new FilePathManager();
            Console.CursorVisible = false;
            
            do //entire program
            {
                //variable declarations
                Choice choice = new Choice();
                string[] details = {"", ""};

                //Login Register
                bool LRrunning = true;
                LoginRegister LR = new LoginRegister();  
                do
                {
                    int LRchoice = choice.LoginRegisterChoice();
                    switch (LRchoice)
                    {
                        case 0:
                            details = LR.UserLogin(file);
                            if (details[0] != "")
                            {
                                UserInterface.WriteColoredText(3, 2, "green", "Login Successful!");
                                LRrunning = false;
                                Console.Clear();
                                continue; //proceed to main menu
                            }
                            UserInterface.WriteColoredText(3, 2, "red", "Invalid Credentials!");
                            Console.Clear();
                            break;

                        case 1:
                            LR.UserRegister(file);
                            Console.Clear();
                            break;

                        case 2:
                            Console.Clear();
                            UserInterface.CenterVerbatimText(@$"
                                                            ┌─┐┌┐ ┌─┐┬ ┬┌┬┐  ┌┬┐┬ ┬┌─┐  ┌─┐┬─┐┌─┐┌─┐┬─┐┌─┐┌┬┐
                                                            ├─┤├┴┐│ ││ │ │    │ ├─┤├┤   ├─┘├┬┘│ ││ ┬├┬┘├─┤│││
                                                            ┴ ┴└─┘└─┘└─┘ ┴    ┴ ┴ ┴└─┘  ┴  ┴└─└─┘└─┘┴└─┴ ┴┴ ┴

                                                            This is my Final Project for CPE261 (OOP 1) - H2!
                                                            Program created by: John Michael A. Nave
                                                            Version 1 (November 19, 2024)

                                                            Special Thanks:
                                                            Mga barkada sa CPE
                                                            Online Sources (YouTube, GitHub, Stack Overflow, etc.)
                                                            Engr. Julian N. Semblante

                                                            Links:
                                                            Text Art: https://patorjk.com/software/taag/#p=display&f=Graffiti&t=
                                                            Car Art: https://www.asciiart.eu/vehicles/cars
                                                            ");
                            UserInterface.CenterVerbatimText("Press any key to return to the Login and Register screen");
                            UserInterface.WaitForKey(0, 0, "");
                            break;

                        case 3:
                            ExitProgram();
                            break;
                    }
                } while (LRrunning);

                //Main Menu
                int MMchoice;
                do
                {
                    VehicleManager MV = new VehicleManager();
                    UserManager UR = new UserManager();
                    MMchoice = choice.MainMenuChoice(details[0], details[1]);
                    switch (MMchoice)
                    {
                        //client and vehicle provider cases
                        case 0:
                            int Rchoice;
                            do
                            {
                                Rchoice = choice.RentalChoice(file);
                                switch (Rchoice)
                                {
                                    case 0:
                                        MV.SearchRentalVehicles(details[0], details[1], file);
                                        break;

                                    case 1:
                                        MV.ViewRentalVehicles(details[0], details[1], file);
                                        break;
                                }
                            } while (Rchoice != 2);
                            break;
                                
                        case 1:
                            int VRchoice;
                            do
                            {
                                VRchoice = choice.RentalDetailsChoice(details[0], details[1], file);
                                switch (VRchoice)
                                {
                                    case 0:
                                        MV.PendingVehiclesClient(details[0], file);
                                        break;

                                    case 1:
                                        MV.PendingVehiclesOwner(details[0], file);
                                        break;

                                    case 2:
                                        MV.ApprovedVehicles(details[0], file);
                                        break;

                                    case 3:
                                        MV.CurrentlyRentingVehicle(details[0], file);
                                        break;
                                }
                            } while (VRchoice !=4);
                            break;

                        case 2:
                            int MVchoice;
                            do
                            {
                                MVchoice = choice.ManageVehiclesChoice();
                                switch (MVchoice)
                                {
                                    case 0:
                                        MV.AddVehicle(details[0]);
                                        break;

                                    case 1:
                                        MV.UpdateVehicle(details[0], file);
                                        break;

                                    case 2:
                                        MV.DeleteVehicle(details[0], file);
                                        break;
                                }
                            } while (MVchoice != 3);
                            break;

                        case 3:
                            int MUchoice;
                            do
                            {
                                MUchoice = choice.ManageUserChoice();
                                switch (MUchoice)
                                {
                                    case 0:
                                        UR.ViewUserDetails(details[0], file);
                                        break;

                                    case 1:
                                        UR.UpdateUser(details[0], file);
                                        break;

                                    case 2:
                                        MUchoice = UR.DeleteUser(details[0], details[1], file);
                                        if (MUchoice == 3)
                                        {
                                            UserInterface.WriteColoredText(3, 1, "green", "Returning to login screen...");
                                            MMchoice = 4;
                                        }
                                        break;
                                }
                            } while (MUchoice != 3);
                            break;

                        //case 4 is basically log out
                        case 5:
                            ExitProgram();
                            break;

                        //admin cases
                        case 6:
                            MV.ViewRentalVehicles(details[0], details[1], file);
                            break;

                        case 7:
                            UR.DeleteUser(details[0], details[1], file);
                            break;
                    }
                } while (MMchoice != 4);
            } while (true);
        }
        static void ExitProgram()
        {
            Console.Clear();
            string text = @"
                            ╔═╗┌─┐┌─┐  ┬ ┬┌─┐┬ ┬  ┌─┐┌─┐┌─┐┬┌┐┌
                            ╚═╗├┤ ├┤   └┬┘│ ││ │  ├─┤│ ┬├─┤││││
                            ╚═╝└─┘└─┘   ┴ └─┘└─┘  ┴ ┴└─┘┴ ┴┴┘└┘

              -                            _____       _____                         -
              -               .........   {     }     {     }                        -
              -              (>>\zzzzzz [======================]                     -
              -              ( <<<\lllll_\\ _        _____    \\                     -
              -             _,`-,\<   __#:\\::_    __#:::_:__  \\                    -
              -            /    . `--,#::::\\:::___#::::/__+_\ _\\                   -
              -           /  _  .`-    `--,/_~~~~~~~~~~~~~~~~~~~~  -,_               -
              -          :,// \ .         .  '--,____________   ______`-,            -
              -          :: o |.         .  ___ \_____||____\+/     ||~ \            -
              -           :;   ;-,_       . ,' _`,~~~~~~~~~~~~~~~~~~~~~~~~\          -
              -           \ \_/ _ :`-,_   . ; / \\ ====================== /          -
              -            \__/~ /     `-,.; ; o |\___[~~~]________[~~~]__:          -
              -               ~~~          ; :   ;~ ;  ~~~         ;~~~::;           -
              -                             \ \_/ ~/               ::::::;           -
              -                              \_/~~/                 \:::/            -
              -                                ~~~                   ~~~             -";
            UserInterface.CenterVerbatimText(text);
            System.Environment.Exit(0);
        }
    }
}