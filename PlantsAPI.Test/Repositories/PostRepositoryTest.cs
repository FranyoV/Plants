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
    public class PostRepositoryTest
    {
        private readonly PlantsDbContext context;
        private readonly Mock<ILogger> logger;
        private readonly Mock<IPostRepository> mockPostRepository;

        public PostRepositoryTest()
        {

            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            context = new PlantsDbContext(dbOptions.Options);
            logger = new Mock<ILogger>();
            mockPostRepository = new Mock<IPostRepository>();
        }

        [Fact]
        public void ContructorShouldCreateObject()
        {
            Assert.NotNull(new PostRepository(context, logger.Object));
        }
    }
}
