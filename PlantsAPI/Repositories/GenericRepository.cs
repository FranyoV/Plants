using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Services;

namespace PlantsAPI.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected PlantsDbContext _dbContext;
        internal DbSet<T> dbSet;
        //protected readonly ILogger logger;
        protected IUserContext _userContext;

        public GenericRepository(PlantsDbContext dbContext, IUserContext userContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.dbSet = dbContext.Set<T>() ?? throw new ArgumentNullException(nameof(dbContext));
            //this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

    }
}
