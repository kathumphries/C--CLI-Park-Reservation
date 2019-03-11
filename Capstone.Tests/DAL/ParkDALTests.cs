using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.DAL.Tests
{
    [TestClass()]
    public class ParkDALTests
    {
        private TransactionScope transaction;
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog = NationalParkReservation; Integrated Security = True";
        private int numberOfParks;
        private int parkId;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Get number of parks
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM park", connection);
                numberOfParks = (int)command.ExecuteScalar();

                //Insert Dummy Park
                command = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('Yellowstone', 'Wyoming', '0001-01-01', 1, 1, 'Dummy park for testing'); SELECT CAST(SCOPE_IDENTITY() as int)", connection);
                parkId = (int)command.ExecuteScalar();
            }
        }  
        
        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod()]
        public void GetAllParksTest()
        {
            //Arrange
            ParkDAL dal = new ParkDAL(connectionString);

            //Act
            List<Park> parks = dal.GetAllParks();

            //Assert
            Assert.IsNotNull(parks);
            Assert.AreEqual(numberOfParks + 1, parks.Count);
        }

        [TestMethod()]
        public void GetParkTest()
        {
            ParkDAL dal = new ParkDAL(connectionString);

            Park park = dal.GetPark(parkId);

            Assert.IsNotNull(park);
            Assert.AreEqual("Yellowstone", park.Name);
            Assert.AreEqual("Wyoming", park.Location);
        }
    }
}
