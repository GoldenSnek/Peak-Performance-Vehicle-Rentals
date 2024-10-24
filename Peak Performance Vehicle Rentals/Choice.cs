﻿using System;
using System.IO;

//To-Do List:
//1. create separate files for each user (done 10/19/24)
//2. change choices pina noah style nga mag arrow keys ka

namespace Peak_Performance_Vehicle_Rentals
{
    internal class Choice
    {
        public static int LoginRegisterChoice() //choice method 1: login and register
        {
            string prompt = "Welcome to the program";
            string[] options = {"Login", "Register", "About", "Exit"};

            UserInterface LR = new UserInterface(prompt, options);
            int choice = LR.RunUserInterface();
            return choice;
        }

        public static int MainMenuChoice() //choice method 2: main menu
        {

            string prompt = "MAIN MENU";
            string[] options = { "View vehicles", "View rental details", "Manage vehicles", "Help", "Logout", "Exit Program" };

            UserInterface MM = new UserInterface(prompt, options);
            int choice = MM.RunUserInterface();
            return choice;
        }
        public static int ViewAllVehiclesChoice(FilePathManager file) //choice method ?: view all vehicles
        {
            string prompt = "Select a vehicle that you want to view the details";
            string[] options = Inventory.ViewAllVehicles(file);

            UserInterface VAV = new UserInterface(prompt, options);
            int choice = VAV.RunUserInterface();
            return choice;
        }
        public static int ViewOwnedVehiclesChoice(string username, FilePathManager file) //choice method ?: view owned vehicles
        {
            string prompt = "Select a vehicle that you want to manage";
            string[] options = Inventory.ViewOwnedVehicles(username, file);

            UserInterface VOV = new UserInterface(prompt, options);
            int choice = VOV.RunUserInterface();
            return choice;
        }

        public static int ManageVehiclesChoice() //choice method ?: manage vehicles
        {

            string prompt = "Select a vehicle that you want to view the details";
            string[] options = { "Add your own rentable vehicle", "Update vehicles", "Delete vehicles", "Go back to main menu" };

            UserInterface MV = new UserInterface(prompt, options);
            int choice = MV.RunUserInterface();
            return choice;

        }

        public static string VehicleTypeChoice() //choice method ?: view owned vehicles
        {
            string prompt = "Choose vehicle type";
            string[] options = { "Car", "Motorcycle" };

            UserInterface VT = new UserInterface(prompt, options);
            int choiceVT = VT.RunUserInterface();

            if (choiceVT == 0)
            {
                prompt = "Choose Car type: ";
                string[] optionVTCar = { "Sedan", "SUV", "Coupe", "Convertible", "Hatchback", "Minivan", "Pickup Truck", "Limousine", "Sports Car", "Luxury Car" };

                UserInterface VTCar = new UserInterface(prompt, optionVTCar);
                return $"Car-{VTCar.RunUserInterfaceString()}";

            }
            else if (choiceVT == 1)
            {
                prompt = "Choose Motorcycle type: ";
                string[] optionVTMotorcycle = { "Underbone", "Scooter", "Naked", "Off-road", "Cafe Racer", "Chopper", "Tourer", "Sports Bike" };

                UserInterface VTMotorcycle = new UserInterface(prompt, optionVTMotorcycle);
                return $"Motorcycle-{VTMotorcycle.RunUserInterfaceString()}";

            }
            else
                return "";
            
        }

        public static string VehicleFuelChoice() //choice method ?: view owned vehicles
        {
            string prompt = "Choose fuel type";
            string[] options = { "Gasoline", "Diesel", "Electric", "Hybrid", "Hydrogen" };

            UserInterface VF = new UserInterface(prompt, options);
            return VF.RunUserInterfaceString();
        }

        public static string VehicleStatusChoice() //choice method ?: view owned vehicles
        {
            string prompt = "Choose vehicle status";
            string[] options = { "Available", "In Maintenance", "Reserved"};

            UserInterface VS = new UserInterface(prompt, options);
            return VS.RunUserInterfaceString();
        }

        

    }
}