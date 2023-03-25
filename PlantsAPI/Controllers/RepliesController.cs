using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepliesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RepliesController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
        }

        [HttpGet]
        [Route("replies")]
        public async Task<ActionResult<IEnumerable<Reply>>> GetPlants()
        {

            var replies = await unitOfWork.Replies.GetReplies();
            return Ok(replies);
        }

        [HttpGet]
        [Route("reply")]
        public async Task<ActionResult<Reply>> GetPlantById(Guid id)
        {
            var reply = await unitOfWork.Replies.GetReplyById(id);
            return Ok(reply);
        }

        [HttpPost]
        [Route("reply")]
        public async Task<ActionResult<Reply>> PostPlant(Reply reply)
        {
            var newReply = await unitOfWork.Replies.AddReply(reply);
            await unitOfWork.SaveChangesAsync();
            return Ok(newReply);
        }

        [HttpPut]
        [Route("reply")]
        public async Task<ActionResult<Reply>> PutPlant(Reply reply)
        {
            var modifiedReply = await unitOfWork.Replies.EditReply(reply);
            await unitOfWork.SaveChangesAsync();
            return Ok(modifiedReply);
        }

        [HttpDelete]
        [Route("reply")]
        public async Task<ActionResult<bool>> DeletePlant(Guid id)
        {
            var result = await unitOfWork.Replies.DeleteReply(id);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }
    }
}
