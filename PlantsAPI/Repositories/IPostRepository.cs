using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<PostDto>> GetPosts();
        Task<IEnumerable<Post>> GetPostsOfUser(Guid id);
        Task<IEnumerable<Post>> GetPostsWithUsersReplies(Guid userid);
        Task<Post> GetPostById(Guid id);
        Task<Post> AddPost(Post post);
        Task<Post> EditPost(Post post);
        Task<bool> DeletePost(Guid postId);
    }
}
