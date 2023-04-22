using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public class ReplyRepository : GenericRepository<Reply>, IReplyRepository
    {
        public ReplyRepository(PlantsDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {

        }

        public async Task<IEnumerable<Reply>> GetReplies()
        {
            return await dbSet.ToListAsync();
        }

        public Task<Reply> GetReplyById(Guid replyId)
        {
            if (replyId == Guid.Empty) throw new ArgumentNullException(nameof(replyId));

            var reply = dbSet.Where(r => r.Id == replyId).FirstAsync();
            return reply;
        }

        public async Task<IEnumerable<Reply>> GetRepliesOfPost(Guid postId)
        {
            if (postId == Guid.Empty) throw new ArgumentNullException(nameof(postId));
            List<Reply> replies = await dbSet.Where(r => r.PostId == postId).ToListAsync();

            var repliesInOrder = replies.OrderBy(x => x.DateOfCreation);

            return repliesInOrder;
        }

        public async Task<int> GetRepliesCount(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Reply> result = await dbSet.Where(p => p.UserId == userId).ToListAsync();
            return result.Count;

        }

        public async Task<Reply> AddReply(Reply reply)
        {
            if (reply == null) throw new ArgumentNullException(nameof(reply));
            reply.Id = Guid.NewGuid();
            var added = dbSet.Add(reply);

            //var result = await GetReplyById(reply.Id);
            return added.Entity;
        }

        public async Task<Reply> EditReply(Reply reply)
        {
            if (reply == null) throw new ArgumentNullException(nameof(reply));

            // dbSet.Update() ??
            var originalReply = await dbSet.Where(r => r.Id == reply.Id).FirstAsync();
            if (originalReply != null)
            {
                originalReply.Content = reply.Content;
                originalReply.DateOfCreation = reply.DateOfCreation;
            }
            return originalReply;
        }

        public async Task<bool> DeleteReply(Guid replyId)
        {
            if (replyId == Guid.Empty) throw new ArgumentNullException(nameof(replyId));

            var toBeDeleted = await dbSet.Where(r => r.Id == replyId).FirstAsync();

            var result = dbSet.Remove(toBeDeleted);

            return result.State == EntityState.Deleted;
        }
    }
}
