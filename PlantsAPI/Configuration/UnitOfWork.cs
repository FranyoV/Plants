using PlantsAPI.Data;
using PlantsAPI.Repositories;

namespace PlantsAPI.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly PlantsDbContext dbContext;
        public IUserRepository Users { get; private set; }
        public IPlantRepository Plants { get; private set; }
        public IPostRepository Posts { get; private set; }
        public IReplyRepository Replies { get; private set; }

        public UnitOfWork(PlantsDbContext plantsDbContext, 
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            this.dbContext = plantsDbContext;
            this.logger = loggerFactory.CreateLogger("logs");
            this.configuration = configuration;

            this.Users = new UserRepository(dbContext, logger);
            this.Plants = new PlantRepository(dbContext, logger);
            this.Posts = new PostRepository(dbContext, logger);
            this.Replies = new ReplyRepository(dbContext, logger);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
