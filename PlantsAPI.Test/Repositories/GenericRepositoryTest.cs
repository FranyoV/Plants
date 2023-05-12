using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using System;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    //implementing IDisposable needed?
    /*
    public class GenericRepositoryTest 
    {
        private readonly PlantsDbContext context;
        private readonly Mock<ILogger> logger;
        private readonly Mock<IGenericRepository> mockGenericRepository;
        private readonly Mock<IConfiguration> configuration;
       
        public GenericRepositoryTest()
        {
            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            configuration = new Mock<IConfiguration>();
            context = new PlantsDbContext(dbOptions.Options);
            logger = new Mock<ILogger>();
            mockGenericRepository = new Mock<IGenericRepository>();
        }

        [Fact]
        public void Constructor_ShouldCreateObject()
        {
            Assert.NotNull(new GenericRepository<Plant>(context, logger.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new GenericRepository<Plant>(null, logger.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new GenericRepository<Plant>(context, null));
        }
    }*/

}
