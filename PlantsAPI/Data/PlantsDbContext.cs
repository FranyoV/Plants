using Microsoft.EntityFrameworkCore;
using PlantsAPI.Models;

namespace PlantsAPI.Data
{
    public class PlantsDbContext : DbContext
    {
        public PlantsDbContext(DbContextOptions<PlantsDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null;
        public virtual DbSet<Post> Posts { get; set; } = null;
        public virtual DbSet<Reply> Replies { get; set; } = null;
        public virtual DbSet<Plants> Plants { get; set; } = null;
        
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            //TODO: finish configuration of modelbuilder for all entities
            modelbuilder.Entity<User>().ToTable("Users").HasMany(p=>p.Posts);
        }
    }
}