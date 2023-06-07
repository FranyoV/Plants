using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Services;
using System.Drawing;

namespace PlantsAPI.Repositories
{
    public class PlantRepository : GenericRepository<Plant>,IPlantRepository
    {

        public PlantRepository(PlantsDbContext dbContext, IUserContext userContext) : base(dbContext, userContext)
        {
        }

        public async Task<IEnumerable<Plant>> GetPlants()
        {
            return await dbSet.ToListAsync();
        }


        //throws unathourized exception
        public async Task<Plant> GetPlantById(Guid plantId)
        {
            if (plantId == Guid.Empty) throw new ArgumentNullException(nameof(plantId));

            Guid currentUserId = Guid.Parse(_userContext.GetMe()); ;

            var isPlantUsers = dbSet.Where(p => p.Id == plantId).Any(up => up.UserId == currentUserId);


            if ( isPlantUsers )
            {
                var result = await dbSet.Where(p => p.Id == plantId).FirstAsync();

                return result;
            }
            else 
            {
                throw new UnauthorizedAccessException();
            }
            
        }

        //returns empty list
        public async Task<IEnumerable<Plant>> GetPlantsOfUser(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Plant> plants = new();

            if (_userContext.HasAuthorization(userId))
            {
                plants = await dbSet.Where(p => p.UserId == userId).ToListAsync();
            }
            return plants;
        }

        //returns 0
        public async Task<int> GetPlantsCount(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Plant> result = new();

            if (_userContext.HasAuthorization(userId))
            {
                result = await dbSet.Where(p => p.UserId == userId).ToListAsync();
            }

            return result.Count;

        }

        //throws unathourized exception
        public async Task<Plant> AddPlant(PlantDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (_userContext.HasAuthorization(request.UserId))
            {
                
                Plant newPlant = new();
                newPlant.Id = Guid.NewGuid();
                newPlant.Name = request.Name;
                newPlant.Description = request.Description;
                newPlant.UserId = request.UserId;

                if (request.LastNotification.HasValue && request.Interval.HasValue)
                {

                    if (request.LastNotification.Value.AddDays(request.Interval.Value) < DateTime.Now)
                    {
                        newPlant.NextNotification = DateTime.Now;
                    }
                    else
                    {
                        newPlant.NextNotification = request.LastNotification.Value.AddDays(request.Interval.Value);
                    }

                }
                /*
                if (image != null && image.Length > 0)
                {
                    byte[] imageByteArray = null;
                    using (var readStream = image.OpenReadStream())
                    using (var memoryStream = new MemoryStream())
                    {
                        readStream.CopyTo(memoryStream);
                        imageByteArray = memoryStream.ToArray();
                    }
                    newPlant.ImageUrl = imageByteArray;
                }*/
               /* if (request.ImageUrl != null)
                {
                    Image image;
                    using (var ms = new MemoryStream())
                    {
                        request.ImageUrl.Save(ms, request.ImageUrl.RawFormat);
                        newPlant.ImageUrl = ms.ToArray();
                    }
                    
                }*/

                var result = await dbSet.AddAsync(newPlant);
                return result.Entity;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }



        public async Task<Plant> EditPlant(Plant plant)
        {
            if (plant == null) throw new ArgumentNullException(nameof(plant));

            if (_userContext.HasAuthorization(plant.UserId))
            {
                Plant originalPlant = new();
                originalPlant = await dbSet.FirstAsync(p => p.Id == plant.Id);

                if (originalPlant != null)
                {

                    if (plant.Name != null)
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
            else
            {
                throw new UnauthorizedAccessException();
            }

        }


        public async Task<bool> DeletePlant(Guid plantId)
        {
            if (plantId == Guid.Empty) throw new ArgumentNullException(nameof(plantId));

            Guid currentUserId = Guid.Parse(_userContext.GetMe()); ;

            var isPlantUsers = dbSet.Where(p => p.Id == plantId).Any(up => up.UserId == currentUserId);
            
            if (isPlantUsers)
            {
                var toBeDeleted = await dbSet.Where(p => p.Id == plantId).FirstAsync();
                if (toBeDeleted != null)
                {
                    var result = dbSet.Remove(toBeDeleted);
                    return result.State == EntityState.Deleted;
                }
            }
            return false;
        }
    }
}
