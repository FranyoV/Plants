using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Configuration;
using PlantsAPI.Models;
using PlantsAPI.Services;

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
            this.unitOfWork = unitOFWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        //TODO AUTHORIZATION
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            try
            {
                var posts = await unitOfWork.Posts.GetPosts();
                return Ok(posts);
            }
            catch
            {
                return BadRequest();
            }
 
        }

        //TODO AUTHORIZATION
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Post>> GetPostById(Guid id)
        {
            try
            {
                var post = await unitOfWork.Posts.GetPostById(id);
                return Ok(post);
            }
            catch
            {
                return BadRequest();
            }
            
        }


        [HttpGet]
        [Route("{userId}/posts")]
        public async Task<ActionResult<Post>> GetPostByUser(Guid userId)
        {
            try
            {

                    var post = await unitOfWork.Posts.GetPostsOfUser(userId);
                    return Ok(post);

            }
            catch
            {
                return BadRequest();
            }
            
        }


        [HttpGet]
        [Route("{userId}/replies")]
        public async Task<ActionResult<Post>> GetPostByUserReplies(Guid userId)
        {
            try
            {

                    var post = await unitOfWork.Posts.GetPostsByUserReplies(userId);
                    return Ok(post);

            }
            catch
            {
                return BadRequest();
            }

        }


        [HttpGet]
        [Route("user/{userId}/count")]
        public async Task<ActionResult<int>> GetPostsCount([FromRoute] Guid userId)
        {
            try
            {

                    var posts = await unitOfWork.Posts.GetPostsCount(userId);
                    return Ok(posts);

            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            try
            {

                    var newPost = await unitOfWork.Posts.AddPost(post);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(newPost);

            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Post>> PutPost([FromBody] Post post, [FromQuery] Guid id)
        {
            try
            {

                    var modifiedPost = await unitOfWork.Posts.EditPost(post);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(modifiedPost);
            }
            catch
            { 
                return BadRequest(); 
            }
        }


        //TODO AUTHORIZATION
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeletePost(Guid id)
        {
            try
            {
                var result = await unitOfWork.Posts.DeletePost(id);
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
