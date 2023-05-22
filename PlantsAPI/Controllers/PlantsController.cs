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

        public PlantsController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork ?? throw new ArgumentNullException(nameof(unitOFWork));
        }


        //TODO AUTHORIZATION
        /* [HttpGet]
         public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
         {

             var plants = await unitOfWork.Plants.GetPlants();
             return Ok(plants);
         }*/

        
        //TODO AUTHORIZATION
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Plant>> GetPlantById(Guid id)
        {
            try
            {
                var plant = await unitOfWork.Plants.GetPlantById(id);
                return Ok(plant);
            }
            catch
            {
                return BadRequest();
            }
        }
       

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlantsOfUser([FromRoute] Guid userId)
        {
            try
            {

                    var plants = await unitOfWork.Plants.GetPlantsOfUser(userId);
                    return Ok(plants);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("user/{userId}/count")]
        public async Task<ActionResult<int>> GetPlantsCount ([FromRoute] Guid userId)
        {
            try
            {
                    var plants = await unitOfWork.Plants.GetPlantsCount(userId);
                    return Ok(plants);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant([FromBody] Plant plant)
        {
            try
            {

                    var newPlant = await unitOfWork.Plants.AddPlant(plant);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(newPlant);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Plant>> PutPlant([FromBody] Plant plant)
        {
            try
            {

                    var modifiedPlant = await unitOfWork.Plants.EditPlant(plant);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(modifiedPlant);

            }
            catch
            {
                return BadRequest();
            }
        }

        //TODO AUTHORIZATION
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeletePlant(Guid id)
        {
            try
            {
                var result = await unitOfWork.Plants.DeletePlant(id);
                await unitOfWork.SaveChangesAsync();
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
