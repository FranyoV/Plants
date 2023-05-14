using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlantsAPI.Configuration;
using PlantsAPI.Controllers;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using System;
using System.Net;
using Xunit;

namespace PlantsAPI.Test.Controllers
{
    public class UserAccountControllerTest
    {
        #region Constructor
        [Fact]
        public void ConstructorShouldCreateObject()
        {
            TestHelper helper = new();
            Assert.NotNull(new UserAccountController(helper.mockUnitOfWork.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_1()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new UserAccountController(null));
        }

        #endregion

        #region Register

        [Fact]
        public void Register_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.Register(null));
        }


        [Fact] 
        public void Register_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.Auth.GenerateSalt(It.IsAny<int>()))
                .Returns(It.IsAny<string>());

            helper.mockUnitOfWork.Setup(
                x => x.Auth.CreatePasswordHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            helper.mockUnitOfWork.Setup(
                x => x.Auth.AddUser(It.IsAny<User>()));
                

            var response = helper.Controller.Register(new RegisterRequest());

            Assert.True((response?.Result as OkResult).StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public void Register_ShouldReturnBadRequest()
        {
            TestHelper helper = new();

            var response = helper.Controller.Register(new RegisterRequest());

            Assert.True((response.Result as BadRequestResult).StatusCode == (int)HttpStatusCode.BadRequest);
        }
        #endregion


        #region Login

        [Fact]
        public void Login_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.Login(null));
        }


        [Fact]
        public void Login_ShouldReturnOkWithUserNotFound()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.Auth.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<User>);


            var response = helper.Controller.Login(new LoginRequest());

            //Assert.True(response?.Result.Result.ToString() == "UserNotFound");
            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public void Login_ShouldReturnOkWithWrongPassword()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.Auth.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<User>);

            helper.mockUnitOfWork.Setup(
                x => x.Auth.CreatePasswordHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<string>());


            var response = helper.Controller.Login(new LoginRequest());

            //Assert.True(response?.Result.Result.ToString() == "UserNotFound");
            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact]
        public void Login_ShouldReturnOkWithResponse()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                x => x.Auth.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<User>);

            helper.mockUnitOfWork.Setup(
                x => x.Auth.CreatePasswordHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<string>);

            helper.mockUnitOfWork.Setup(
                x => x.Auth.CreateToken(It.IsAny<User>()))
                .Returns(It.IsAny<string>());

            var response = helper.Controller.Login(new LoginRequest());

            //Assert.True(response?.Result.Result.ToString() == "UserNotFound");
            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }

        #endregion



        private class TestHelper
        {
            protected internal Mock<IAuthRepository> mockAuthRepository;
            protected internal UserAccountController Controller;
            protected internal Mock<IUnitOfWork> mockUnitOfWork;
            protected internal Mock<IHttpContextAccessor> mockHttpContextAccesor;

            public TestHelper()
            {
                mockAuthRepository = new Mock<IAuthRepository>();
                mockUnitOfWork = new Mock<IUnitOfWork>();
                mockHttpContextAccesor = new Mock<IHttpContextAccessor>();
                Controller = new(mockUnitOfWork.Object);
            }
        }
    }
}
