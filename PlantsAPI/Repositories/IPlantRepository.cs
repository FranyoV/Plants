using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IPlantRepository : IGenericRepository
    {
        Task<IEnumerable<Plant>> GetPlants();
        Task<Plant> GetPlantById(Guid id);
        Task<IEnumerable<Plant>> GetPlantsOfUser(Guid userId);
        Task<int> GetPlantsCount(Guid userId);
        Task<Plant> AddPlant(Plant plant);
        Task<Plant> AddImageToPlant(Guid id, byte[] image);
        Task<Plant> EditPlant(Plant plant);
        Task<bool> DeletePlant(Guid id);
    }
}
