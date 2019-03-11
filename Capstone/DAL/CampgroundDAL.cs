using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;
using System.Linq;

namespace Capstone.DAL
{

   public class CampgroundDAL

    {

        //private const string SQL_Campground = "";
        private const string SQL_GetAllCampgrounds = @"SELECT * FROM campground WHERE park_id = @park_id";
        private const string SQL_GetCost = @"SELECT daily_fee FROM campground  WHERE campground_id = @campground_id";
        private string connectionString;

        public CampgroundDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Campground> GetAllCampgroundsForAPark(int park_Id) 
        {
            List<Campground> campgrounds = new List<Campground>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    //Create the command
                    SqlCommand command = new SqlCommand(SQL_GetAllCampgrounds, connection);

                    //Add parameters to my command
                    command.Parameters.AddWithValue("@park_id", park_Id);
                    //execute the command
                    SqlDataReader reader = command.ExecuteReader();

                    //Read each row and turn it into an object
                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        campground.Park_Id = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.Open_From_MM = Convert.ToInt32(reader["open_from_mm"]);
                        campground.Open_To_MM = Convert.ToInt32(reader["open_to_mm"]);
                        campground.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);

                        campgrounds.Add(campground);

                    }
                }
            }

            catch (SqlException exception)
            {
                throw exception;
            }

            return campgrounds;
        }
                
        public decimal GetCost(int campground_Id)
        {

            decimal cost = 0.00M;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(SQL_GetCost, connection);

                    command.Parameters.AddWithValue("@campground_id", campground_Id);

                    
                    cost = (decimal)command.ExecuteScalar();
                }
            }

            catch (Exception)
            {

                throw;
            }

            return cost;
        }
    }
}
        
        
