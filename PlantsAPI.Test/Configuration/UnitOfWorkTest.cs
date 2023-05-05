
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Configuration;
using PlantsAPI.Data;
using System;
using Xunit;

namespace PlantsAPI.Test.Configuration
{
    public class UnitOfWorkTest 
    {
        protected internal Mock<IUnitOfWork> unitOfWork;
        protected internal Mock<ILoggerFactory> logger;
        protected internal Mock<IConfiguration> configuration;
        protected internal PlantsDbContext context;
        protected internal Mock<IHttpContextAccessor> httpContextAccessor;

        public UnitOfWorkTest()
        {
            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            context = new PlantsDbContext(dbOptions.Options);
            logger = new Mock<ILoggerFactory>();
            httpContextAccessor = new Mock<IHttpContextAccessor>();
            configuration = new Mock<IConfiguration>();

        }
        #region Constructor
        [Fact]
        public void ConstructorShouldCreateObject()
        {
            Assert.NotNull(new UnitOfWork(
                context,
                logger.Object,
                configuration.Object,
                httpContextAccessor.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork(
                null,
                logger.Object,
                configuration.Object,
                httpContextAccessor.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<NullReferenceException>(() => new UnitOfWork(
                context,
                null,
                configuration.Object,
                httpContextAccessor.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_3()
        {
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork(
                context,
                logger.Object,
                null,
                httpContextAccessor.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_4()
        {
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork(
                context,
                logger.Object,
                configuration.Object,
                null));
        }

        #endregion



    }
}
