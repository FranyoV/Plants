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
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
            this.logger = logger;
        }

    }
}
