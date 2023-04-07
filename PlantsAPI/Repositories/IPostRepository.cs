using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<PostDto>> GetPosts();
        Task<Post> GetPostById(Guid id);
        Task<Post> AddPost(Post post);
        Task<Post> EditPost(Post post);
        Task<bool> DeletePost(Guid postId);
    }
}
