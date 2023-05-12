using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
{/*
    public class AuthRepositoryTest
    {
        private readonly PlantsDbContext context;
        private readonly Mock<ILogger> logger;
        private readonly Mock<IAuthRepository> mockAuthRepository;
        private readonly Mock<IConfiguration> configuration;

        public AuthRepositoryTest()
        {
            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            configuration = new Mock<IConfiguration>();
            context = new PlantsDbContext(dbOptions.Options);
            logger = new Mock<ILogger>();
            mockAuthRepository = new Mock<IAuthRepository>();
        }

        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new AuthRepository(context, logger.Object, configuration.Object));
        }

        [Fact]
        public void Contructor_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AuthRepository(context, logger.Object, null));
        }
    }*/
}
