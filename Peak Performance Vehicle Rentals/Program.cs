using System;
using System.IO;

//Notes for self:
//1. User Data file path may vary depending on device used

//To-Do List:
//1.


namespace Peak_Performance_Vehicle_Rentals
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //variable declarations



            //Register-Login
            bool LRrunning = true;
            bool ValidUser = false;
            do
            {
                int LRchoice = Choice.LoginRegisterChoice();
                switch (LRchoice)
                {
                    case 1:
                        ValidUser = Login.UserLogin();
                        if (ValidUser)
                        {
                            Console.WriteLine("Login Successful!");
                            LRrunning = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid credentials!");
                            LRrunning = true;
                        }
                        break;

                    case 2:
                        Register.UserRegister();
                        break;

                    default:
                        break;
                }
            } while (LRrunning);






            //Succesful Login
            if (ValidUser)
            {


                Console.WriteLine("hello world!");
                //rest of the program


            }

        }
    }
}