using Microsoft.EntityFrameworkCore;
using Moq;
using SD_125_W22SD_Lab2.Data;
using SD_125_W22SD_Lab2.Models;

namespace TestProject2
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            var PassTestData = new List<Pass>
            {
                new Pass{ID = 1,Capacity=3,Premium=false,Purchaser="Lin", Vehicles = new List<Vehicle>()},
                new Pass{ID = 2,Capacity=3,Premium=false,Purchaser="Lin2", Vehicles = new List<Vehicle>()},
                new Pass{ID = 3,Capacity=4,Premium=false,Purchaser="Lin3", Vehicles = new List<Vehicle>()},
                new Pass{ID = 4,Capacity=4,Premium=true,Purchaser="Lin4", Vehicles = new List<Vehicle>()},
                new Pass{ID = 5,Capacity=5,Premium=true,Purchaser="Lin5", Vehicles = new List<Vehicle>()},
            }.AsQueryable();

            var VehicleTestData = new List<Vehicle>
            {
                new Vehicle{ID = 1,Licence = "Licence1",Parked = false,Pass = PassTestData.First(p => p.ID ==1),PassID = 1,Reservations = new List<Reservation>()},
                new Vehicle{ID = 2,Licence = "Licence2",Parked = false,Pass = PassTestData.First(p => p.ID ==2),PassID = 2,Reservations = new List<Reservation>()},
                new Vehicle{ID = 3,Licence = "Licence3",Parked = false,Pass = PassTestData.First(p => p.ID ==3),PassID = 3,Reservations = new List<Reservation>()},
                new Vehicle{ID = 4,Licence = "Licence4",Parked = true,Pass = PassTestData.First(p => p.ID ==4),PassID = 4,Reservations = new List<Reservation>()},
                new Vehicle{ID = 5,Licence = "Licence5",Parked = true,Pass = PassTestData.First(p => p.ID ==5),PassID = 5,Reservations = new List<Reservation>()},
            }.AsQueryable();

            var ReservationTestData = new List<Reservation>
            {
                new Reservation{ID = 1, Expiry = new DateTime(2023,1,1), IsCurrent = false, ParkingSpotID = 1, VehicleID = 1},
                new Reservation{ID = 2, Expiry = new DateTime(2023,1,2), IsCurrent = false, ParkingSpotID = 2, VehicleID = 2},
                new Reservation{ID = 3, Expiry = new DateTime(2023,1,3), IsCurrent = false, ParkingSpotID = 3, VehicleID = 3},
                new Reservation{ID = 4, Expiry = new DateTime(2023,1,4), IsCurrent = true, ParkingSpotID = 4, VehicleID = 4},
                new Reservation{ID = 5, Expiry = new DateTime(2023,1,5), IsCurrent = true, ParkingSpotID = 5, VehicleID = 5},
            }.AsQueryable();

            var ParkingSpotTestData = new List<ParkingSpot>
            {
                new ParkingSpot{ID = 1, Occupied = false, Reservations = new List<Reservation>() },
                new ParkingSpot{ID = 2, Occupied = false, Reservations = new List<Reservation>() },
                new ParkingSpot{ID = 3, Occupied = true, Reservations = new List<Reservation>() },
                new ParkingSpot{ID = 4, Occupied = true, Reservations = new List<Reservation>() },
                new ParkingSpot{ID = 5, Occupied = true, Reservations = new List<Reservation>() },
            }.AsQueryable();

            var mockPassSet = new Mock<DbSet<Pass>>();
            mockPassSet.As<IQueryable<Pass>>().Setup(m => m.Provider).Returns(PassTestData.Provider);
            mockPassSet.As<IQueryable<Pass>>().Setup(m => m.Expression).Returns(PassTestData.Expression);
            mockPassSet.As<IQueryable<Pass>>().Setup(m => m.ElementType).Returns(PassTestData.ElementType);
            mockPassSet.As<IQueryable<Pass>>().Setup(m => m.GetEnumerator()).Returns(PassTestData.GetEnumerator());

            var mockVehicleSet = new Mock<DbSet<Vehicle>>();
            mockVehicleSet.As<IQueryable<Vehicle>>().Setup(m => m.Provider).Returns(VehicleTestData.Provider);
            mockVehicleSet.As<IQueryable<Vehicle>>().Setup(m => m.Expression).Returns(VehicleTestData.Expression);
            mockVehicleSet.As<IQueryable<Vehicle>>().Setup(m => m.ElementType).Returns(VehicleTestData.ElementType);
            mockVehicleSet.As<IQueryable<Vehicle>>().Setup(m => m.GetEnumerator()).Returns(VehicleTestData.GetEnumerator());

            var mockParkingSpotSet = new Mock<DbSet<ParkingSpot>>();
            mockParkingSpotSet.As<IQueryable<ParkingSpot>>().Setup(m => m.Provider).Returns(ParkingSpotTestData.Provider);
            mockParkingSpotSet.As<IQueryable<ParkingSpot>>().Setup(m => m.Expression).Returns(ParkingSpotTestData.Expression);
            mockParkingSpotSet.As<IQueryable<ParkingSpot>>().Setup(m => m.ElementType).Returns(ParkingSpotTestData.ElementType);
            mockParkingSpotSet.As<IQueryable<ParkingSpot>>().Setup(m => m.GetEnumerator()).Returns(ParkingSpotTestData.GetEnumerator());

            var mockReservationSet = new Mock<DbSet<Reservation>>();
            mockReservationSet.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(ReservationTestData.Provider);
            mockReservationSet.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(ReservationTestData.Expression);
            mockReservationSet.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(ReservationTestData.ElementType);
            mockReservationSet.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(ReservationTestData.GetEnumerator());

            var mockContext = new Mock<ParkingContext>();
            mockContext.Setup(c => c.Passes).Returns(mockPassSet.Object);
            mockContext.Setup(c => c.Vehicle).Returns(mockVehicleSet.Object);
            mockContext.Setup(c => c.ParkingSpots).Returns(mockParkingSpotSet.Object);
            mockContext.Setup(c => c.Reservations).Returns(mockReservationSet.Object);
        }

        [DataRow("Lin", "Licence1")]
        [TestMethod]
        public void AddVehicleToPassTest(string passholderName, string vehicleLicence)
        {
            Pass pass = PassTestData.First(t => t.Purchaser == passholderName);
            Vehicle vehicle = PassTestData.First(t => t.Licence == vehicleLicence);
            ParkingHelper ph = new ParkingHelper();
            ph.AddVehicleToPass(passholderName, vehicleLicence);
            Assert.AreEqual(passholderName, PassTestData.First(t => t.Licence == vehicleLicence).Purchaser);
        }

        [DataRow("Lin","Licence123")]
        [TestMethod]
        public void AddVehicleToPass_WithWrongLicence_ThrowException(string passholderName, string vehicleLicence)
        {
            Assert.ThrowsException<Exception>(() => new ParkingHelper.AddVehicleToPass(passholderName, vehicleLicence));
        }

        [DataRow("Lin123", "Licence1")]
        [TestMethod]
        public void AddVehicleToPass_WithWrongPassholderName_ThrowException(string passholderName, string vehicleLicence)
        {
            Assert.ThrowsException<Exception>(() => new ParkingHelper.AddVehicleToPass(passholderName, vehicleLicence));
        }

        [DataRow("Lin", "Licence1")]
        [TestMethod]
        public void AddVehicleToPass_CapacityZero_ThrowException(string passholderName, string vehicleLicence)
        {
            Pass pass = PassTestData.First(t => t.Purchaser == passholderName);
            if(pass.Capacity == 0)
            {
                Assert.ThrowsException<Exception>(() => new ParkingHelper.AddVehicleToPass(passholderName, vehicleLicence));
            }
        }


    }
}