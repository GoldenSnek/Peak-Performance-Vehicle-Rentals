using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Abstraction - Through the process of abstraction, a programmer hides all but the relevant data about an object in order to reduce complexity and increase efficiency.
// In the same way that abstraction sometimes works in art, the object that remains is a representation of the original, with unwanted detail omitted.
// -googly moogly

namespace Peak_Performance_Vehicle_Rentals
{
    //User Interface
    public abstract class AbstractUserInterface
    {
        private int choice;
        private string[] options;
        private string prompt;
        public int Choice { get; set; }
        public string[] Options { get; set; }
        public string Prompt { get; set; }
        abstract public int RunUserInterface();
        abstract public string RunUserInterfaceString();
        abstract public void DisplayOptions();
        abstract public void WaitForKey();
        abstract public void WaitForSpecificKey();
    }

    //Login Register
    public interface ILoginRegister
    {
        public string UserLogin(FilePathManager file);
        public void UserRegister(FilePathManager file);
    }

    //MAIN MENU
    public interface IVehicleManagement
    {
        public void ViewRentalVehicles(FilePathManager file);
        public void AddVehicle(string username);
        public void UpdateVehicle(string username, FilePathManager file);
        public void DeleteVehicle(string username, FilePathManager file);
    }
    public interface IVehicleDetailManagement
    {
        public string VehicleName();
        public string VehicleModel();
        public string VehicleYear();
        public string VehicleLicensePlate();
        public string VehicleColor();
        public string VehicleSeatingCapacity();
        public string VehicleMileage();
        public string VehicleLocation();
        public string VehiclePrice();
    }
    public interface IUserManagement
    {
        public void ViewUserDetails(string username, FilePathManager file);
    }
    public interface IUserDetailManagement
    {
        //add soon
    }

    //Choice
    public interface IChoice
    {
        public int LoginRegisterChoice();
        public int MainMenuChoice();
        public int ViewAllVehiclesChoice(FilePathManager file);
        public int ViewOwnedVehiclesChoice(string username, FilePathManager file);
        public int ManageVehiclesChoice();
        public string UpdateVehicleDetailsChoice(string username, FilePathManager file, int choice);
        public string VehicleTypeChoice();
        public string VehicleFuelChoice();
        public string VehicleStatusChoice();
    }

    //Inventory
    public interface IInventoryManagement
    {
        public string[] ViewVehicles(FilePathManager file);
    }

    //FileManager
    public interface IUserFileManagement
    {
        public void CreateUserFile(string username);
        public void UpdateUserFile();
        public void DeleteUserFile();
        public void DisplayUserFile(string username);
    }
    public interface IVehicleFileManagement
    {
        public void CreateVehicleFile(string username, string[] details);
        public void UpdateVehicleFile(string username, FilePathManager file, int choice, string detailchoice, string newdetail);
        public void DeleteVehicleFile(string username, FilePathManager file, int choice);
        public void DisplayVehicleFile(int DVchoice);
    }
}
