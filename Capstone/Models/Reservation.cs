using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    class Reservation
    {
        //properties
      public int Reservation_Id   { get; set; }
      public int Site_Id          { get; set; }
      public string Name          { get; set; }
      public DateTime From_Date   { get; set; }
      public DateTime To_Date     { get; set; }
      public DateTime Create_Date { get; set; }
          
        //contructor
        public Reservation()
        {

        }
        public Reservation (int reservation_Id, int site_Id, string name, DateTime from_Date, DateTime to_Date, DateTime create_Date)
        {
            Reservation_Id = reservation_Id;  
            Site_Id = site_Id;
            Name = name;
            From_Date = from_Date;
            To_Date = to_Date;
            Create_Date = create_Date;
        }

        public override string ToString()
        {
            //int reservation_Id, int site_Id, string name, DateTime from_Date, DateTime to_Date, DateTime create_Date)
            return base.ToString();
        }

    }
}
