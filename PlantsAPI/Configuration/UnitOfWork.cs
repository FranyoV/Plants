using PlantsAPI.Data;
using PlantsAPI.Repositories;
using PlantsAPI.Services;

namespace PlantsAPI.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
       // private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly PlantsDbContext _dbContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserContext _userContext;
        private readonly INotificationService _notificationService;

        public IPlantRepository Plants { get; private set; }
        public IPostRepository Posts { get; private set; }
        public IReplyRepository Replies { get; private set; }
        public IItemsRepository Items { get; private set; }
        public IAuthRepository Auth { get; private set; }

        //public IUserContext UserContext {get; private set; }

        //public INotificationService NotificationService { get; private set; }

        public UnitOfWork(PlantsDbContext plantsDbContext, 
            //ILoggerFactory loggerFactory,
            IConfiguration configuration,
          //  IHttpContextAccessor httpContextAccessor, 
            IUserContext userContext,
            INotificationService notificationService)
        {
            _dbContext = plantsDbContext ?? throw new ArgumentNullException(nameof(plantsDbContext));
            //_logger = loggerFactory.CreateLogger("logs") ?? throw new ArgumentNullException(nameof(loggerFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
           // _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService)); 
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext)); 

           
            this.Plants = new PlantRepository(_dbContext, _userContext);
            this.Posts = new PostRepository(_dbContext, _userContext);
            this.Replies = new ReplyRepository(_dbContext, _userContext,  _notificationService );
            this.Items = new ItemsRepository(_dbContext, _userContext);
            this.Auth = new AuthRepository(_dbContext,  _configuration, _userContext);
            //this.UserContext = new UserContext(httpContextAccessor);
           // this.NotificationService = new NotificationService(configuration);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
