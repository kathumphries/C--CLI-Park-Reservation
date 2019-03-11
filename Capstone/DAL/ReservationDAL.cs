using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ReservationDAL
    {
        private string connectionString;
        private const string SQL_BookReservation = @"INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@site_id, @name, @from_date, @to_date); SELECT CAST(SCOPE_IDENTITY() as int);";

        public ReservationDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public int BookReservation(int site_id, string name, DateTime from_date, DateTime to_date)
        {
            Reservation reservation = new Reservation();           
            int reservation_id = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Create the command
                    SqlCommand command = new SqlCommand(SQL_BookReservation, connection);

                    //Add parameters to my command
                    command.Parameters.AddWithValue("@site_id", site_id);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@from_date", from_date);
                    command.Parameters.AddWithValue("@to_date", to_date);

                    //execute the command
                    reservation_id = (int)command.ExecuteScalar();
                }
            }

            catch (SqlException exception)
            {
                throw exception;
            }

            return reservation_id;
        }
    }
}
