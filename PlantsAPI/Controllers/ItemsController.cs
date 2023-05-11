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
            this.unitOfWork = unitOFWork ?? throw new ArgumentNullException(nameof(unitOFWork));
        }

        //TODO AUTHORIZATION
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            try
            {
                var items = await unitOfWork.Items.GetItems();
                return Ok(items);
            }
            catch
            {
                return BadRequest();
            }

        }

        //TODO AUTHORIZATION
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Item>> GetItemById(Guid id)
        {
            try
            {
                var item = await unitOfWork.Items.GetItemById(id);
                return Ok(item);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsOfUser([FromRoute] Guid userId)
        {
            try
            {
                if (unitOfWork.UserContext.HasAuthorization(userId))
                {
                    var items = await unitOfWork.Items.GetItemsOfUser(userId);
                    return Ok(items);
                }
                return Unauthorized();
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("user/{userId}/count")]
        public async Task<ActionResult<int>> GetItemsCount([FromRoute] Guid userId)
        {
            try
            {
                if (unitOfWork.UserContext.HasAuthorization(userId))
                {
                    var items = await unitOfWork.Plants.GetPlantsCount(userId);
                    return Ok(items);
                }
                return Unauthorized();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Item>> PostItem([FromBody] Item item)
        {
            try
            {
                if (unitOfWork.UserContext.HasAuthorization(item.UserId))
                {
                    var newItem = await unitOfWork.Items.AddItem(item);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(newItem);
                }
                return Unauthorized();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Item>> PutItem([FromBody] Item item)
        {
            try
            {
                if (unitOfWork.UserContext.HasAuthorization(item.UserId))
                {
                    var modifiedPlant = await unitOfWork.Items.EditItem(item);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(modifiedPlant);

                }
                return Unauthorized();
            }
            catch
            {
                return BadRequest();
            }
        }

        //TODO AUTHORIZATION
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeleteItem(Guid id)
        {
            try
            {
                var result = await unitOfWork.Items.DeleteItem(id);
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
