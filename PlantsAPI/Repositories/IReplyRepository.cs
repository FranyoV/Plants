using Microsoft.AspNetCore.Mvc;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IReplyRepository
    {
        Task<IEnumerable<Reply>> GetReplies();
        Task<Reply> GetReplyById(Guid replyId);
        Task<IEnumerable<Reply>> GetRepliesOfPost(Guid postId);
        Task<int> GetRepliesCount(Guid userId);
        Task<Reply> AddReply(Reply reply);
        Task<Reply> EditReply(Reply reply);
        Task<bool> DeleteReply(Guid replyId);
    }
}
