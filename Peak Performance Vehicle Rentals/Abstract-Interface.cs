using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_Performance_Vehicle_Rentals
{
    public interface IUserInterface
    {
        public int RunUserInterface();
        public string RunUserInterfaceString();
        public void DisplayOptions();
        public void WaitForKey();
        public void WaitForSpecificKey();
    }
    public interface IChoice
    {
        public int LoginRegisterChoice();
        public int MainMenuChoice();
        public int ViewAllVehiclesChoice(FilePathManager file);
        public int ViewOwnedVehiclesChoice(string username, FilePathManager file);
        public int ManageVehiclesChoice();
        public string VehicleTypeChoice();
        public string VehicleFuelChoice();
        public string VehicleStatusChoice();
    }
    public interface IInventoryManagement
    {
        public string[] ViewVehicles(FilePathManager file);
    }

    /*internal abstract class ChoiceBase
    {
        internal abstract string prompt { get; set; }
        internal abstract string options { get; set; }
    }*/
}
