using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;


namespace Capstone.DAL
{

   public class ParkDAL

    {
        private string connectionString;
        private const string SQL_GetAllParks = @"SELECT * FROM park ORDER BY name"; 
        private const string SQL_GetPark = @"SELECT * FROM park WHERE park_id = @park_id;"; 
        public ParkDAL ()
        {

        }
            
        public ParkDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Park GetPark(int park_Id)
        {
            Park parkSearchResult = new Park();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Create the command
                    SqlCommand command = new SqlCommand(SQL_GetPark, connection);
                    
                    //Add parameters to my command
                    command.Parameters.AddWithValue("@park_id", park_Id);

                    //execute the command
                    SqlDataReader reader = command.ExecuteReader();

                    //Read each row and turn it into an object
                    while (reader.Read())
                    {
                        parkSearchResult.Park_Id = Convert.ToInt32(reader["park_id"]);
                        parkSearchResult.Name = Convert.ToString(reader["name"]);
                        parkSearchResult.Location = Convert.ToString(reader["location"]);
                        parkSearchResult.Establish_Date = Convert.ToDateTime(reader["establish_date"]);
                        parkSearchResult.Area = Convert.ToInt32(reader["area"]);
                        parkSearchResult.Visitors = Convert.ToInt32(reader["visitors"]);
                        parkSearchResult.Description = Convert.ToString(reader["description"]);
                    }
                }
            }

            catch (SqlException exception)
            {
                throw exception;
            }

            return parkSearchResult;
        }

        public List<Park> GetAllParks() 
        {
            List<Park> allParks = new List<Park>();
                       
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    //Create the command
                    SqlCommand command = new SqlCommand(SQL_GetAllParks, connection);

                    //Add parameters to my command
                    
                    //execute the command
                    SqlDataReader reader = command.ExecuteReader();

                    //Read each row and turn it into an object
                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.Park_Id = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.Establish_Date = Convert.ToDateTime( reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description =Convert.ToString(reader["description"]);
                        allParks.Add(park); 
                    }
                }
            }

            catch (SqlException exception)
            {
                throw exception;
            }

            return allParks;
        }//GetAllParks

      
    }//class

}//namespace
