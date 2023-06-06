using Microsoft.AspNetCore.Mvc;
using Moq;
using PlantsAPI.Configuration;
using PlantsAPI.Controllers;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace PlantsAPI.Test.Controllers
{
    public class ItemsControllerTest
    {
        #region Constructor
        [Fact]
        public void ConstructorShouldCreateObject()
        {
            TestHelper helper = new();
            Assert.NotNull(new ItemsController(helper.mockUnitOfWork.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new ItemsController(null));
        }
        #endregion


        #region GetItems

      /*  [Fact]
        public void GetItem_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                 x => x.Items.GetItems())
                 .ReturnsAsync(It.IsAny<List<Item>>);

            var response = helper.Controller.GetItems();

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }*/


        [Fact]
        public void GetItems_ShouldReturnBadRequest()
        {
            TestHelper helper = new();

            var response = helper.Controller.GetItems();

            Assert.True((response?.Result.Result as BadRequestResult).StatusCode == (int)HttpStatusCode.BadRequest);
        }
        #endregion


        #region GetItemById

        [Fact]
        public void GetItemById_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetItemById(Guid.Empty));
        }


        [Fact]
        public void GetItemById_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                 x => x.Items.GetItemById(It.IsAny<Guid>()))
                 .ReturnsAsync(It.IsAny<Item>);

            var response = helper.Controller.GetItemById(Guid.NewGuid());

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }


        [Fact]
        public void GetItemById_ShouldReturnBadRequest()
        {
            TestHelper helper = new();

            var response = helper.Controller.GetItemById(Guid.NewGuid());

            Assert.NotNull(response);
            Assert.True((response.Result.Result as BadRequestResult).StatusCode == (int)HttpStatusCode.BadRequest);

        }

        #endregion


        #region GetItemsOfUser

        [Fact]
        public void GetItemsOfUser_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetItemsOfUser(Guid.Empty));
        }


        [Fact]
        public void GetItemsOfUser_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                 x => x.Items.GetItemsOfUser(It.IsAny<Guid>()))
                 .ReturnsAsync(It.IsAny<List<Item>>);

            var response = helper.Controller.GetItemsOfUser(Guid.NewGuid());

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }


        [Fact]
        public void GetItemOfUser_ShouldReturnBadRequest()
        {
            TestHelper helper = new();

            var response = helper.Controller.GetItemsOfUser(Guid.NewGuid());

            Assert.True((response?.Result.Result as BadRequestResult).StatusCode == (int)HttpStatusCode.BadRequest);
        }
        #endregion

        #region GetItemsCount

        [Fact]
        public void GetItemsCount_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.GetItemsCount(Guid.Empty));
        }


        [Fact]
        public void GetItemsCount_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                 x => x.Items.GetItemsCount(It.IsAny<Guid>()))
                 .ReturnsAsync(It.IsAny<int>);

            var response = helper.Controller.GetItemsCount(Guid.NewGuid());

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }



        [Fact]
        public void GetItemsCount_ShouldReturnBadRequest()
        {
            TestHelper helper = new();

            var response = helper.Controller.GetItemsCount(Guid.NewGuid());

            Assert.True((response?.Result.Result as BadRequestResult).StatusCode == (int)HttpStatusCode.BadRequest);
        }
        #endregion

        #region PostItem
        [Fact]
        public void PostItem_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PostItem(null));
        }


        [Fact]
        public void PostItem_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                 x => x.Items.AddItem(It.IsAny<Item>()))
                 .ReturnsAsync(It.IsAny<Item>);

            var response = helper.Controller.PostItem(new Item());

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }


        [Fact]
        public void PostItem_ShouldReturnBadRequest()
        {
            TestHelper helper = new();

            var response = helper.Controller.PostItem(new Item());

            Assert.NotNull(response);
            Assert.True((response.Result.Result as BadRequestResult).StatusCode == (int)HttpStatusCode.BadRequest);

        }

        #endregion


        #region PutItem
        [Fact]
        public void PutItem_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.PutItem(null));
        }


        [Fact]
        public void PutItem_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                 x => x.Items.EditItem(It.IsAny<Item>()))
                 .ReturnsAsync(It.IsAny<Item>);

            var response = helper.Controller.PostItem(new Item());

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }


        [Fact]
        public void PutItem_ShouldReturnBadRequest()
        {
            TestHelper helper = new();

            var response = helper.Controller.PutItem(new Item());

            Assert.NotNull(response);
            Assert.True((response.Result.Result as BadRequestResult).StatusCode == (int)HttpStatusCode.BadRequest);

        }
        #endregion



        #region DeleteItem
        [Fact]
        public void DeleteItem_ShouldThrowArgumentNullException()
        {
            TestHelper helper = new();
            Assert.ThrowsAsync<ArgumentNullException>(() => helper.Controller.DeleteItem(Guid.Empty));
        }

        [Fact]
        public void DeleteItem_ShouldReturnOk()
        {
            TestHelper helper = new();

            helper.mockUnitOfWork.Setup(
                 x => x.Items.DeleteItem(It.IsAny<Guid>()))
                 .ReturnsAsync(It.IsAny<bool>);

            var response = helper.Controller.DeleteItem(Guid.NewGuid());

            Assert.True((response?.Result.Result as OkObjectResult).StatusCode == (int)HttpStatusCode.OK);
        }


        [Fact]
        public void DeleteItem_ShouldReturnBadRequest()
        {
            TestHelper helper = new();

            var response = helper.Controller.DeleteItem(Guid.NewGuid());

            Assert.NotNull(response);
            Assert.True((response.Result.Result as BadRequestResult).StatusCode == (int)HttpStatusCode.BadRequest);

        }

        #endregion


        private class TestHelper
        {
            protected internal Mock<IItemsRepository> mockItemsRepository;
            protected internal ItemsController Controller;
            protected internal Mock<IUnitOfWork> mockUnitOfWork;

            public TestHelper()
            {
                mockItemsRepository = new Mock<IItemsRepository>();
                mockUnitOfWork = new Mock<IUnitOfWork>();
                Controller = new(mockUnitOfWork.Object);
            }
        }
    }
}
