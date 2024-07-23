using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using GISServer.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GISServer.Tests
{
    public class GeoObjectRepositoryTests
    {
        private Context GetDatabase()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new Context(options);
        }

        [Fact]
        public async void CanCreateGeoObject()
        {
            // Arrange
            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);

            GeoObject geoObject = new GeoObject
            {
                Name = "Test"
            };

            // Act
            await repository.AddGeoObject(geoObject);
            context.SaveChanges();

            // Assert
            Assert.Equal(1, context.GeoObjects.Count());
            Assert.Equal("Test", context.GeoObjects.Single().Name);
        }
        
        [Fact]
        public async void CanRetrieveGeoObject()
        {
            // Arrange
            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);

            GeoObject geoObject = new GeoObject
            {
                Name = "Test"
            };
            await repository.AddGeoObject(geoObject);
            context.SaveChanges();

            // Act
            var result = await repository.GetGeoObject(geoObject.Id);

            // Assert
            Assert.Equal(geoObject.Name, result.Name);
        }

        [Fact]
        public async void TryToUpDate()
        {
            // Arrange
            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);
            GeoObject geoObject = new GeoObject
            {
                Name = "Объект_Тестовый",
                Status = Status.Actual
            };
            await repository.AddGeoObject(geoObject);
            context.SaveChanges();

            // Act
            geoObject.Name = "Объект_Обновленный";
            geoObject.Status = Status.Archive;
            await repository.UpdateGeoObject(geoObject);
            context.SaveChanges();

            // Assert
            
            var test = await repository.GetGeoObject(geoObject.Id);
            Assert.Equal("Объект_Обновленный", test.Name);
            Assert.Equal(Status.Archive,test.Status);
           
        }

        [Fact]
        public async void CanDeleteGeoObject()
        {
            // Arrange
            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);
            GeoObject geoObject = new GeoObject
            {
                Name = "Test"
            };
            await repository.AddGeoObject(geoObject);
            context.SaveChanges();

            // Act
            await repository.DeleteGeoObject(geoObject.Id);
            context.SaveChanges();

            // Assert
            Assert.Equal(0, context.GeoObjects.Count());
        }

        [Fact]
        public async void TrytoUpDateObjectAndLinks()
        {
            // Arrange
            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);

            GeoObject geoObject1 = new GeoObject
            {
                Name = "об.1",
                Status = Status.Actual
            };

            GeoObject geoObject2 = new GeoObject
            {
                Name = "об.2",
                Status = Status.Actual
            };

            TopologyLink tpl1 = new TopologyLink
            {
                CommonBorder = "l1"
            };
            TopologyLink tpl2 = new TopologyLink
            {
                CommonBorder = "l2"
            };
            geoObject1.InputTopologyLinks.Add(tpl1);
            geoObject1.OutputTopologyLinks.Add(tpl2);
            geoObject2.InputTopologyLinks.Add(tpl2);
            geoObject2.OutputTopologyLinks.Add(tpl1);
            

            await repository.AddGeoObject(geoObject1);
            await repository.AddGeoObject(geoObject2);
            context.SaveChanges();

            //Act
            GeoObject geoObject3 = new GeoObject
            {
                Name = "об.3",
                Status = Status.Actual
            };

            TopologyLink tpl3 = new TopologyLink
            {
                CommonBorder = "l3"
            };

            TopologyLink tpl4 = new TopologyLink
            {
                CommonBorder = "l3"
            };

            TopologyLink tpl5 = new TopologyLink
            {
                CommonBorder = "l3"
            };

            TopologyLink tpl6 = new TopologyLink
            {
                CommonBorder = "l3"
            };
            
            geoObject3.OutputTopologyLinks.Add(tpl3);
            geoObject3.InputTopologyLinks.Add(tpl4);
            geoObject3.InputTopologyLinks.Add(tpl5);
            geoObject3.OutputTopologyLinks.Add(tpl6);

            await repository.AddGeoObject(geoObject3);

            geoObject1.InputTopologyLinks.Add(tpl3);
            geoObject1.OutputTopologyLinks.Add(tpl4);
            geoObject2.OutputTopologyLinks.Add(tpl5);
            geoObject2.InputTopologyLinks.Add(tpl6);

            await repository.UpdateAsync(geoObject1);
            await repository.UpdateAsync(geoObject2);
            
            context.SaveChanges();

            //Assert
            Assert.Equal(3, context.GeoObjects.Count());
            Assert.Equal(2, geoObject1.InputTopologyLinks.Count());
            Assert.Equal(2, geoObject1.OutputTopologyLinks.Count());

            Assert.Equal(2, repository.GetByNameAsync("об.2").Result.OutputTopologyLinks.Count());
            Assert.Equal(2, geoObject2.InputTopologyLinks.Count());
            Assert.Equal(2, geoObject3.OutputTopologyLinks.Count());
            Assert.Equal(2, geoObject3.InputTopologyLinks.Count());
            
        }

    }
}

