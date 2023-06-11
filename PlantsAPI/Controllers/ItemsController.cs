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
                    var items = await unitOfWork.Items.GetItemsOfUser(userId);
                    return Ok(items);

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
                var items = await unitOfWork.Items.GetItemsCount(userId);
                return Ok(items);
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
                var newItem = await unitOfWork.Items.AddItem(item);
                await unitOfWork.SaveChangesAsync();
                return Ok(newItem);
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
                var modifiedItem = await unitOfWork.Items.EditItem(item);
                await unitOfWork.SaveChangesAsync();
                return Ok(modifiedItem);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("{itemId}/image")]
        public async Task<ActionResult<ItemDto>> AddImage([FromForm] IFormFile image, [FromRoute] Guid itemId)
        {
            try
            {
                ItemDto item = await unitOfWork.Items.AddImageToItem(itemId, image);
                await unitOfWork.SaveChangesAsync();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



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
