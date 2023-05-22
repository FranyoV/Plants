using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using PlantsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    public class ItemRepositoryTest
    {
        private readonly PlantsDbContext context;
       
        private readonly IItemsRepository itemRepository;
        private readonly Mock<IUserContext> userContext;

        public ItemRepositoryTest()
        {

            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            context = new PlantsDbContext(dbOptions.Options);
            userContext = new Mock<IUserContext>();
            itemRepository = new ItemsRepository(context, userContext.Object);
        }


        #region Constructor
        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new ItemsRepository(context, userContext.Object));
        }

        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemsRepository(null, userContext.Object));
        }

        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemsRepository(context, null));
        }
        #endregion


    //    #region GetItems
    //    [Fact]
    //    public void GetItems_ShouldReturnResult()
    //    {
    //        itemRepository.Setup(
    //            x => x.GetItems())
    //            .ReturnsAsync(It.IsAny<List<Item>>());
    //        //var itemList = itemRepository.GetItems();
    //        Assert.NotNull(itemList.Result);
    //        Assert.NotEmpty(itemList.Result);
    //    }
    //    #endregion


    //    #region GetItemById

    //    [Fact]
    //    public void GetItemById_ShouldThrowArgumentNullException()
    //    {
    //        Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.GetItemById(Guid.Empty));
    //    }

    //    #endregion

    //    #region GetItemsOfUser
    //    [Fact]
    //    public void GetItemsOfUser_ShouldThrowArgumentNullException()
    //    {
    //        Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.GetItemsOfUser(Guid.Empty));
    //    }
    //    #endregion

    //    #region GetItemsCount
    //    [Fact]
    //    public void GetItemsCount_ShouldThrowArgumentNullException()
    //    {
    //        Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.GetItemsCount(Guid.Empty));
    //    }
    //    #endregion


    //    #region AddItem

    //    [Fact]
    //    public void AddItem_ShouldThrowArgumentNullException()
    //    {
    //        Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.AddItem(null));
    //    }

    //    #endregion

    //    #region EditItem

    //    [Fact]
    //    public void EditItem_ShouldThrowArgumentNullException()
    //    {
    //        Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.EditItem(null));
    //    }

    //    #endregion

    //    #region  DeleteItem

    //    [Fact]
    //    public void DeleteItem_ShouldThrowArgumentNullException()
    //    {
    //        Assert.ThrowsAsync<ArgumentNullException>(() => itemRepository.DeleteItem(Guid.Empty));
    //    }

    //    #endregion
    }
}
