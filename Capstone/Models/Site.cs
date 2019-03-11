using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;



namespace Capstone.Models
{
    public class Site
    {
        //properties
        public string Name          { get; set; }
        public int Site_Id          { get; set; }
        public int Campground_Id    { get; set; }
        public int Site_Number      { get; set; }
        public int Max_Occupancy    { get; set; }
        public int Max_Rv_Length    { get; set; }
        public bool Accessible      { get; set; }
        public bool Utilities       { get; set; }
       

        //constructor
        public Site()
        {

        }

        public Site(string name, int site_Id, int campground_Id, int site_Number, int max_Occupancy, int max_Rv_Length, bool accessible, bool utilities)
        {
            Name = name;
            Site_Id = site_Id;
            Campground_Id = campground_Id;
            Site_Number = site_Number;
            Max_Occupancy = max_Occupancy;
            Max_Rv_Length = max_Rv_Length;
            Accessible = accessible;
            Utilities = utilities;
        }

        public override string ToString()
        {
            
            string formattedUtilities = Utilities ? "Yes" : @"N/A";
            string formattedRV = Max_Rv_Length==0 ? @"N/A": Max_Rv_Length.ToString();
            string formattedAccessible = Accessible ? "Yes" : "No";
           string result = ($"{Site_Number.ToString().PadRight(10)}{Max_Occupancy.ToString().PadRight(15)}{formattedAccessible.PadRight(15)}{formattedRV.PadRight(15)}{formattedUtilities.PadRight(15)}");

            return result;
        }

    }
}
