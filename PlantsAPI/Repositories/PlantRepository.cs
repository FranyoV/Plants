using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Services;

namespace PlantsAPI.Repositories
{
    public class PlantRepository : GenericRepository<Plant>,IPlantRepository
    {
        //private readonly IUserContext _userContext;
        private readonly INotificationService _notificationService;

        public PlantRepository(PlantsDbContext dbContext, IUserContext userContext, INotificationService notificationService) : base(dbContext, userContext)
        {
            ///_userContext = userContext;
            _notificationService = notificationService;
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
            /*
            if (_userContext.HasAuthorization(result.UserId))
            {
               
            }
            else 
            {
                return new Plant();
            }*/
            
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


            if (plant.LastNotification.HasValue && plant.Interval.HasValue)
            {
                
                if (plant.LastNotification.Value.AddDays(plant.Interval.Value) < DateTime.Now)
                {
                    plant.NextNotification = DateTime.Now;
                }
                else
                {
                    plant.NextNotification = plant.LastNotification.Value.AddDays(plant.Interval.Value);
                }
            }

            var result = await dbSet.AddAsync(plant);
            return result.Entity;
        }

        public async Task<Plant> EditPlant(Plant plant)
        {
            if (plant == null) throw new ArgumentNullException(nameof(plant));

            Plant originalPlant = new();
            originalPlant = await dbSet.FirstAsync(p => p.Id == plant.Id);

            if (originalPlant != null)
            {
                
                if ( plant.Name != null)
                {
                    originalPlant.Name = plant.Name;
                }
               
                originalPlant.Description = plant.Description;

                originalPlant.ImageUrl = plant.ImageUrl;
                

                if (plant.LastNotification != null)
                {
                    originalPlant.LastNotification = plant.LastNotification;
                    originalPlant.Note = plant.Note;
                    originalPlant.Interval = plant.Interval;

                    if (plant.LastNotification.Value.AddDays(plant.Interval.Value) < DateTime.Now)
                    {
                        originalPlant.NextNotification = DateTime.Now;
                    }
                    else
                    {
                        originalPlant.NextNotification = originalPlant.LastNotification.Value.AddDays(originalPlant.Interval.Value);
                    }
                }
                else
                {
                    originalPlant.LastNotification = null;
                    originalPlant.Note = null;
                    originalPlant.Interval = null;
                    originalPlant.NextNotification = null;
                }

            }
            return originalPlant ?? new Plant();
        }


        public async Task<bool> DeletePlant(Guid plantId)
        {
            if (plantId == Guid.Empty) throw new ArgumentNullException(nameof(plantId));

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
