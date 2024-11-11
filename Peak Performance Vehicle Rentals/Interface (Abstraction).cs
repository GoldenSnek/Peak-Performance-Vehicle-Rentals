using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// Abstraction - Through the process of abstraction, a programmer hides all but the relevant data about an object in order to reduce complexity and increase efficiency.
// In the same way that abstraction sometimes works in art, the object that remains is a representation of the original, with unwanted detail omitted.
// -googly moogly

namespace Peak_Performance_Vehicle_Rentals
{
    //User Interface
    public abstract class UserInterfaceBase
    {
        private int choice;
        private string[]? options;
        private string? prompt;
        public int Choice { get { return choice; } set { choice = value; } }
        public string[]? Options { get { return options; } set { options = value; } }
        public string? Prompt { get { return prompt; } set { prompt = value; } }
    }
    public interface IUserInterface
    {
        public int RunUserInterface(string type);
        public string RunUserInterfaceString(string type);
        public void DisplayOptions(bool verbatim);
    }

    //Login Register
    public abstract class LoginRegisterBase
    {
        private string? username;
        private string? password;
        public string? Username { get { return username; } set { username = value; } }
        public string? Password { get { return password; } set { password = value; } }
    }
    public interface ILoginRegister
    {
        public string UserLogin(FilePathManager file);
        public void UserRegister(FilePathManager file);
    }

    //Main Menu
    public interface IVehicleManagement
    {
        public void SearchRentalVehicles(string username, FilePathManager file);
        public void ViewRentalVehicles(string username, FilePathManager file);
        public void PendingVehicles(string username, FilePathManager file);
        public void ApprovedVehicles(string username, FilePathManager file);
        public void CurrentlyRentingVehicle(string username, FilePathManager file);
        public void AddVehicle(string username);
        public void UpdateVehicle(string username, FilePathManager file);
        public void DeleteVehicle(string username, FilePathManager file);
    }
    public interface IUserManagement
    {
        public void ViewUserDetails(string username, FilePathManager file);
        public void UpdateUser(string username, FilePathManager file);
        public bool DeleteUser(string username, FilePathManager file);
    }

    //Choice
    public abstract class AbstractChoice
    {
        private string? prompt;
        private string[]? options;
        public string? Prompt { get { return prompt; } set { prompt = value; } }
        public string[]? Options { get { return options; } set { options = value; } }
    }
    public interface IChoice
    {
        public int LoginRegisterChoice();
        public int MainMenuChoice(string username);
        public int RentalChoice(FilePathManager file);
        public string ViewSearchedVehiclesChoice(string keyword, FilePathManager file);
        public int ViewAllVehiclesChoice(FilePathManager file);
        public int VehicleRentChoice(string vehicleOwner, string username);
        public int ViewOwnedVehiclesChoice(string username, FilePathManager file);
        public int ViewPendingChoice(string username, FilePathManager file);
        public int RentalTimeChoice();
        public int RentalDetailsChoice(string username, FilePathManager file);
        public int ApprovePendingChoice();
        public int CurrentlyRentingChoice(string username, FilePathManager file);
        public int ManageVehiclesChoice();
        public string UpdateVehicleDetailsChoice(string username, FilePathManager file, int choice);
        public string VehicleTypeChoice();
        public string VehicleFuelChoice();
        public int ManageUserChoice();
        public string UpdateUserDetailsChoice(string username, FilePathManager file);
        public int DeleteUserChoice(string username, FilePathManager file);
    }

    //Inventory
    public interface IInventoryManagement
    {
        public string[] ViewVehicles(FilePathManager file);
        public string[] ViewSearchedVehicles(string keyword, FilePathManager file);
        public string[] ViewVehicleDetails(string username, FilePathManager file, int choice);
        public string[] ViewUserDetails(string username, FilePathManager file);
        public string ViewPendingRentalClient(string username, FilePathManager file);
        public string[] ViewPendingRentalOwner(string username, FilePathManager file);
        public string[] ViewApprovedRental(string username, FilePathManager file);
        public string ViewCurrentRental(string username, FilePathManager file);


    }

    //FileManager
    public abstract class FilePathManagerBase
    {
        private string? baseDirectory;
        public string? BaseDirectory { get { return baseDirectory; } set { baseDirectory = value; } }
    }
    public interface IUserFileManagement
    {
        public void CreateUserFile(string username);
        public void UpdateUserFile(string username, string detailchoice, string newdetail);
        public int DeleteUserFile(string username, FilePathManager file);
        public void DisplayUserFile(string type, string username);
    }
    public interface IVehicleFileManagement
    {
        public void CreateVehicleFile(string username, string[] details);
        public void UpdateVehicleFile(string username, int choice, string detailchoice, string newdetail);
        public void DeleteVehicleFile(string username, FilePathManager file, int choice);
        public string[] DisplayVehicleFile(int DVchoice, string search, string type);
        public void TransferPendingFile(int choice, string[] rentDetails, string username, string search, string type);
        public void TransferApprovedFile(int choice, string username, FilePathManager file);
        public void TransferNonApprovedFile(int choice, string username, FilePathManager file);
        public void DisplayPendingFile(int choice, string username, FilePathManager file);
        public void DisplayRecieptFile(string username, FilePathManager file);
        public void TransferFinishRentFile(string username, FilePathManager file);
    }
}