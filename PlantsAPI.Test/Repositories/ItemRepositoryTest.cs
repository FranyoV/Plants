using Microsoft.EntityFrameworkCore;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using PlantsAPI.Services;
using System;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    public class ItemRepositoryTest
    {
        private readonly PlantsDbContext dbContext;
       
        private readonly IItemsRepository itemRepository;
        private readonly Mock<IUserContext> userContext;
        

        public ItemRepositoryTest()
        {

            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            dbContext = new PlantsDbContext(dbOptions.Options);
            DbInitializer.Initialize(dbContext);

            userContext = new Mock<IUserContext>();
            itemRepository = new ItemsRepository(dbContext, userContext.Object);
        }


        #region Constructor
        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new ItemsRepository(dbContext, userContext.Object));
        }

        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemsRepository(null, userContext.Object));
        }

        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemsRepository(dbContext, null));
        }
        #endregion


        #region GetItems
        [Fact]
        public void GetItems_ShouldReturnResult()
        { 
            var itemList = itemRepository.GetItems();

            Assert.NotNull(itemList.Result);
            Assert.NotEmpty(itemList.Result);
            Assert.Single(itemList.Result);
        }
        #endregion


        #region GetItemById

        [Fact]
        public void GetItemById_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.GetItemById(Guid.Empty));
        }


        [Fact]
        public void GetItemById_ShouldReturnResult()
        {
            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            var itemList = itemRepository.GetItems();

            Assert.NotNull(itemList.Result);
            Assert.NotEmpty(itemList.Result);
            Assert.Single(itemList.Result);
        }

        #endregion

        #region GetItemsOfUser
        [Fact]
        public void GetItemsOfUser_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.GetItemsOfUser(Guid.Empty));
        }



        [Fact]
        public void GetItemsOfUser_ShouldReturnResult()
        {
            Guid existingUser = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            var itemList = itemRepository.GetItemsOfUser(existingUser);

            Assert.NotNull(itemList.Result);
            Assert.NotEmpty(itemList.Result);
            Assert.Single(itemList.Result);
        }


        [Fact]
        public void GetItemsOfUser_ShouldReturnEmptyList()
        {
            Guid existingUser = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var itemList = itemRepository.GetItemsOfUser(existingUser);

            Assert.NotNull(itemList.Result);
            Assert.Empty(itemList.Result);

        }
        #endregion

        #region GetItemsCount
        [Fact]
        public void GetItemsCount_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.GetItemsCount(Guid.Empty));
        }


        [Fact]
        public void GetItemsCount_ShouldReturnResult()
        {
            Guid existingUser = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            var itemList = itemRepository.GetItemsCount(existingUser);

            Assert.NotNull(itemList.Result);
            Assert.Equal(1, itemList.Result);
            
        }


        [Fact]
        public void GetItemsCount_ShouldReturn0()
        {
            Guid existingUser = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            var itemList = itemRepository.GetItemsCount(existingUser);

            Assert.NotNull(itemList.Result);
            Assert.Equal(0, itemList.Result);

        }
        #endregion


        #region AddItem

        [Fact]
        public void AddItem_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.AddItem(null));
        }


        [Fact]
        public void AddItem_ShouldReturnResult()
        {
            Guid existingUser = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            var itemList = itemRepository.AddItem(new Item());

            Assert.NotNull(itemList.Result);
          
        }


        [Fact]
        public void AddItem_ShouldThrowsUnAuthorizedException()
        {
            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => itemRepository.AddItem(new Item()));
        }
        #endregion

        #region EditItem

        [Fact]
        public void EditItem_ShouldThrowArgumentNullException()
        {
            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.EditItem(null));
        }


        [Fact]
        public void EditItem_ShouldReturnResult()
        {
            Item modifiedItem = new Item
            {
                Id = Guid.Parse("f3cef2b2-97d4-4dff-972b-17934ddbb129"),
                Name = "Cactus",
                Type = ItemType.PLANT,
                Price = 50,
                Date = DateTime.Now,
                Sold = false,
                UserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab")
            };

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(true);

            var itemList = itemRepository.EditItem(modifiedItem);

            Assert.NotNull(itemList.Result);

        }


        [Fact]
        public void EditItem_ShouldThrowsUnAuthorizedException()
        {
            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => itemRepository.EditItem(new Item()));
        }

        #endregion

        #region  DeleteItem

        [Fact]
        public void DeleteItem_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.DeleteItem(Guid.Empty));
        }

        [Fact]
        public void DeleteItem_ShouldReturnTrue()
        {
            Guid usersItem = Guid.Parse("f3cef2b2-97d4-4dff-972b-17934ddbb129");
            Guid loggedInUser = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.GetMe())
                .Returns(loggedInUser.ToString());

            Assert.True(itemRepository.DeleteItem(usersItem).Result);
        }

        [Fact]
        public void DeleteItem_ShouldReturnFalse()
        {
            Guid existingItem = Guid.Parse("f3cef2b2-97d4-4dff-972b-17934ddbb129");
            Guid notLoggedInUser = Guid.Parse("8a02b8d4-570d-404f-946e-40b4524727ab");

            userContext.Setup(
                x => x.GetMe())
                .Returns(notLoggedInUser.ToString());

            Assert.False(itemRepository.DeleteItem(existingItem).Result);

        }

        #endregion
    }
}
