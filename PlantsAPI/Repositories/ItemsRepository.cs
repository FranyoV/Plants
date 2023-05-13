﻿using Microsoft.EntityFrameworkCore;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Services;

namespace PlantsAPI.Repositories
{
    public class ItemsRepository : GenericRepository<Item>, IItemsRepository
    {
        public ItemsRepository(PlantsDbContext dbContext, IUserContext userContext) : base(dbContext, userContext )
        {
        }

        //anonymous access allowed
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

            List<Item> items = new();

            if ( _userContext.HasAuthorization(userId))
            {
                items = await dbSet.Where(i => i.UserId == userId).ToListAsync();
            }
            return items;
        }


        public async Task<int> GetItemsCount(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            List<Item> items = new();

            if (_userContext.HasAuthorization(userId))
            {
                items = await dbSet.Where(p => p.UserId == userId).ToListAsync();
                
            }
            return items.Count;
        }


        public async Task<Item> AddItem(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            item.Id = Guid.NewGuid();

            if (_userContext.HasAuthorization(item.UserId))
            {
                var result = await dbSet.AddAsync(item);
                return result.Entity;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<Item> EditItem(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (_userContext.HasAuthorization(item.UserId))
            {
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

                return originalItem ?? new Item();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }

        }

        public async Task<bool> DeleteItem(Guid itemId)
        {
            if (itemId == Guid.Empty) throw new NotImplementedException();

            Guid currentUserId = Guid.Parse(_userContext.GetMe()); ;

            var isItemUsers = dbSet.Where(p => p.Id == itemId).Any(ui => ui.UserId == currentUserId);

            if (isItemUsers)
            {
                var toBeDeleted = await dbSet.Where(p => p.Id == itemId).FirstAsync();
                if (toBeDeleted != null)
                {
                    var result = dbSet.Remove(toBeDeleted);
                    return result.State == EntityState.Deleted;
                }
            }

            return false;
        }
    }
}
