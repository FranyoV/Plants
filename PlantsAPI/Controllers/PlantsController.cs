using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    [Route("api/plants")]
    [ApiController]
    [Authorize]
    public class PlantsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private Guid tokenId;

        public PlantsController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
            tokenId = Guid.Parse(unitOfWork.UserContext.GetMe());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
        {

            var plants = await unitOfWork.Plants.GetPlants();
            return Ok(plants);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Plant>> GetPlantById(Guid id)
        {
            var plant = await unitOfWork.Plants.GetPlantById(id);
            return Ok(plant);
        }
       

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlantsOfUser([FromRoute] Guid userId)
        {

            var plants = await unitOfWork.Plants.GetPlantsOfUser(userId);
            var context = unitOfWork.UserContext.GetMe();
            return Ok(plants);
        }


        [HttpGet]
        [Route("user/{userId}/count")]
        public async Task<ActionResult<int>> GetPlantsCount ([FromRoute] Guid userId)
        {
            var plants = await unitOfWork.Plants.GetPlantsCount(userId);
            return Ok(plants);
        }


        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant([FromBody] Plant plant)
        {
            var newPlant = await unitOfWork.Plants.AddPlant(plant);
            await unitOfWork.SaveChangesAsync();
            return Ok(newPlant);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Plant>> PutPlant([FromBody] Plant plant)
        {
            var modifiedPlant = await unitOfWork.Plants.EditPlant(plant);
            await unitOfWork.SaveChangesAsync();
            return Ok(modifiedPlant);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeletePlant(Guid id)
        {
            var result = await unitOfWork.Plants.DeletePlant(id);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }
    }
}
