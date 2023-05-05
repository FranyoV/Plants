using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Repositories;
using System;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    //implementing IDisposable needed?
    public class GenericRepositoryTest<T> where T : class 
    {
        private readonly PlantsDbContext context;
        private readonly DbSet<T> dbSet;
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
            dbSet = context.Set<T>();
        }

        [Fact]
        public void Constructor_ShouldCreateObject()
        {
            Assert.NotNull(new GenericRepository<T>(context, logger.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new GenericRepository<T>(null, logger.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new GenericRepository<T>(context, null));
        }
    }

}
