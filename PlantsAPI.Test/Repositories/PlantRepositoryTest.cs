using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Repositories;
using System;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    public class PlantRepositoryTest
    {
        private readonly PlantsDbContext context;
        private readonly Mock<ILogger> logger;
        private readonly Mock<IPlantRepository> mockPlantRepository;

        public PlantRepositoryTest()
        {

            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new PlantsDbContext(dbOptions.Options);
            logger = new Mock<ILogger>();
            mockPlantRepository = new Mock<IPlantRepository>();
        }

        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new PlantRepository(context, logger.Object));
        }
    }
}
