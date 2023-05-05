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
{
    public class UserRepositoryTest
    {
        private readonly PlantsDbContext context;
        private readonly Mock<ILogger> logger;
        private readonly Mock<IUserRepository> mockReplyRepository;
        private readonly Mock<IConfiguration> configuration;

        public UserRepositoryTest()
        {
            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            configuration = new Mock<IConfiguration>();
            context = new PlantsDbContext(dbOptions.Options);
            logger = new Mock<ILogger>();
            mockReplyRepository = new Mock<IUserRepository>();
        }

        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new UserRepository(context, logger.Object));
        }

    }
}
