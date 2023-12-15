using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using WebAPI.Services;
using WebAPI.Models;
using WebAPI.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Tests
{
    [TestClass()]
    public class TripsControllerTests
    {

        [TestMethod()]
        public void GetPublic_Ok()
        {
            Mock<TripsService> serviceMock = new Mock<TripsService>();
            Mock<TripsController> controllerMock = new Mock<TripsController>(serviceMock.Object) { CallBase = true };


            List<Trip> publicTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 1,
                    Title = "Test",
                    IsPublic = true,
                    Users = new List<DemoUser>()
                }
            };

            List<Trip> privateTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 2,
                    Title = "Test",
                    IsPublic = false,
                    Users = new List<DemoUser>()
                }
            };

            string wow = null;

            serviceMock.Setup(p => p.GetPublicTrips()).ReturnsAsync(publicTrips);

            serviceMock.Setup(p2 => p2.GetUserTrips(It.IsAny<string>())).ReturnsAsync(privateTrips);

            controllerMock.Setup(t => t.UserId).Returns(wow);

            var actionResult = controllerMock.Object.GetTrips();

            var result = actionResult.Result;

            GetTripsDTO tripResult = (GetTripsDTO)result.Value;

            Assert.AreEqual(publicTrips, tripResult.PublicTrips);
            Assert.AreEqual(0, tripResult.UserTrips.Count);
        }


        [TestMethod()]
        public void GetPrivate_Ok()
        {
            Mock<TripsService> serviceMock = new Mock<TripsService>();
            Mock<TripsController> controllerMock = new Mock<TripsController>(serviceMock.Object) { CallBase = true };


            List<Trip> publicTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 1,
                    Title = "Test",
                    IsPublic = true,
                    Users = new List<DemoUser>()
                }
            };

            List<Trip> privateTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 2,
                    Title = "Test",
                    IsPublic = false,
                    Users = new List<DemoUser>()
                }
            };

            serviceMock.Setup(p => p.GetPublicTrips()).ReturnsAsync(publicTrips);

            serviceMock.Setup(p2 => p2.GetUserTrips(It.IsAny<string>())).ReturnsAsync(privateTrips);

            controllerMock.Setup(t => t.UserId).Returns("215121");

            var actionResult = controllerMock.Object.GetTrips();

            var result = actionResult.Result;

            GetTripsDTO tripResult = (GetTripsDTO)result.Value;

            Assert.AreEqual(privateTrips, tripResult.UserTrips);
            Assert.AreEqual(publicTrips, tripResult.PublicTrips);
        }

        [TestMethod]
        public void Share_NotUserTrip()
        {
            Mock<TripsService> serviceMock = new Mock<TripsService>();
            Mock<TripsController> controllerMock = new Mock<TripsController>(serviceMock.Object) { CallBase = true };

            List<Trip> publicTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 1,
                    Title = "Test",
                    IsPublic = true,
                    Users = new List<DemoUser>()
                }
            };

            List<Trip> privateTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 2,
                    Title = "Test",
                    IsPublic = false,
                    Users = new List<DemoUser>()
                }
            };

            serviceMock.Setup(p => p.ShareTrip(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Throws(new NotUserTripException());
 

            var actionResult = controllerMock.Object.ShareTrip(1, new ShareTripDTO());

            var result = actionResult.Result;

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Share_TripNotFound()
        {
            Mock<TripsService> serviceMock = new Mock<TripsService>();
            Mock<TripsController> controllerMock = new Mock<TripsController>(serviceMock.Object) { CallBase = true };

            List<Trip> publicTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 1,
                    Title = "Test",
                    IsPublic = true,
                    Users = new List<DemoUser>()
                }
            };

            List<Trip> privateTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 2,
                    Title = "Test",
                    IsPublic = false,
                    Users = new List<DemoUser>()
                }
            };

            serviceMock.Setup(p => p.ShareTrip(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Throws(new TripNotFoundException());

            var actionResult = controllerMock.Object.ShareTrip(1, new ShareTripDTO());

            var result = actionResult.Result;

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Share_UserNotFound()
        {
            Mock<TripsService> serviceMock = new Mock<TripsService>();
            Mock<TripsController> controllerMock = new Mock<TripsController>(serviceMock.Object) { CallBase = true };

            List<Trip> publicTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 1,
                    Title = "Test",
                    IsPublic = true,
                    Users = new List<DemoUser>()
                }
            };

            List<Trip> privateTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 2,
                    Title = "Test",
                    IsPublic = false,
                    Users = new List<DemoUser>()
                }
            };

            serviceMock.Setup(p => p.ShareTrip(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).Throws(new UserNotFoundException());


            var actionResult = controllerMock.Object.ShareTrip(1, new ShareTripDTO());

            var result = actionResult.Result;

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Share_Ok()
        {
            Mock<TripsService> serviceMock = new Mock<TripsService>();
            Mock<TripsController> controllerMock = new Mock<TripsController>(serviceMock.Object) { CallBase = true };

            List<Trip> publicTrips = new List<Trip>
            {
                new Trip
                {
                    Id = 1,
                    Title = "Test",
                    IsPublic = true,
                    Users = new List<DemoUser>()
                }
            };

            Trip t = new Trip()
            {
                Id = 2,
                Title = "Test",
                IsPublic = false,
                Users = new List<DemoUser>()
            };

            serviceMock.Setup(p => p.ShareTrip(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(t);


            var actionResult = controllerMock.Object.ShareTrip(1, new ShareTripDTO());

            var result = actionResult.Result;

            Assert.IsNotNull(result);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}