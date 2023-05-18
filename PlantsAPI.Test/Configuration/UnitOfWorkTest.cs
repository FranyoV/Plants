
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Configuration;
using PlantsAPI.Data;
using PlantsAPI.Services;
using System;
using Xunit;

namespace PlantsAPI.Test.Configuration
{
    public class UnitOfWorkTest 
    {
       

        #region Constructor
        [Fact]
        public void ConstructorShouldCreateObject()
        {
            TestHelper helper = new();

            Assert.NotNull(new UnitOfWork(
                helper.context,
               // helper.logger,
                helper.configuration.Object,
                //helper.httpContextAccessor.Object,
                helper.userContext.Object,
                helper.notificationService.Object
                ));
        }
        
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_1()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork(
                 null,
                // helper.logger,
                helper.configuration.Object,
               // helper.httpContextAccessor.Object,
                helper.userContext.Object,
                helper.notificationService.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_2()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork(
                 helper.context,
                // helper.logger,
                null,
               // helper.httpContextAccessor.Object,
                helper.userContext.Object,
                helper.notificationService.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_3()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork(
                 helper.context,
                // helper.logger,
                helper.configuration.Object,
              //  null,
               null,
                helper.notificationService.Object));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_4()
        {
            TestHelper helper = new();
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork(
                 helper.context,
                // helper.logger,
                helper.configuration.Object,
               // helper.httpContextAccessor.Object,
                helper.userContext.Object,
                null));
        }
        
        #endregion

        private class TestHelper
        {
            
            protected internal Mock<IUnitOfWork> unitOfWork;
           // protected internal ILoggerFactory logger;
            protected internal Mock<IConfiguration> configuration;
            protected internal PlantsDbContext context;
            protected internal Mock<IHttpContextAccessor> httpContextAccessor;
            protected internal Mock<INotificationService> notificationService;
            protected internal Mock<IUserContext> userContext;

            public TestHelper()
            {
                DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString());
                

                context = new PlantsDbContext(dbOptions.Options);
                //logger = new ILoggerFactory();
                httpContextAccessor = new Mock<IHttpContextAccessor>();
                configuration = new Mock<IConfiguration>();
                userContext = new Mock<IUserContext>();
                notificationService = new Mock<INotificationService>();
                
                unitOfWork = new Mock<IUnitOfWork>();
                
            }
        }

        [Fact]
        public void SaveChangesAsync_ShouldWork()
        {
            TestHelper helper = new();

            helper.unitOfWork.Setup(
                x => x.SaveChangesAsync());
        }


        [Fact]
        public void Dispose()
        {
            TestHelper helper = new();

            helper.unitOfWork.Setup(
                x => x.Dispose());
        }



    }
}
