using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;

namespace PlantsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PostsController(IUnitOfWork unitOFWork)
        {
            this.unitOfWork = unitOFWork;
        }

        [HttpGet]
        [Route("posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {

            var posts = await unitOfWork.Posts.GetPosts();
            return Ok(posts);
        }

        [HttpGet]
        [Route("post")]
        public async Task<ActionResult<Post>> GetPostById(Guid id)
        {
            var post = await unitOfWork.Posts.GetPostById(id);
            return Ok(post);
        }

        [HttpPost]
        [Route("post")]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            var newPost = await unitOfWork.Posts.AddPost(post);
            await unitOfWork.SaveChangesAsync();
            return Ok(newPost);
        }

        [HttpPut]
        [Route("post")]
        public async Task<ActionResult<Post>> PutPost(Post post)
        {
            var modifiedPost = await unitOfWork.Posts.EditPost(post);
            await unitOfWork.SaveChangesAsync();
            return Ok(modifiedPost);
        }

        [HttpDelete]
        [Route("post")]
        public async Task<ActionResult<bool>> DeletePost(Guid id)
        {
            var result = await unitOfWork.Posts.DeletePost(id);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }
    }
}
