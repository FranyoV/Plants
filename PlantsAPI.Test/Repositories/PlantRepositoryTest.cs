using Microsoft.EntityFrameworkCore;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using PlantsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PlantsAPI.Test.Repositories
{
    public class PlantRepositoryTest
    {
        private readonly PlantsDbContext context;    
        private readonly Mock<IUserContext> userContext;
        private readonly IPlantRepository plantRepository;
      

        public PlantRepositoryTest()
        {

            DbContextOptionsBuilder<PlantsDbContext> dbOptions = new DbContextOptionsBuilder<PlantsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            context = new PlantsDbContext(dbOptions.Options);

            GetMockPlantList();

            userContext = new Mock<IUserContext>();
            plantRepository = new PlantRepository(context, userContext.Object);

            
            
            
           /* mockDbSet.Setup(
                x => x.AddAsync(It.IsAny<Plant>(), It.IsAny<CancellationToken>()))
                .Callback((Plant model, CancellationToken tkn) => { })
                .Returns((Plant model, CancellationToken tkn) => ValueTask.FromResult((EntityEntry<Plant>)null));

            mockDbContext.Setup(
                x => x.Set<Plant>())
                .Returns(mockDbSet.Object);
            mockDbContext.Setup(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(1));*/

           // GetMockPlantList();

           // plantRepository = new PlantRepository(context, userContext.Object);
           
        }

        public async void GetMockPlantList()
        {
            List<Plant> plants = new()
            {
                new Plant
                {
                    Id = Guid.Parse("7fe95d67-7808-4b97-9968-5a2e529bab66"),
                    Name = "John Doe",
                    Description = "J.D@gmail.com",
                    UserId = Guid.Parse("a879da40-e11e-41cd-8cff-e14e61ce5950")

                },
                new Plant()
                {
                    Id = Guid.Parse("0ba7dd3b-469e-4eb8-9401-7709b77d4ccd"),
                    Name = "TestName",
                    UserId = Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950")
                },
                new Plant
                {
                    Id = Guid.Parse("dafe0ccc-8830-46b4-aa92-71794079d727"),
                    Name = "Mark Luther",
                    Description = "M.L@gmail.com",
                    UserId = Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950")

                }
        };
        
        
            foreach (var plant in plants)
            {
                await context.Plants.AddAsync(plant);
                await context.SaveChangesAsync();
            }
            
        }


  

        [Fact]
        public void ConstructorShouldCreateObject()
        {
            Assert.NotNull(new PlantRepository(context, userContext.Object));
        }

        [Fact]
        public void Contsructor_ShouldThrowArgumentNullException_1()
        {
            Assert.Throws<ArgumentNullException>(() => new PlantRepository(null, userContext.Object));
        }

        [Fact]
        public void Contsructor_ShouldThrowArgumentNullException_2()
        {
            Assert.Throws<ArgumentNullException>(() => new PlantRepository(context, null));
        }

        #region GetPlantsOfUser
        [Fact]
        public void GetPlantsOfUser_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.GetPlantsOfUser(Guid.Empty));
        }


        [Fact]
        public void GetPlantsOfUser_ShouldReturnOk()
        {

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);


            var result = plantRepository.GetPlantsOfUser(Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950"));


            Assert.NotNull(result.Result);
            Assert.Empty(result.Result);
        }



        [Fact]
        public void GetPlantsOfUser_ShouldReturnEmptyList()
        {

            var result = plantRepository.GetPlantsOfUser(Guid.NewGuid());
            Assert.NotNull(result.Result);
            Assert.Empty(result.Result);
        }


        [Fact]
        public void GetPlantsOfUser_ShouldReturnResult()
        {

            Plant testPlant = new()
            {
                Id = Guid.Parse("dafe0ccc-8830-46b4-aa92-71794079d727"),
                Name = "Mark Luther",
                Description = "M.L@gmail.com",
                UserId = Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950")

            };
          /*    context.SaveChangesAsync();

              var userC = new Mock<IUserContext>();
              var plantRepo = new PlantRepository(context, userC.Object);


              userC.Setup(
                   x => x.HasAuthorization(It.IsAny<Guid>()))
                   .Returns(true);


              var result = plantRepo.GetPlantsOfUser(Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950"));*/


            userContext.Setup(
                 x => x.HasAuthorization(It.IsAny<Guid>()))
                 .Returns(true);


            var result = plantRepository.GetPlantsOfUser(Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950"));

            Assert.NotNull(result);
            Assert.NotEmpty(result.Result);
            Assert.Equal(2, result.Result.Count());

        }
        #endregion


        #region GetPlantsCount

        [Fact]
        public void GetPlantsCount_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.GetPlantsCount(Guid.Empty));
        }

        [Fact]
        public void GetPlantsCount_ShouldReturn0()
        {

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);


            var result = plantRepository.GetPlantsCount(Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950"));


            Assert.NotNull(result.Result);
            Assert.Equal(0,result.Result);
        }



        [Fact]
        public void GetPlantsCount_ShouldReturnResult()
        {

            Plant testPlant = new()
            {
                Id = Guid.Parse("dafe0ccc-8830-46b4-aa92-71794079d727"),
                Name = "Mark Luther",
                Description = "M.L@gmail.com",
                UserId = Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950")

            };
            /*    context.SaveChangesAsync();

                var userC = new Mock<IUserContext>();
                var plantRepo = new PlantRepository(context, userC.Object);


                userC.Setup(
                     x => x.HasAuthorization(It.IsAny<Guid>()))
                     .Returns(true);


                var result = plantRepo.GetPlantsOfUser(Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950"));*/


            userContext.Setup(
                 x => x.HasAuthorization(It.IsAny<Guid>()))
                 .Returns(true);


            var result = plantRepository.GetPlantsCount(Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950"));

            Assert.NotNull(result);
           
            Assert.Equal(2, result.Result);

        }

        #endregion


        #region AddPlant

        [Fact]
        public void AddPlant_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.AddPlant(null));
        }

        [Fact]
        public void AddPlant_ShouldHaveNoAuthorization()
        {

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => plantRepository.AddPlant(new Plant()));
        }



        [Fact]
        public void AddPlant_ShouldReturnResult()
        {
            Plant testPlant = new()
            {
                Id = Guid.Parse("dafe0ccc-8830-46b4-aa92-71794079d727"),
                Name = "Zöldike",
                Description = "él",
                UserId = Guid.Parse("c979da40-e11e-41cd-8cff-e14e61ce5950")

            };

            userContext.Setup(
                 x => x.HasAuthorization(It.IsAny<Guid>()))
                 .Returns(true);


            var result = plantRepository.AddPlant(testPlant);

            Assert.NotNull(result);

            Assert.True(testPlant.Id == result.Result.Id);
        }

        #endregion

        #region EditPlant

        [Fact]
        public void EditPlant_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.EditPlant(null));
        }


        [Fact]
        public void EditPlant_ShouldHaveNoAuthorization()
        {

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => plantRepository.EditPlant(new Plant()));
        }



        [Fact]
        public void EditPlant_ShouldReturnResult()
        {
            Plant testPlant = new()
            {
                Id = Guid.Parse("dafe0ccc-8830-46b4-aa92-71794079d727"),
                Name = "Zöldike",
                Description = "él",
                UserId = Guid.Parse("c979da40-e11e-41cd-8cff-e14e61ce5950")

            };

            userContext.Setup(
                 x => x.HasAuthorization(It.IsAny<Guid>()))
                 .Returns(true);


            var result = plantRepository.EditPlant(testPlant);

            Assert.NotNull(result);

            Assert.True(testPlant.Id == result.Result.Id);
        }

        #endregion

        #region DeletePlant

        [Fact]
        public void DeletePlant_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.DeletePlant(Guid.Empty));
        }


        [Fact]
        public void DeletePlant_ShouldHaveNoAuthorization()
        {

            userContext.Setup(
                x => x.HasAuthorization(It.IsAny<Guid>()))
                .Returns(false);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => plantRepository.DeletePlant(Guid.NewGuid()));
        }



        [Fact]
        public void DeletePlant_ShouldReturnResult()
        {
            Plant testPlant = new()
            {
                Id = Guid.Parse("dafe0ccc-8830-46b4-aa92-71794079d727"),
                Name = "Zöldike",
                Description = "él",
                UserId = Guid.Parse("c979da40-e11e-41cd-8cff-e14e61ce5950")

            };

            userContext.Setup(
                 x => x.GetMe()).Returns(It.IsAny<string>());


            var result = plantRepository.DeletePlant(Guid.Parse("dafe0ccc-8830-46b4-aa92-71794079d727"));

            Assert.NotNull(result);

            Assert.True(result.Result);
        }
        #endregion
    }
}
