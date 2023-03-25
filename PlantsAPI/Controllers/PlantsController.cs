using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PlantsController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
        }

        [HttpGet]
        [Route("plants")]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
        {

            var plants = await unitOfWork.Plants.GetPlants();
            return Ok(plants);
        }

        [HttpGet]
        [Route("plant")]
        public async Task<ActionResult<Plant>> GetPlantById(Guid id)
        {
            var plant = await unitOfWork.Plants.GetPlantById(id);
            return Ok(plant);
        }

        [HttpPost]
        [Route("plant")]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
            var newPlant = await unitOfWork.Plants.AddPlant(plant);
            await unitOfWork.SaveChangesAsync();
            return Ok(newPlant);
        }

        [HttpPut]
        [Route("plant")]
        public async Task<ActionResult<Plant>> PutPlant(Plant plant)
        {
            var modifiedPlant = await unitOfWork.Plants.EditPlant(plant);
            await unitOfWork.SaveChangesAsync();
            return Ok(modifiedPlant);
        }

        [HttpDelete]
        [Route("plant")]
        public async Task<ActionResult<bool>> DeletePlant(Guid id)
        {
            var result = await unitOfWork.Plants.DeletePlant(id);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }
    }
}
