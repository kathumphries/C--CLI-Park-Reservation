using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Models;
using System.Linq;
namespace Capstone.DAL.Tests
{
    [TestClass]
    public class SiteDALTests
    {


        private TransactionScope transaction;      //<-- used to begin a transaction during initialize and rollback during cleanup

        private const string DatabaseConnection = @"Data Source=.\sqlexpress;Initial Catalog = NationalParkReservation; Integrated Security = True";

     
        int fk_park_id1 = 0;
        int fk_park_id2 = 0;
        int fk_campground_id1 = 0;
        int fk_campground_id2 = 0;
        int site_id1 = 0;
        int site_id2 = 0;
        int site_id3 = 0;
        DateTime toDate = new DateTime(2019, 07, 31, 16, 45, 0);
        DateTime fromDate = new DateTime(2019, 08, 30, 16, 45, 0);

        // Set up the database before each test        

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
                fk_park_id1 = (int)command.ExecuteScalar();

                command = new SqlCommand(@"INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('Testing City Park', 'Testing City ', '1919-02-26', 47389, 2563129, 'Parks may consist of grassy areas, rocks, soil and trees, but may also contain buildings and other artifacts such as monuments, fountains or playground structures. ... Large national and sub-national parks are typically overseen by a park ranger or a park warden.'); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                fk_park_id2 = (int)command.ExecuteScalar();

                command = new SqlCommand(@"INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES(@park_id, 'Testing Meadow', 1, 12, 35.00); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                command.Parameters.AddWithValue("@park_id", fk_park_id1);
                fk_campground_id1 = (int)command.ExecuteScalar();

                command = new SqlCommand(@"INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES(@park_id, 'Testing Meadow', 1, 12, 35.00); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                command.Parameters.AddWithValue("@park_id", fk_park_id2);
                fk_campground_id2 = (int)command.ExecuteScalar();


                command = new SqlCommand(@"INSERT INTO site(site_number, campground_id) VALUES(1001,@campground_id ); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                command.Parameters.AddWithValue("@campground_id", fk_campground_id1);
                site_id1 = (int)command.ExecuteScalar();

                command = new SqlCommand(@"INSERT INTO site(site_number, campground_id) VALUES(1002, @campground_id ); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                command.Parameters.AddWithValue("@campground_id", fk_campground_id2);
                site_id2 = (int)command.ExecuteScalar();

                command = new SqlCommand(@"INSERT INTO site(site_number, campground_id) VALUES(1003, @campground_id ); SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                command.Parameters.AddWithValue("@campground_id", fk_campground_id2);
                site_id3 = (int)command.ExecuteScalar();


            }

        }//initialize

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
        }//cleanup


        [TestMethod]
        public void GetAllSitesForCampgroundTest()
        {
            //Arrange
            SiteDAL siteDAL = new SiteDAL(DatabaseConnection);
            List<Site> testSites1 = new List<Site>();
            List<Site> testSites2 = new List<Site>();
            //ACT

            testSites1 = siteDAL.GetAllSitesForCampground(fk_campground_id1);
           testSites2 = siteDAL.GetAllSitesForCampground(fk_campground_id2);
            //Assert

            Assert.IsTrue(testSites1.Any(site => site.Site_Number == 1001));
            Assert.AreEqual(testSites1.Count, 1);
            Assert.AreEqual(testSites2.Count, 2);

        }

        //[TestMethod]
        //public void GetAllSitesforParkTest()
        //{
        //    SiteDAL siteDAL = new SiteDAL(DatabaseConnection);
        //    List<Site> parkSite = new List<Site>();

        //    parkSite = siteDAL.GetAllSitesforPark(fk_park_id1, fromDate, toDate);

        //    Assert.IsTrue(parkSite.Count == 1);

        //    parkSite = siteDAL.GetAllSitesforPark(fk_park_id2, fromDate, toDate);

        //    Assert.IsTrue(parkSite.Count == 2);
   
        //}
        [TestMethod]
        public void GetAvailableSitesTest()
        {
            //Arrange
            SiteDAL siteDAL = new SiteDAL(DatabaseConnection);
            //Act
            List<Site> availableSites = new List<Site>();
            availableSites = siteDAL.GetAvailableSitesforCampground(fk_campground_id1, fromDate, toDate);
            //Assert
            Assert.IsTrue(availableSites.Any(site => site.Site_Number==1001));
            Assert.AreEqual(availableSites.Count, 1);



            availableSites = siteDAL.GetAvailableSitesforCampground(fk_campground_id2, fromDate, toDate);
            Assert.IsTrue(availableSites.Any(site => site.Site_Number == 1002));
            Assert.IsTrue(availableSites.Any(site => site.Site_Number == 1003));
            Assert.AreEqual(availableSites.Count, 2);

            // out of season should not show..
                     
            // reserve site 1002 so it doesnt show available on request time
            // Open a SqlConnection object using the active transaction
           



        using (TransactionScope ts = new TransactionScope())
            {

            using (SqlConnection connection = new SqlConnection(DatabaseConnection))
            {
                SqlCommand command;
                 connection.Open();
                command = new SqlCommand(@"INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@site_id, 'Testing Family', @to_date, @from_date); SELECT CAST(SCOPE_IDENTITY() as int); ", connection);
                command.Parameters.AddWithValue("@site_id", site_id2);
                command.Parameters.AddWithValue("@name", "Testing");
                command.Parameters.AddWithValue("@from_date",fromDate);
                command.Parameters.AddWithValue("@to_date", toDate);
                int reservation_id = (int)command.ExecuteScalar();

            }
            availableSites = siteDAL.GetAvailableSitesforCampground(fk_campground_id2, fromDate, toDate);
            Assert.IsFalse(availableSites.Any(site => site.Site_Number == 1002));
            Assert.IsTrue(availableSites.Any(site => site.Site_Number == 1003));
            Assert.AreEqual(availableSites.Count, 1);
        }
    }

    }//class
}//namespace