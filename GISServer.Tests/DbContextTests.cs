using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GISServer.Tests
{
    public class DbContextTests
    {
        private Context GetDatabase()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new Context(options);
        }

        [Fact]
        public void CanRetrieveObjectsFromDatabase()
        {
            // Arrange
            using var context = GetDatabase();
            GeoObject geoObject1 = new GeoObject 
            {
                Name = "Test1"
            };
            GeoObject geoObject2 = new GeoObject 
            {
                Name = "Test2"
            };
            context.GeoObjects.AddRange(geoObject1, geoObject2);
            context.SaveChanges();

            // Act
            var geoObjects = context.GeoObjects.ToList();
            var geoObjectInfos = context.GeoObjectInfos.ToList();

            // Assert
            Assert.NotNull(geoObjects);
            Assert.Equal(2, geoObjects.Count);
            Assert.Contains(geoObject1, geoObjects);
            Assert.Contains(geoObject2, geoObjects);
        }
    }
}
