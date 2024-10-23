﻿using System;
using System.IO;
using System.Threading;

//Notes for self:
//Date Started: 10/18/24
//1. User Data file path may vary depending on device used (fixed 10/19/24)
//2. Started creating main menu 10/20/24
//3. Viewing and adding cars (unifnished)
//4. unya raman unta ang pachuychuy pero na inspire ko ni noah so ako gi una HAHAHAH (incomplete pa 10/23/24)

//To-Do List:
//1. Program Proper, after maka login
//2. passwords should be hidden or protected sa txt file.
//3. Main menu should have an option to go back to login ang register screen
//4. passwords should be ******


namespace Peak_Performance_Vehicle_Rentals
{
    internal class Program
    {
        static void Main(string[] args)
        {
            do //entire program
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
                        case 0:
                            username = Login.UserLogin(file);
                            if (username != "")
                            {
                                Console.WriteLine("Login Successful!\n");
                                Thread.Sleep(1000);
                                LRrunning = false;
                            }
                            else
                            {
                                Console.WriteLine("Invalid credentials!\n");
                                Thread.Sleep(1000);
                                LRrunning = true;
                            }
                            break;

                        case 1:
                            Register.UserRegister(file);
                            break;

                        case 2:
                            Console.WriteLine("This is my Final Project for CPE261!");
                            Console.WriteLine("Program created by: John Michael A. Nave");
                            UserInterface.WaitForKeyLR();
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
                        int MMchoice = Choice.MainMenuChoice();
                        switch (MMchoice)
                        {
                            case 0:
                                Console.WriteLine("");
                                ViewVehicles.ViewRentalVehicles(file);
                                break;

                            case 1:
                                Console.WriteLine("");
                                break;

                            case 2:
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
                                            ManageVehicles.DeleteVehicle(username, file);
                                            break;

                                        case 0:
                                            MVrunning = false;
                                            break;

                                        default:
                                            break;
                                    }
                                } while (MVrunning);

                                break;

                            case 3:
                                Console.WriteLine("help");
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