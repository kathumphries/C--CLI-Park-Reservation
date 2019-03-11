using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int Campground_Id { get; set; }
        public int Park_Id { get; set; }
        public string Name { get; set; }
        public int Open_From_MM { get; set; }
        public int Open_To_MM { get; set; }
        public decimal Daily_Fee { get; set; }

        public Campground()
        {

        }

        public Campground(int campground_Id, int park_Id, string name, int open_From_MM, int open_To_MM, decimal daily_Fee)
        {
            Campground_Id = campground_Id;
            Park_Id = park_Id;
            Name = name;
            Open_From_MM = open_From_MM;
            Open_To_MM = Open_To_MM;
            Daily_Fee = daily_Fee;
        }

     


        public override string ToString()
        {
            string closeMonth = convertMonthNumToString(Open_To_MM);
            string openMonth = convertMonthNumToString(Open_From_MM);
            return ("#"+Campground_Id).PadRight(7)+ Name.PadRight(35) + openMonth.PadRight(10) + closeMonth.PadRight(10) + Daily_Fee.ToString("C2").PadRight(5);       }

        private string convertMonthNumToString(int number)
        {
            string result = "";
            switch (number)
            {
                case 1:
                    result =("January");
                    break;
                case 2:
                    result = ("February");
                    break;
                case 3:
                    result = ("March");
                    break;
                case 4:
                    result = ("April");
                    break;
                case 5:
                    result = ("May");
                    break;
                case 6:
                    result = ("June");
                    break;
                case 7:
                    result = ("July");
                    break;
                case 8:
                    result = ("August");
                    break;
                case 9:
                    result = ("September");
                    break;
                case 10:
                    result = ("October");
                    break;
                case 11:
                    result = ("November");
                    break;
                case 12:
                    result = ("December");
                    break;
                default:
                    Console.Write("Unknown");
                    break;
            }
            return result;
        }
    }
}
