using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;

namespace PlantsAPI.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected PlantsDbContext dbContext;
        internal DbSet<T> dbSet;
        protected readonly ILogger logger;

        public GenericRepository(PlantsDbContext dbContext, ILogger logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.dbSet = dbContext.Set<T>() ?? throw new ArgumentNullException(nameof(dbContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

    }
}
