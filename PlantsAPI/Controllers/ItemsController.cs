using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    [Route("api/items")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ItemsController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
        }

        //TODO AUTHORIZATION
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var items = await unitOfWork.Items.GetItems();
            return Ok(items);
        }

        //TODO AUTHORIZATION
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Item>> GetItemById(Guid id)
        {
            var item = await unitOfWork.Items.GetItemById(id);
            return Ok(item);
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsOfUser([FromRoute] Guid userId)
        {
            if (unitOfWork.UserContext.HasAuthorization(userId))
            {
                var items = await unitOfWork.Items.GetItemsOfUser(userId);
                return Ok(items);
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("user/{userId}/count")]
        public async Task<ActionResult<int>> GetItemsCount([FromRoute] Guid userId)
        {
            if (unitOfWork.UserContext.HasAuthorization(userId))
            {
                var items = await unitOfWork.Plants.GetPlantsCount(userId);
                return Ok(items);
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<ActionResult<Item>> PostPlant([FromBody] Item item)
        {
            if (unitOfWork.UserContext.HasAuthorization(item.UserId))
            {
                var newItem = await unitOfWork.Items.AddItem(item);
                await unitOfWork.SaveChangesAsync();
                return Ok(newItem);
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Item>> PutItem([FromBody] Item item)
        {
            if (unitOfWork.UserContext.HasAuthorization(item.UserId))
            {
                var modifiedPlant = await unitOfWork.Items.EditItem(item);
                await unitOfWork.SaveChangesAsync();
                return Ok(modifiedPlant);

            }
            return Unauthorized();
        }

        //TODO AUTHORIZATION
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeleteItem(Guid id)
        {
            var result = await unitOfWork.Items.DeleteItem(id);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }

    }
}
