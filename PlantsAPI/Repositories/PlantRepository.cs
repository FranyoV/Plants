using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public class PlantRepository : GenericRepository<Plant>,IPlantRepository
    {
        public PlantRepository(PlantsDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {
        }

        public async Task<IEnumerable<Plant>> GetPlants()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<Plant> GetPlantById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var result = await dbSet.Where(p => p.Id == id).FirstAsync();

            return result;
        }

        public async Task<IEnumerable<Plant>> GetPlantsOfUser(Guid userId)
        {
            if (userId == Guid.Empty ) throw new ArgumentNullException(nameof(userId));

            var result = await dbSet.Where(p => p.UserId == userId).ToListAsync();
            return result;

        }

        public async Task<int> GetPlantsCount(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Plant> result = await dbSet.Where(p => p.UserId == userId).ToListAsync();
            return result.Count;

        }


        public async Task<Plant> AddPlant(Plant plant)
        {
            if (plant == null) throw new ArgumentNullException(nameof(plant));

            plant.Id = Guid.NewGuid();  

            if(plant.LastNotification != null && (plant.Interval != null || plant.Interval != 0) )
            {
                plant.NextNotification = plant.LastNotification.Value.AddDays((double)plant.Interval);
            }
           
            var result = dbSet.Add(plant);
            return result.Entity;
        }

        public async Task<Plant> EditPlant(Plant plant)
        {
            if (plant == null) throw new ArgumentNullException(nameof(plant));
            
            Plant originalPlant = await dbSet.FirstAsync(p => p.Id == plant.Id);

            if (originalPlant != null)
            {
                originalPlant.ImageUrl = plant.ImageUrl;
                originalPlant.Name = plant.Name;
                originalPlant.Description = plant.Description;
                originalPlant.ImageUrl = plant.ImageUrl;
                originalPlant.Note = plant.Note;
                originalPlant.Interval = plant.Interval;

                if (plant.LastNotification != null)
                {
                    originalPlant.LastNotification = plant.LastNotification;

                    if (plant.LastNotification < DateTime.Now)
                    {
                        originalPlant.NextNotification = DateTime.Now;
                    }
                    else
                    {
                        originalPlant.NextNotification = originalPlant.LastNotification.Value.AddDays(originalPlant.Interval.Value);
                    }
                }

            }

            return (originalPlant != null) ? originalPlant : new Plant();
        }


        public async Task<bool> DeletePlant(Guid plantId)
        {
            if (plantId == Guid.Empty) throw new NotImplementedException();

            var toBeDeleted = await dbSet.Where(p => p.Id == plantId).FirstAsync();
            if (toBeDeleted != null)
            {
                var result = dbSet.Remove(toBeDeleted);
                return result.State == EntityState.Deleted;
            }

            return false;
        }

    }
}
