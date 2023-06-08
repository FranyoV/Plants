using PlantsAPI.Repositories;
using PlantsAPI.Services;

namespace PlantsAPI.Configuration
{
    public interface IUnitOfWork
    {
        IPlantRepository Plants { get; }
        IPostRepository Posts { get; }   
        IReplyRepository Replies { get; }
        IItemsRepository Items { get; }
        IUserAccountRepository UserAccounts { get; }

        Task SaveChangesAsync();
        void Dispose();
    }
}
