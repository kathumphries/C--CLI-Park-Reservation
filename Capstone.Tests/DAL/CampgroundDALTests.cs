using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.DAL.Tests
{
    [TestClass()]
    public class CampgroundDALTests
    {
        private TransactionScope transaction;
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog = NationalParkReservation; Integrated Security = True";
        private int parkId;
        private int campgroundId;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Insert Park Dummy Record - Return park ID
                SqlCommand command = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('Yellowstone', 'Wyoming', '0001-01-01', 1, 1, 'Dummy park for testing'); SELECT CAST(SCOPE_IDENTITY() as int)", connection);
                parkId = (int)command.ExecuteScalar();

                //Insert Campground Dummy Record - Return campground ID
                command = new SqlCommand("INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES (@park_id, 'Madison', 1, 12, 1.00); SELECT CAST(SCOPE_IDENTITY() as int)", connection);
                command.Parameters.AddWithValue("@park_id", parkId);
                campgroundId = (int)command.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod()]
        public void GetAllCampgroundsForAParkTest()
        {
            //Arrange
            CampgroundDAL dal = new CampgroundDAL(connectionString);

            //Act
            List<Campground> campgrounds = dal.GetAllCampgroundsForAPark(parkId);

            //Assert
            Assert.IsNotNull(campgrounds);
            Assert.AreEqual(1, campgrounds.Count);
        }

        [TestMethod()]
        public void GetCostTest()
        {
            CampgroundDAL dal = new CampgroundDAL(connectionString);

            decimal cost = dal.GetCost(campgroundId);

            Assert.AreEqual(1.00M, dal.GetCost(campgroundId));   
        }
    }
}
