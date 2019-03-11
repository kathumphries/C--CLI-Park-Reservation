using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Linq;

namespace Capstone
{
    public class NationalParkRegistrationCLI
    {
        private const string DatabaseConnection = @"Data Source=.\sqlexpress;Initial Catalog = NationalParkReservation; Integrated Security = True";
        bool continueProgram = true;

        public void RunCLI()
        {
            while (true)
            {
                MainMenu();

            }

        }//RunCLI

        void MainMenu()
        {
            PrintHeader("View Parks Interface");

            //Display parks and gets number of parks displayed
            int numberOfParks = MainMenuDisplayAllParks();
            //Add Quit Option
            Console.WriteLine("Q) Quit".PadRight(10));
            FlowerLine();
            //Get userselection and sanitize
            string parkMenuDisplaySelection = CLIHelper.GetString("\nSelect a Park ID for Further Details:");

            if (parkMenuDisplaySelection == "Q" || parkMenuDisplaySelection == "q")
            {
                EndOfProgramMessage();
                continueProgram = false;
                return;
            }
            else
            {   //Selectioni must be a Park Id - convert string to int.
                int park_Id;
                int.TryParse(parkMenuDisplaySelection, out park_Id);

                // Validate that campgroundIndex is an accepted index for the List of Campgrounds displayed
                if (park_Id <= 0 || numberOfParks < park_Id)
                {
                    Console.WriteLine("Please enter a valid selection!");
                    Console.ReadLine();
                }
                else
                {
                    DisplayParkInformationMenu(park_Id);
                }
            }
        }//MainMenu

        private int MainMenuDisplayAllParks()
        {
            int numberOfParks = 0;

            FlowerLine();

            ParkDAL parkDal = new ParkDAL(DatabaseConnection);
            IList<Park> parks = parkDal.GetAllParks();
            numberOfParks = parks.Count;

            if (numberOfParks > 0)
            {

                foreach (Park park in parks)
                {
                    Console.WriteLine(park.ToString());
                }

            }
            else //no parks returned 
            {
                Console.WriteLine("**** NO RESULTS ****");
            }

            return numberOfParks;
        }//DisplayAll

        private void DisplayParkInformationMenu(int park_Id)
        {
            bool returnToPreviousMenu = false;
            while (returnToPreviousMenu == false)
            {

                PrintHeader("Park Information Screen");
                DisplayParkInformationList(park_Id);

                const int viewCampgrounds = 1;
                const int searchForReservation = 2;
                const int returnToPrevious = 3;

                Console.WriteLine("\nOptions");
                FlowerLine();
                Console.WriteLine("1) View Campgrounds".PadRight(10));
                Console.WriteLine("2) Search for Reservation".PadRight(10));
                Console.WriteLine("3) Return to Previous Screen".PadRight(10));
                FlowerLine();

                int parkInformationScreenSelection = CLIHelper.GetInteger("Please select an option: ");


                //Validate selections 1,2,or 3
                if (parkInformationScreenSelection < 1 || 3 < parkInformationScreenSelection)
                {
                    Console.WriteLine("Please enter a valid selction!");
                    Console.ReadLine();
                }
                else
                {   //peform selection

                    if (parkInformationScreenSelection == viewCampgrounds)
                    {
                        ParkCampgroundsMenu(park_Id);
                    }

                    if (parkInformationScreenSelection == searchForReservation)
                    {
                        ParkwideReservationSearchMenu(park_Id);

                    }

                    if (parkInformationScreenSelection == returnToPrevious)
                    {
                        returnToPreviousMenu = true;
                        return;
                    }
                }
            }
        }//DisplayParkInformationMenu
        private void DisplayParkInformationList(int parkMenuSelectionInt)
        {


            //displays selected park information formatted based on requirments sample screen
            ParkDAL parkDal = new ParkDAL(DatabaseConnection);
            Park park = parkDal.GetPark(parkMenuSelectionInt);

            Console.WriteLine($"\n{park.Name} National Park");
            Console.WriteLine($"{"Location:".PadRight(18)}{park.Location.ToString()}");
            Console.WriteLine($"{"Established:".PadRight(18)}{ park.Establish_Date.ToString("MM/dd/yyyy")}");
            Console.WriteLine($"{"Area:".PadRight(18)}{(park.Area).ToString("N0")} acres");
            Console.WriteLine($"{"Annual Visitors:".PadRight(18)}{park.Visitors.ToString("N0")}");
            Console.WriteLine($"\n{park.Description}\n");

        }//DisplayParkInformationList

