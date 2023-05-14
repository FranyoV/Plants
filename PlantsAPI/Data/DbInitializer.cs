using PlantsAPI.Models;

namespace PlantsAPI.Data
{
    public class DbInitializer
    {
        public static void Initialize(PlantsDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (dbContext.Users.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User{
                    Id = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab"),
                    Name = "Virág98",
                    EmailAddress = "viragfranyo@gmail.com",
                    PasswordHash = "",
                    PasswordSalt = "",
                },
                new User{
                    Id = Guid.Parse("2926d291-b549-429c-b4a7-6cdfbbca322e"),
                    Name = "Borii31",
                    EmailAddress = "borii31@gmail.com",
                    PasswordHash = "",
                    PasswordSalt = "",
                },
                new User{
                    Id = Guid.Parse("20526112-56fa-4e4a-8ae5-1d3603aa47e9"),
                    Name = "Emma64",
                    EmailAddress = "emma64@gmail.com",
                    PasswordHash = "",
                    PasswordSalt = "",
                }
            };

            foreach (var user in users)
            {
                dbContext.Users.Add(user);
            }
            dbContext.SaveChanges();



            var posts = new Post[]
            {
                new Post
                {
                    Id = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                    Title = "My cactus is dying.",
                    Content = "Why can't I keep a cactus alive. They supposed to stay alive even in deserts.",
                    DateOfCreation = DateTime.Now,
                    UserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab"),
                }
            };

            foreach (var post in posts)
            {
                dbContext.Posts.Add(post);
            }
            dbContext.SaveChanges();


         
            var replies = new Reply[]
            {
                new Reply
                {
                    Id = Guid.Parse("e41ca1f8-7fda-4610-bcce-f63972b6e302"),
                    Content = "Dudee water it once every two weeks and put the bad boy by the window and you both should be good.",
                    DateOfCreation = DateTime.Now,
                    PostId = Guid.Parse("c1a1e629-7d42-4b6a-8fa1-6508f9c594ec"),
                    UserId = Guid.Parse("20526112-56fa-4e4a-8ae5-1d3603aa47e9")
                }
            };

            foreach (var reply in replies)
            {
                dbContext.Replies.Add(reply);
            }
            dbContext.SaveChanges();



            
            var items = new Item[]
            {
                new Item
                {
                    Id = Guid.Parse("f3cef2b2-97d4-4dff-972b-17934ddbb129"),
                    Name = "Cactus",
                    Type = ItemType.PLANT,
                    Price = 100,
                    Date = DateTime.Now,
                    Sold = false,
                    UserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab")
                }
            };

            foreach (var item in items)
            {
                dbContext.Items.Add(item);
            }
            dbContext.SaveChanges();



            
            var plants = new Plant[]
            {
                new Plant
                {
                    Id = Guid.Parse("722f0dc8-4f33-4fee-a0fe-c2c29adb2055"),
                    Name = "Dying Cactus",
                    Description = "I don't think plants are for me.",
                    UserId = Guid.Parse("7a02b8d4-570d-404f-946e-40b4524727ab")
                }
            };

            foreach (var plant in plants)
            {
                dbContext.Plants.Add(plant);
            }
            dbContext.SaveChanges();

        }
    }
}
