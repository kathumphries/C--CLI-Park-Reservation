using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SiteDAL
    {
        private string connectionString;
        
        private const string SQL_GetAvailableSitesForCampground = (@"SELECT TOP 5 site.* FROM site JOIN campground on campground.campground_id = site.campground_id WHERE campground.campground_id = @campground_id AND @fromMonth BETWEEN campground.open_from_mm AND campground.open_to_mm AND @toMonth BETWEEN campground.open_from_mm AND campground.open_to_mm AND site.site_id NOT IN  (SELECT reservation.site_id FROM reservation WHERE @from_date BETWEEN from_date AND to_date Union SELECT reservation.site_id FROM reservation WHERE @to_date BETWEEN from_date AND to_date Union SELECT reservation.site_id FROM reservation WHERE @from_date <= from_date AND @to_date >= to_date) ");

        private const string SQL_GetAllSitesForCampground = (@"SELECT * FROM site WHERE site.campground_id = @campground_id;");

        private const string SQL_GetSiteIDfromSiteNumberAndCampground = @"SELECT site.site_id FROM site WHERE site_number = @site_number and site.campground_id = @campground_id";
        
        public SiteDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

      

        public List<Site> GetAvailableSitesforCampground(int campground_Id, DateTime from_date, DateTime to_date)
        {
            List<Site> availableSites = new List<Site>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(SQL_GetAvailableSitesForCampground, connection);

                    command.Parameters.AddWithValue("@campground_id", campground_Id);
                    command.Parameters.AddWithValue("@to_date", from_date);
                    command.Parameters.AddWithValue("@from_date", to_date);
                    command.Parameters.AddWithValue("@fromMonth", from_date.Month);
                    command.Parameters.AddWithValue("@toMonth", to_date.Month) ;




                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = new Site();

                        site.Site_Id = Convert.ToInt32(reader["site_id"]);
                        site.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        site.Site_Number = Convert.ToInt32(reader["site_number"]);
                        site.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.Max_Rv_Length = Convert.ToInt32(reader["max_rv_length"]);
                        site.Accessible = Convert.ToBoolean(reader["accessible"]);
                        site.Utilities = Convert.ToBoolean(reader["utilities"]);

                        availableSites.Add(site);
                    }
                }
            }

            catch (SqlException exception)
            {

                throw exception;
            }

            return availableSites;
        }

        public List<Site> GetAllSitesForCampground(int campground_id)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    //Create the command
                    SqlCommand command = new SqlCommand(SQL_GetAllSitesForCampground, connection);

                    //Add parameters to my command
                    command.Parameters.AddWithValue("@campground_id", campground_id);
                    //execute the command
                    SqlDataReader reader = command.ExecuteReader();

                    //Read each row and turn it into an object
                    while (reader.Read())
                    {
                        Site site = new Site();
                        //site.Campground_Id = Convert.ToInt32(reader["site_id"]);

                        site.Site_Id = Convert.ToInt32(reader["site_id"]);
                        site.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                        site.Site_Number = Convert.ToInt32(reader["site_number"]);
                        site.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.Max_Rv_Length = Convert.ToInt32(reader["max_rv_length"]);
                        site.Accessible = Convert.ToBoolean(reader["accessible"]);
                        site.Utilities = Convert.ToBoolean(reader["utilities"]);

                        sites.Add(site);

                    }

                }
            }

            catch (SqlException exception)
            {
                throw exception;
            }
            return sites;

        }

        //public List<Site> GetAllSitesforPark(int park_id, DateTime from_date, DateTime to_date)
        //{
        //    //returns top 5 sites in each campground for a park.  Only returns sites that have availability  and in season

        //    List<Site> sites = new List<Site>();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();

        //            //Create the command
        //            SqlCommand command = new SqlCommand(SQL_GetAllSitesForParkReservation, connection);

        //            //Add parameters to my command
        //            command.Parameters.AddWithValue("@park_id", park_id);
        //            command.Parameters.AddWithValue("@from_date", from_date);
        //            command.Parameters.AddWithValue("@to_date", to_date);

        //            //execute the command
        //            SqlDataReader reader = command.ExecuteReader();

        //            //Read each row and turn it into an object
        //            while (reader.Read())
        //            {
        //                Site site = new Site();
        //                //site.Campground_Id = Convert.ToInt32(reader["site_id"]);

        //                site.Site_Id = Convert.ToInt32(reader["site_id"]);
        //                site.Campground_Id = Convert.ToInt32(reader["campground_id"]);
        //                site.Site_Number = Convert.ToInt32(reader["site_number"]);
        //                site.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
        //                site.Max_Rv_Length = Convert.ToInt32(reader["max_rv_length"]);
        //                site.Accessible = Convert.ToBoolean(reader["accessible"]);
        //                site.Utilities = Convert.ToBoolean(reader["utilities"]);

        //                sites.Add(site);

        //            }

        //        }
        //    }

        //    catch (SqlException exception)
        //    {
        //        throw exception;
        //    }
        //    return sites;

        //}

      
    }
}
