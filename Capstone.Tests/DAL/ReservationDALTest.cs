using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Models;
using System;
using Capstone.DAL;


namespace Capstone.DAL.Tests
{
    [TestClass]
    public class ReservationDALTest
    {

        private TransactionScope transaction;      //<-- used to begin a transaction during initialize and rollback during cleanup
        private const string DatabaseConnection = @"Data Source=.\sqlexpress;Initial Catalog = NationalParkReservation; Integrated Security = True";


        int fk_park_id = 0;
        int fk_campground_id = 0;
        int fk_site_id = 0;
        int reservation_id = 0;
  

        //Set up the database before each test
       [TestInitialize]
         public void Initialize()
        {
            // Initialize a new transaction scope. This automatically begins the transaction.
            transaction = new TransactionScope();

            // Open a SqlConnection object using the active transaction
            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand command;

                connection.Open();


                //insert tesing campgrounds 
                command = new SqlCommand(@"INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('Testing City Park', 'Testing City ', '1919-02-26', 47389, 2563129, 'Parks may consist of grassy areas, rocks, soil and trees, but may also contain buildings and other artifacts such as monuments, fountains or playground structures. ... Large national and sub-national parks are typically overseen by a park ranger or a park warden.'); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                fk_park_id = (int)command.ExecuteScalar();


                command = new SqlCommand(@"INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES(@park_id, 'Testing Meadow', 1, 12, 35.00); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                command.Parameters.AddWithValue("@park_id", fk_park_id);
                fk_campground_id = (int)command.ExecuteScalar();


                command = new SqlCommand(@"INSERT INTO site(site_number, campground_id) VALUES(1000, @campground_id ); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                command.Parameters.AddWithValue("@campground_id", fk_campground_id);
                fk_site_id = (int)command.ExecuteScalar();

                command = new SqlCommand(@"INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@site_id, 'Testing Family', GETDATE()-6, GETDATE()+1); SELECT CAST(SCOPE_IDENTITY() as int); ", connection);
                command.Parameters.AddWithValue("@site_id", fk_site_id);
                reservation_id = (int)command.ExecuteScalar();

            }//using

        }//initialize

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
        }//cleanup

        [TestMethod]
        public void BookReservationTest()
        {
            //Arrange
            ReservationDAL reservationDal = new ReservationDAL(DatabaseConnection);
            //Act
            int actualReservationid = reservationDal.BookReservation(fk_site_id, "BookReservaton Test", DateTime.UtcNow, DateTime.UtcNow);
            //Assert
            Assert.IsTrue(actualReservationid > 0);
        }        //BookReservationTest




    }//class
}//namespace
