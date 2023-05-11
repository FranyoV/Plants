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

namespace PlantsAPI.Test.Controllers
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
        private void GetPlantById_ShouldReturnArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetPlantById(Guid.Empty));
        }

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


        #region GetPlantByUser
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
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.GetPlantsOfUser(Guid.NewGuid())?.Result;

            Assert.True((response?.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);
        }



        [Fact]
        public void GetPlantByUser_ShouldReturnResult()
        {

            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Plants.GetPlantsOfUser(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<List<Plant>>);

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
        public void GetPlantsCount_ShouldReturnOk()
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


        [Fact]
        public void GetPlantsCount_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.GetPlantsCount(Guid.NewGuid())?.Result;

            Assert.True((response?.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);

        }
        #endregion


        #region PostPlant
        [Fact]
        public void PostPlant_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PostPlant(null));
        }

        [Fact]
        public void PostPlant_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Plants.AddPlant(It.IsAny<Plant>()))
                .ReturnsAsync(It.IsAny<Plant>());

            var response = helper.Controller.PostPlant(new Plant())?.Result;

            Assert.True((response?.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);

        }


        [Fact]
        public void PostPlant_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.PostPlant(new Plant())?.Result;

            Assert.True((response?.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);

        }
        #endregion


        #region PutPlant
        [Fact]
        public void PutPlant_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PutPlant( null));
        }

        [Fact]
        public void PutPlant_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Plants.EditPlant(It.IsAny<Plant>()))
                .ReturnsAsync(It.IsAny<Plant>());

            var response = helper.Controller.PutPlant(new Plant())?.Result;

            Assert.True((response?.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);

        }


        [Fact]
        public void PutPlant_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.PutPlant(new Plant())?.Result;

            Assert.True((response?.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);

        }
        #endregion


        #region DeletePlant
        [Fact]
        public void DeletePlant_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.DeletePlant(Guid.Empty));
        }


        [Fact]
        public void DeletePlant_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            helper.mockUnitOfWork.Setup(
                x => x.Plants.DeletePlant(It.IsAny<Guid>()))
                .ReturnsAsync(It.IsAny<bool>());

            var response = helper.Controller.DeletePlant(Guid.NewGuid())?.Result;

            Assert.True((response?.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);

        }


        [Fact]
        public void DeletePlant_ShouldReturnUnauthorized()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.UserContext.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var response = helper.Controller.DeletePlant(Guid.NewGuid())?.Result;

            Assert.True((response?.Result as UnauthorizedResult).StatusCode == (int)HttpStatusCode.Unauthorized);

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