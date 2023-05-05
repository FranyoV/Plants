using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Repositories;
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
        private readonly Mock<ILogger> logger;
        private readonly Mock<IItemsRepository> mockItemRepository;

        public ItemRepositoryTest()
        {

            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new PlantsDbContext(dbOptions.Options);
            logger = new Mock<ILogger>();
            mockItemRepository = new Mock<IItemsRepository>();
        }

        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new ItemsRepository(context, logger.Object));
        }

        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemsRepository(null, logger.Object));
        }

        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemsRepository(context, null));
        }
    }
}
