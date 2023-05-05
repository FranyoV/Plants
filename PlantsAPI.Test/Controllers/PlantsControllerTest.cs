using Xunit;
using PlantsAPI.Controllers;
using PlantsAPI.Repositories;
using Moq;
using PlantsAPI.Configuration;
using System;
using PlantsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Collections.Generic;

namespace PlantsAPI.Test
{

    public class PlantsControllerTest
    {

        #region Constructor
        [Fact]
        public void ConstructorShouldCreateObject()
        {
            TestHelper helper = new();
            Assert.NotNull(new PlantsController(helper.mockUnitOfWork.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new PlantsController(null));
        }
        #endregion

        #region GetByPlantId

        [Fact]
        public void GetPlantById_ShouldReturnResult()
        {
            TestHelper helper = new();
            helper.mockUnitOfWork.Setup(
                x => x.Plants.GetPlantById(It.IsAny<Guid>()))
                .ReturnsAsync(new Plant());

            var response = helper.Controller.GetPlantById(Guid.NewGuid());

            Assert.True((response.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }
        #endregion


        #region
        [Fact]
        public void GetPlantByUser_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => 
            helper.Controller.GetPlantsOfUser(Guid.Empty));
        }


        [Fact]
        public void GetPlantByUser_ShouldReturnUnathorized()
        {
            Plant plant = new()
            {
                Id = Guid.NewGuid(),
                Name = "testName",
            };

            IEnumerable<Plant> plantsResult = new List<Plant>() { plant };

            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

           /* helper.mockUnitOfWork.Setup(
                x => x.Plants.GetPlantsOfUser(It.IsAny<Guid>()))
                .ReturnsAsync(plantsResult);*/

            var response = helper.Controller.GetPlantsOfUser(Guid.NewGuid())?.Result;

            Assert.True((response?.Result as ObjectResult).StatusCode == (int)HttpStatusCode.Unauthorized);
        }



        [Fact]
        public void GetPlantByUser_ShouldReturnResult()
        {
            Plant plant = new()
            {
                Id = Guid.NewGuid(),
                Name = "testName",

            };
            IEnumerable<Plant> plantsResult = new List<Plant>() { plant };

            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Plants.GetPlantsOfUser(It.IsAny<Guid>()))
                .ReturnsAsync(plantsResult);

            var response = helper.Controller.GetPlantsOfUser(Guid.NewGuid())?.Result;

            Assert.True((response?.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }
        #endregion



        #region GetPlantsCount
        [Fact]
        public void GetPlantsCount_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetPlantsCount(Guid.Empty));
        }


        [Fact]
        public void GetPlantsCount_ShouldReturnResult()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Plants.GetPlantsCount(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<int>());

            var response = helper.Controller.GetPlantsCount(Guid.NewGuid())?.Result;

            Assert.True((response?.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);

        }
        #endregion


        #region PostPlant
        [Fact]
        public void PostPlant_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PostPlant(null));
        }
        #endregion


        #region PutPlant
        [Fact]
        public void PutPlant_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PutPlant( null));
        }
        #endregion


        #region DeletePlant
        [Fact]
        public void DeletePlant_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.DeletePlant(Guid.Empty));
        }
        #endregion


        private class TestHelper
        {
            protected internal Mock<IPlantRepository> mockPlantsRepository;
            protected internal PlantsController Controller;
            protected internal Mock<IUnitOfWork> mockUnitOfWork;

            public TestHelper()
            {
                mockPlantsRepository = new Mock<IPlantRepository>();
                mockUnitOfWork = new Mock<IUnitOfWork>();
                Controller = new(mockUnitOfWork.Object);
            }
        }
    }
}