using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public interface IItemsRepository
    {
        Task<IEnumerable<ItemDto>> GetItems();
        Task<Item> GetItemById(Guid id);
        Task<IEnumerable<Item>> GetItemsOfUser(Guid userId);
        Task<int> GetItemsCount(Guid userId);
        Task<Item> AddItem(Item item);
        Task<Item> EditItem(Item item);
        Task<bool> DeleteItem(Guid itemId);
    }
}
