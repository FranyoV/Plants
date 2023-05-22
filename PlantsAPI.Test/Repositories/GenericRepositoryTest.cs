using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using PlantsAPI.Services;
using System;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    //implementing IDisposable needed?
    
    public class GenericRepositoryTest 
    {
        private readonly PlantsDbContext context;
        private readonly Mock<IUserContext> userContext;
        private readonly Mock<IGenericRepository> mockGenericRepository;
        private readonly Mock<IConfiguration> configuration;
       
        public GenericRepositoryTest()
        {
            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            configuration = new Mock<IConfiguration>();
            context = new PlantsDbContext(dbOptions.Options);
            userContext = new Mock<IUserContext>();
            mockGenericRepository = new Mock<IGenericRepository>();
        }

        [Fact]
        public void Constructor_ShouldCreateObject()
        {
            Assert.NotNull(new GenericRepository<Plant>(context, userContext.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new GenericRepository<Plant>(null, userContext.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new GenericRepository<Plant>(context, null));
        }
    }

}
