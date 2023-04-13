using PlantsAPI.Repositories;

namespace PlantsAPI.Configuration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IPlantRepository Plants { get; }
        IPostRepository Posts { get; }   
        IReplyRepository Replies { get; }
        IAuthRepository Auth { get; }
        Task SaveChangesAsync();
        void Dispose();
    }
}
