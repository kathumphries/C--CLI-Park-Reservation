using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Park
    {
        public int Park_Id              { get; set; }
        public string Name              { get; set; }
        public string Location          { get; set; }
        public DateTime Establish_Date  { get; set; }
        public int Area                 { get; set; }
        public int Visitors             { get; set; }
        public string Description       { get; set; }

        public Park()
        {

        }

        public Park(int park_Id, string name)
        {
            Park_Id = park_Id;
            Name = name;
        }
        public Park(int park_Id, string name, string location, DateTime establish_Date, int area, int visitors, string description)
        {
            Park_Id = park_Id;
            Name = name;
            Location = location;
            Establish_Date = establish_Date;
            Area = area;
            Visitors = visitors;
            Description = description;
        }

        public override string ToString()
        { 
            return (Park_Id.ToString()+ ") " + Name.PadRight(10));
        }
        }
}
