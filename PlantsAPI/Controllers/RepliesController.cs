using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    [Route("api/replies")]
    [ApiController]
    [Authorize]
    public class RepliesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RepliesController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork ?? throw new ArgumentNullException(nameof(unitOFWork));
        }

       
 
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Reply>> GetReplyById(Guid id)
        {
            try
            {
                var reply = await unitOfWork.Replies.GetReplyById(id);
                return Ok(reply);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("post/{postid}")]
        public async Task<ActionResult<IEnumerable<ReplyDto>>> GetRepliesOfPost([FromRoute] Guid postid) 
        {
            try
            {
                var replies = await unitOfWork.Replies.GetRepliesOfPost(postid);
                return Ok(replies);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("user/{userId}/count")]
        public async Task<ActionResult<int>> GetRepliesCount([FromRoute] Guid userId)
        {
            try
            {
                var replies = await unitOfWork.Replies.GetRepliesCount(userId);
                return Ok(replies);
            }
            catch
            {
                return BadRequest();
            }
           
        }

        [HttpPost]
        public async Task<ActionResult<ReplyDto>> PostReply(Reply reply)
        {
            try
            {
                var newReply = await unitOfWork.Replies.AddReply(reply);
                await unitOfWork.SaveChangesAsync();
                return Ok(newReply);
            }
            catch
            {
                return BadRequest();
            }
        }

   
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeleteReply(Guid id)
        {
            try
            {
                var result = await unitOfWork.Replies.DeleteReply(id);
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
