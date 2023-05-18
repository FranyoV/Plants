using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
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
    public class AuthRepositoryTest
    {
        private readonly PlantsDbContext context;
        private readonly Mock<IUserContext> userContext;
        private readonly Mock<IAuthRepository> mockAuthRepository;
        private readonly Mock<IConfiguration> configuration;

        public AuthRepositoryTest()
        {
            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            configuration = new Mock<IConfiguration>();
            context = new PlantsDbContext(dbOptions.Options);
            userContext = new Mock<IUserContext>();
            mockAuthRepository = new Mock<IAuthRepository>();
        }

        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new AuthRepository(context, configuration.Object, userContext.Object));
        }


        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new AuthRepository(context, null, userContext.Object));
        }


        [Fact]
        public void Contructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new AuthRepository(context, configuration.Object, null));
        }
    }
}
