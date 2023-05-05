using PlantsAPI.Data;
using PlantsAPI.Repositories;
using PlantsAPI.Services;

namespace PlantsAPI.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly PlantsDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public IUserRepository Users { get; private set; }
        public IPlantRepository Plants { get; private set; }
        public IPostRepository Posts { get; private set; }
        public IReplyRepository Replies { get; private set; }
        public IItemsRepository Items { get; private set; }
        public IAuthRepository Auth { get; private set; }
        public IUserContext UserContext {get; private set; }

        public UnitOfWork(PlantsDbContext _plantsDbContext, 
            ILoggerFactory _loggerFactory,
            IConfiguration _configuration,
            IHttpContextAccessor _httpContextAccessor)
        {
            this.dbContext = _plantsDbContext ?? throw new ArgumentNullException(nameof(_plantsDbContext));
            this.logger = _loggerFactory.CreateLogger("logs") ?? throw new ArgumentNullException(nameof(_loggerFactory));
            this.configuration = _configuration ?? throw new ArgumentNullException(nameof(_configuration));
            this.httpContextAccessor = _httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));

            this.Users = new UserRepository(dbContext, logger);
            this.Plants = new PlantRepository(dbContext, logger);
            this.Posts = new PostRepository(dbContext, logger);
            this.Replies = new ReplyRepository(dbContext, logger);
            this.Items = new ItemsRepository(dbContext, logger);
            this.Auth = new AuthRepository(dbContext, logger, configuration);
            this.UserContext = new UserContext(httpContextAccessor);
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
