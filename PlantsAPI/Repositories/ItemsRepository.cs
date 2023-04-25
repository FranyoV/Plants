using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;

namespace PlantsAPI.Repositories
{
    public class ItemsRepository : GenericRepository<Item>, IItemsRepository
    {
        public ItemsRepository(PlantsDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {

        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<Item> GetItemById(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var result = await dbSet.Where(i => i.Id == id).FirstAsync();

            return result;
        }


        public async Task<IEnumerable<Item>> GetItemsOfUser(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            var result = await dbSet.Where(i => i.UserId == userId).ToListAsync();
            return result;
        }

        public async Task<int> GetItemsCount(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Item> result = await dbSet.Where(p => p.UserId == userId).ToListAsync();
            return result.Count;
        }


        public async Task<Item> AddItem(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            item.Id = Guid.NewGuid();


            var result = dbSet.Add(item);
            return result.Entity;
        }

        public async Task<Item> EditItem(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            Item originalItem = await dbSet.FirstAsync(p => p.Id == item.Id);

            if (originalItem != null)
            {
                originalItem.Name = item.Name;
                originalItem.Description = item.Description;
                originalItem.ImageUrl = item.ImageUrl;
                originalItem.Type = item.Type;
                originalItem.ImageUrl = item.ImageUrl;
                originalItem.Price = item.Price;
            }

            return (originalItem != null) ? originalItem : new Item();
        }

        public async Task<bool> DeleteItem(Guid itemId)
        {
            if (itemId == Guid.Empty) throw new NotImplementedException();

            var toBeDeleted = await dbSet.Where(p => p.Id == itemId).FirstAsync();
            if (toBeDeleted != null)
            {
                var result = dbSet.Remove(toBeDeleted);
                return result.State == EntityState.Deleted;
            }

            return false;
        }
    }
}
