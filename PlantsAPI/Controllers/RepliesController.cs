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

        //TODO AUTHORIZATION
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reply>>> GetReplies()
        {

            var replies = await unitOfWork.Replies.GetReplies();
            return Ok(replies);
        }

        //TODO AUTHORIZATION
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Reply>> GetReplyById(Guid id)
        {
            var reply = await unitOfWork.Replies.GetReplyById(id);
            return Ok(reply);
        }

        //TODO AUTHORIZATION
        [HttpGet]
        [Route("post/{postid}")]
        public async Task<ActionResult<IEnumerable<Reply>>> GetRepliesOfPost([FromRoute] Guid postid) 
        {

                var replies = await unitOfWork.Replies.GetRepliesOfPost(postid);
                return Ok(replies);
        }

        [HttpGet]
        [Route("user/{userId}/count")]
        public async Task<ActionResult<int>> GetRepliesCount([FromRoute] Guid userId)
        {
            if (unitOfWork.UserContext.HasAuthorization(userId))
            {
                var replies = await unitOfWork.Replies.GetRepliesCount(userId);
                return Ok(replies);
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<ActionResult<Reply>> PostReply(Reply reply)
        {
            if (unitOfWork.UserContext.HasAuthorization(reply.UserId))
            {
                var newReply = await unitOfWork.Replies.AddReply(reply);
                await unitOfWork.SaveChangesAsync();
                return Ok(newReply);
            }
            return Unauthorized();
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Reply>> PutReply(Reply reply)
        {
            if (unitOfWork.UserContext.HasAuthorization(reply.UserId))
            {
                var modifiedReply = await unitOfWork.Replies.EditReply(reply);
                await unitOfWork.SaveChangesAsync();
                return Ok(modifiedReply);
            }
            return Unauthorized();
        }

        //TODO AUTHORIZATION
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
