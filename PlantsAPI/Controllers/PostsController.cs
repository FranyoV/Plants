using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PostsController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {

            var posts = await unitOfWork.Posts.GetPosts();
            return Ok(posts);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Post>> GetPostById(Guid id)
        {
            var post = await unitOfWork.Posts.GetPostById(id);
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            var newPost = await unitOfWork.Posts.AddPost(post);
            await unitOfWork.SaveChangesAsync();
            return Ok(newPost);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Post>> PutPost([FromBody] Post post, [FromQuery] Guid id)
        {
            var modifiedPost = await unitOfWork.Posts.EditPost(post);
            await unitOfWork.SaveChangesAsync();
            return Ok(modifiedPost);
        }

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
