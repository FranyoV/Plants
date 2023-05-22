using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PlantsAPI.Data;
using PlantsAPI.Models;
using PlantsAPI.Repositories;
using PlantsAPI.Services;
using System;
using System.Collections;
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
            userContext = new Mock<IUserContext>();
            plantRepository = new PlantRepository(context, userContext.Object);
        }

        #region DataBase

        //[RunnableInDebugOnly(DisplayName = "Needs database XYZ")]
        //public void ExampleExtract()
        //{
        //    var sqlSetup = new SqlServerProductionSetup<PlantsDbContext>
        //        ("ExampleDatabaseConnectionName");
        //    using (var context = new PlantsDbContext(sqlSetup.Options))
        //    {
        //        //1a. Read in the data to want to seed the database with
        //        var entities = context.Plants
        //            .Include(x => x.Id)
        //            .Include(x => x.Name)
        //            .Include(x => x.UserId)

        //            .ToList();

        //        //1b. Reset primary and foreign keys
        //        var resetter = new DataResetter(context);
        //        resetter.ResetKeysEntityAndRelationships(entities);

        //        //1c. Convert to JSON string
        //        var jsonString = entities.DefaultSerializeToJson();
        //        //1d. Save to JSON local file in TestData directory
        //        sqlSetup.DatabaseName.WriteJsonToJsonFile(jsonString);
        //    }
        //}
        #endregion

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

        #region
        [Fact]
        public void GetPlantsOfUser_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.GetPlantsOfUser(Guid.Empty));
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
            Plant testPlant = new Plant()
            {
                Id = Guid.Parse("0ba7dd3b-469e-4eb8-9401-7709b77d4ccd"),
                Name = "TestName",
                UserId = Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950")
            };

            IEnumerable plants = new List<Plant>() { testPlant };


            var result = plantRepository.GetPlantsOfUser(Guid.Parse("b879da40-e11e-41cd-8cff-e14e61ce5950"));
            Assert.NotNull(result);
            Assert.Single(result.Result);
            Assert.Equal("TestName", result.Result.First().Name);

        }
        #endregion


        #region GetPlantsCount

        [Fact]
        public void GetPlantsCount_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.GetPlantsCount(Guid.Empty));
        }

        #endregion


        #region AddPlant

        [Fact]
        public void AddPlant_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.AddPlant(null));
        }

        #endregion

        #region EditPlant

        [Fact]
        public void EditPlant_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.EditPlant(null));
        }

        #endregion

        #region DeletePlant

        [Fact]
        public void DeletePlant_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => plantRepository.DeletePlant(Guid.Empty));
        }

        #endregion
    }
}