        private void ParkCampgroundsMenu(int park_Id)
        {
            bool returnToPreviousMenu = false;

            while (returnToPreviousMenu == false)
            {
                PrintHeader("Park Campgrounds");
                string parkName = parkNameDisplay(park_Id);
                Console.WriteLine($"{parkName} National Park Campgrounds\n");

                ParkCampgroundsList(park_Id);

                const int searchForAvailableReservations = 1;
                const int returnToPrevious = 2;

                Console.WriteLine("\nOptions");
                FlowerLine();
                Console.WriteLine("1) Search for Available Reservation");
                Console.WriteLine("2) Return to Previous Screen");
                FlowerLine();

                int viewCampgroundSelection = CLIHelper.GetInteger("Select an option:");
                if (viewCampgroundSelection == searchForAvailableReservations)
                {
                    CampgroundReservationSearchMenu(park_Id);
                }

                if (viewCampgroundSelection == returnToPrevious)
                {
                    returnToPreviousMenu = true;
                    break;
                }
            }
        }//ParkCampgroundsMenu
        private List<Campground> ParkCampgroundsList(int park_Id)
        {
            CampgroundDAL campgroundDal = new CampgroundDAL(DatabaseConnection);

            List<Campground> campgroundResultList = campgroundDal.GetAllCampgroundsForAPark(park_Id);
            int campgroundCount = campgroundResultList.Count;
            if (campgroundCount > 0)
            {

                Console.WriteLine($"{"".PadRight(7)}{"Name".PadRight(35)}{"Open".PadRight(10)}{"Close".PadRight(10)}{"Daily Fee".PadRight(5)}");

                foreach (Campground campground in campgroundResultList)
                {
                    Console.WriteLine(campground.ToString());
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
            return campgroundResultList;
        }//ViewCampgroundsLis

        private void CampgroundReservationSearchMenu(int park_Id)
        {
            bool returnToPreviousMenu = false;

            while (returnToPreviousMenu == false)
            {

                PrintHeader("Search for Campground Reservation");

                List<Campground> campgrounds = ParkCampgroundsList(park_Id);
                FlowerLine();
                Console.WriteLine();

               List<Site> reservationsAvailable = null;


                while (reservationsAvailable == null)
                {

                    int campground_Id = CLIHelper.GetInteger(@"Which campground (enter 0 to cancel)?");

                    if (campground_Id == 0)
                    {
                        returnToPreviousMenu = true;
                        return;
                    }
                    //check to see if they are selecting a campground that was listed....
                    else if (!campgrounds.Any(campground =>campground.Campground_Id == campground_Id))
                    {
                        WarningMessageFormat("This Campground ID is not available please try again");
                        Console.ReadLine();
                    }
                    else
                    {

                        DateTime from_date = new DateTime();
                        DateTime to_date = new DateTime();


                        while (from_date.Date >= to_date.Date || from_date.Date <= DateTime.Today)  // can not book in the past? (at least 1 day stay ie to date must be atleast 1 in the future of the from date
                        {
                            Console.WriteLine("Reservations must be booked for at least 1 day.");
                            from_date = CLIHelper.GetDateTime("What is the arrival date? (YYYY-MM-DD)");
                            to_date = CLIHelper.GetDateTime("What is the departure date? (YYYY-MM-DD)");
                            FlowerLine();
                            
                            if(from_date.Date >= to_date.Date)
                            {
                                WarningMessageFormat("Departure Date must be after Arrival Date!");
                            }

                            if (from_date.Date < DateTime.Today)
                             {
                                WarningMessageFormat("Unable to book for past dates!");
                            }


                            Console.WriteLine();
                        }
                   
                        


                        //datetime in rang and also now or greater than today for atleast one day 
                        //if (from_date >= 1753-01-01 && from_date <= 9999-12-31)


                        Console.WriteLine("\nResults Matching Your Search Criteria\n");
                        reservationsAvailable = CampgroundReservationSearchResults(park_Id, campground_Id, from_date, to_date);

                        if (reservationsAvailable.Count > 0)
                        {
                            MakeReservationMenu(park_Id, from_date, to_date, reservationsAvailable);
                            FlowerLine();
                        }
                        
                        else
                        {
                            NoSitesAvailable();
                            FlowerLine();
                        }
                        
                    }

                }
            }
        }//ViewCampgroundsList
        private List<Site> CampgroundReservationSearchResults(int park_Id, int campground_id, DateTime from_date, DateTime to_date)
        {


            
            // Connect to Database.  Return onlty the first 5 available campsites per requirements;

            CampgroundDAL campgroundDal = new CampgroundDAL(DatabaseConnection);
            SiteDAL siteDAL = new SiteDAL(DatabaseConnection);
            List<Site> sites = siteDAL.GetAvailableSitesforCampground(campground_id, from_date, to_date);

            if (sites != null)
            {
            
                //Dispaly Site#, Max Occupancy, Accessibility, Max RV Length, and whether Utilities are available
                Console.WriteLine($"{"Site No.".PadRight(10)}{"Max Occup.".PadRight(15)}{"Accessible?".PadRight(15)}{"Max Rv Length".PadRight(15)}{"Utility".PadRight(15)}{"Stay Cost".PadRight(10)}");

                foreach (Site site in sites)
                {
                    TimeSpan daysBooking = (to_date.Date - from_date.Date);
                    int days = (int)daysBooking.TotalDays;

                    string cost = ((campgroundDal.GetCost(campground_id) * days)).ToString("C").PadRight(10);
                    string siteWithCost = (site.ToString() + cost);
                    Console.WriteLine(siteWithCost);
                }
            }
            else
            {
                NoSitesAvailable();
            }
            return sites;
        }//CampgroundReservationSearchResults

        private void ParkwideReservationSearchMenu(int park_Id)
        {

            bool returnToPreviousMenu = false;
            List<Site> availableSites = new List<Site>();

            while (returnToPreviousMenu == false)
            {

                PrintHeader("Search for Park-wide Reservation");
                Console.WriteLine("Please call for details!");
                Console.ReadLine();
                returnToPreviousMenu = true;
            

                //bool isSitesAvailable = false;
                //DateTime from_date = DateTime.Today;
                //DateTime to_date = DateTime.Today;

                //while (!isSitesAvailable)
                //{
                //    from_date = CLIHelper.GetDateTime("What is the arrival date? (YYYY-MM-DD)");
                //    to_date = CLIHelper.GetDateTime("What is the departure date? (YYYY-MM-DD)");
                //    Console.WriteLine();

                //    availableSites = ParkWideReservationSearchResult(park_Id, from_date, to_date);
                //    isSitesAvailable = (availableSites != null);
                //}

                //// Now that the user has list of sites by park availalbe for ArrivalDate, and DepartureDate book reservation
                //FlowerLine();
                //MakeReservationMenu(park_Id, from_date, to_date, availableSites);
            }

        }//ParkWideReservationMenu
    
        
        //  private List<Site> ParkWideReservationSearchResult(int park_Id, DateTime from_date, DateTime to_date)
      //  {

            //    // Connect to Database.  Return onlty the first 5 available campsites per requirements;

            //CampgroundDAL campgroundDal = new CampgroundDAL(DatabaseConnection);
            //SiteDAL siteDAL = new SiteDAL(DatabaseConnection);
            //List<Site> availableSites = new List<Site>//siteDAL.GetAllSitesforPark(park_Id, from_date, to_date);

            //    if (availableSites != null)
            //    {
            //         Console.WriteLine("Results Matching Your Search Criteria Campground Site");
            //         Console.WriteLine($"{"Site No.".PadRight(10)}{"Max Occup.".PadRight(15)}{"Accessible?".PadRight(15)}{"Max Rv Length".PadRight(15)}{"Utility".PadRight(15)}{"Cost".PadRight(10)}");

            //        foreach (Site site in availableSites)
            //        {
            //            string cost = "";
            //            string siteWithCost = (site.ToString() + cost);
            //            Console.WriteLine(siteWithCost);
            //        }
            //    }
            //    else
            //    {
            //        //if list of available sites is empty display message asking user if they would like to 
            //        //select alternate dates
            //        NoSitesAvailable();

            //    }
       //     return availableSites;

        //}//ParkWideReservationSearchResult


        //Reservations
        void NoSitesAvailable()
        {

            //if list of available sites is empty display message asking user if they would like to 
            //select alternate dates

            WarningMessageFormat("No campsites available!");
            Console.ReadLine();

            const int newDateSearch = 1;
            const int mainMenu = 2;
            const int quit = 3;

            Console.WriteLine("\nOptions");
            FlowerLine();
            Console.WriteLine("1) Enter a new date range for this search");
            Console.WriteLine("2) Return to Main Menu");
            Console.WriteLine("3) Quit Program");
            FlowerLine();
            int noSitesAvailableSelection = CLIHelper.GetInteger("Please select an option: ");

            if (noSitesAvailableSelection == newDateSearch)
            {
                return; //returns to the loop
            }
            else if (noSitesAvailableSelection == mainMenu)
            {
                MainMenu();
            }
            else if (noSitesAvailableSelection == quit)
            {
                EndOfProgramMessage();
            }
            else
            {
                WarningMessageFormat("Selection not available");
                Console.WriteLine("Let's try a new search");
                Console.ReadLine();
                return;

            }
        }//noSitesAvaialable
        void MakeReservationMenu(int park_Id, DateTime from_date, DateTime to_date, List<Site> reservationsAvailable)

        {
            bool returnToPreviousMenu = false;

            while (returnToPreviousMenu == false)
            {

               
               
                int site_selection = CLIHelper.GetInteger("Which site should be reserved(enter 0 to cancel)?");
                    
                if (site_selection == 0)
                {
                    returnToPreviousMenu = true;
                   
                }
                else if (!reservationsAvailable.Any(site => site.Site_Number == site_selection))
                {
                    WarningMessageFormat("This site is not available");
                    Console.ReadLine();
                  
                }
                else
                {
                    //look up site id from avaialbe sites list using site Number.  pull site id property for reservation information. 
                    Site selectedSite = (reservationsAvailable.Find(site => site.Site_Number == site_selection));                   

                    string name = CLIHelper.GetString("What name should the reservation be made under?");
                    ReservationDAL reservationDal = new ReservationDAL(DatabaseConnection);
                    int reservation_id = reservationDal.BookReservation(selectedSite.Site_Id, name, from_date, to_date);
                    FlowerLine();
                    string confirmationMessage = ($"The reservation has been made and the confirmation id is {reservation_id}");
                    confirmationMessage += $"\nYou have booked Site No. {selectedSite.Site_Number} for {from_date.ToString("yyyy/MM/dd")} to {to_date.ToString("yyyy/MM/dd")}. Enjoy your stay!";
                    ConfirmationMessageFormat(confirmationMessage);
                    Console.ReadLine();
                    break;
                }
               
            }

        } //MakeReservationMenu
        private void EndOfProgramMessage()
        {
            Console.Clear();
            Console.WriteLine("\n \tThank you for using the National Parks System. Have a nice day!\n \n");
            Console.WriteLine(@"                                  # #### ####");
            Console.WriteLine(@"                                ### \/#|### |/####");
            Console.WriteLine(@"                               ##\/#/ \||/##/_/##/_#");
            Console.WriteLine(@"                             ###  \/###|/ \/ # ###");
            Console.WriteLine(@"                           ##_\_#\_\## | #/###_/_####");
            Console.WriteLine(@"                          ## #### # \ #| /  #### ##/##");
            Console.WriteLine(@"                           __#_--###`  |{,###---###-~");
            Console.WriteLine(@"                                     \ }{");
            Console.WriteLine(@"                                      }}{");
            Console.WriteLine(@"                                      }}{");
            Console.WriteLine(@"                                      {{}");
            Console.WriteLine(@"                                , -=-~{ .-^- _");
            Console.WriteLine(@"                                      `}");
            Console.WriteLine(@"                                       {");
            Console.ReadLine();
            Environment.Exit(0);

        }//EndofProgram


        //formatting 
        private void PrintHeader(string menuTitle)
        {
            Console.Clear();
            FlowerLine();
            int paddingSetting = 3;
            Console.Write("*".PadRight(paddingSetting) + "NATIONAL PARKS RESERVATION SYSTEM".PadLeft(paddingSetting).PadRight(paddingSetting) + "*".PadLeft(paddingSetting) + "\n");
            FlowerLine();
            Console.WriteLine(menuTitle + "\n");


        } //PrintHeader
        void FlowerLine()
        {
            int length = Console.LargestWindowWidth / 2;
            Console.WriteLine("".PadRight(40, '*'));
        }//FlowerLine
        static void ConfirmationMessageFormat(string message)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
        }//ConfirmationMessageFormat

        public static void WarningMessageFormat(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        } //WarningMessageFormat

        private string parkNameDisplay(int park_Id)
        {
            ParkDAL parkDal = new ParkDAL(DatabaseConnection);
            Park park = parkDal.GetPark(park_Id);
            return ($"{park.Name}");
        }//parkNameDisplay

       //DateTime dateToCheck = DateTime(1753, 01, 01, 0, 0, 0);

       //IsDateSQLApproved(dateToCheck);

       // public static void IsDateSQLApproved(DateTime dateToCheck)
       // {
       //     DateTime startDate = new DateTime(1753, 01, 01, 0, 0, 0);
       //     DateTime endDate = new  DateTime(9999, 12, 31, 0, 0, 0);
       //     return dateToCheck >= startDate && dateToCheck < endDate;
       // }


    }//class
}//namespace
