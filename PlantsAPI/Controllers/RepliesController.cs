using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    [Route("api/replies")]
    [ApiController]
    public class RepliesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RepliesController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reply>>> GetReply()
        {

            var replies = await unitOfWork.Replies.GetReplies();
            return Ok(replies);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Reply>> GetReplyById(Guid id)
        {
            var reply = await unitOfWork.Replies.GetReplyById(id);
            return Ok(reply);
        }

        [HttpPost]
        public async Task<ActionResult<Reply>> PostReply(Reply reply)
        {
            var newReply = await unitOfWork.Replies.AddReply(reply);
            await unitOfWork.SaveChangesAsync();
            return Ok(newReply);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Reply>> PutReply(Reply reply)
        {
            var modifiedReply = await unitOfWork.Replies.EditReply(reply);
            await unitOfWork.SaveChangesAsync();
            return Ok(modifiedReply);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeleteReply(Guid id)
        {
            var result = await unitOfWork.Replies.DeleteReply(id);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }
    }
}
