using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IPlantRepository
    {
        Task<IEnumerable<Plant>> GetPlants();
        Task<Plant> GetPlantById(Guid id);
        Task<IEnumerable<Plant>> GetPlantsOfUser(Guid userId);
        Task<int> GetPlantsCount(Guid userId);
        Task<Plant> AddPlant(PlantDto plant);
        Task<Plant> EditPlant(Plant plant);
        Task<bool> DeletePlant(Guid id);
    }
}
