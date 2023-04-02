using Microsoft.EntityFrameworkCore;
using PlantsAPI.Models;

namespace PlantsAPI.Data
{
    public class PlantsDbContext : DbContext
    {
        public PlantsDbContext(DbContextOptions<PlantsDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Reply> Replies { get; set; } = null!;
        public virtual DbSet<Plant> Plants { get; set; } = null!;
        public virtual DbSet<Maintenance> Maintenances { get; set; } = null!;   

        //TODO: finish configuration of modelbuilder for all entities

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);


            //USER
            modelbuilder.Entity<User>().ToTable("Users");

            modelbuilder.Entity<User>().HasMany(p => p.Posts);

            modelbuilder.Entity<User>().HasMany(u => u.Replies);

            modelbuilder.Entity<User>().HasMany(u => u.Plants);


            //POST
            modelbuilder.Entity<Post>().ToTable("Posts")
                .HasOne(p => p.User)
                .WithMany(p => p.Posts);
                
            //REPLY
            modelbuilder.Entity<Reply>().ToTable("Replies")
                .HasOne(r => r.Post);

            //PLANT
            modelbuilder.Entity<Plant>().ToTable("Plants")
                .HasOne(p => p.User)
                .WithMany(u => u.Plants);

            //modelbuilder.Entity<Plant>().
            //MAINTENANCE
            modelbuilder.Entity<Maintenance>().ToTable("Maintenances")
                .HasOne(m => m.Plant)
                .WithOne(m => m.Maintenance);
                



        }
    }
}