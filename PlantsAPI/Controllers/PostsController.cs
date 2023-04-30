using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    
    [Route("api/posts")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PostsController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
        }

        //TODO AUTHORIZATION
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var posts = await unitOfWork.Posts.GetPosts();
            return Ok(posts);
        }

        //TODO AUTHORIZATION
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Post>> GetPostById(Guid id)
        {
            var post = await unitOfWork.Posts.GetPostById(id);
            return Ok(post);
        }


        [HttpGet]
        [Route("{userId}/posts")]
        public async Task<ActionResult<Post>> GetPostByUser(Guid userId)
        {
            if (unitOfWork.UserContext.HasAuthorization(userId))
            {
                var post = await unitOfWork.Posts.GetPostsByUser(userId);
                return Ok(post);
            }
            return Unauthorized();
        }


        [HttpGet]
        [Route("{userId}/replies")]
        public async Task<ActionResult<Post>> GetPostByUserReplies(Guid userId)
        {
            if (unitOfWork.UserContext.HasAuthorization(userId))
            {
                var post = await unitOfWork.Posts.GetPostsByUserReplies(userId);
                return Ok(post);
            }
            return Unauthorized();
        }


        [HttpGet]
        [Route("user/{userId}/count")]
        public async Task<ActionResult<int>> GetPostsCount([FromRoute] Guid userId)
        {
            if (unitOfWork.UserContext.HasAuthorization(userId))
            {
                var posts = await unitOfWork.Posts.GetPostsCount(userId);
                return Ok(posts);
            }
            return Unauthorized();
        }


        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            if (unitOfWork.UserContext.HasAuthorization(post.UserId))
            {
                var newPost = await unitOfWork.Posts.AddPost(post);
                await unitOfWork.SaveChangesAsync();
                return Ok(newPost);
            }
            return Unauthorized();
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Post>> PutPost([FromBody] Post post, [FromQuery] Guid id)
        {
            if (unitOfWork.UserContext.HasAuthorization(post.Id))
            {
                var modifiedPost = await unitOfWork.Posts.EditPost(post);
                await unitOfWork.SaveChangesAsync();
                return Ok(modifiedPost);
            }
            return Unauthorized();
        }

        //TODO AUTHORIZATION
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeletePost(Guid id)
        {
            var result = await unitOfWork.Posts.DeletePost(id);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }
    }
}
