using GISServer.API.Controllers;
using GISServer.API.Model;
using GISServer.API.Service.Interface;
using GISServer.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GISServer.Tests
{
   public class GeoObjectControllerTests
   {
       private readonly Mock<IGeoObjectService> _geoObjectServiceMock;
       private readonly GeoObjectController _geoObjectController;
       public GeoObjectControllerTests()
       {
           _geoObjectServiceMock = new Mock<IGeoObjectService>();
           _geoObjectController = new GeoObjectController(_geoObjectServiceMock.Object);
       }

       [Fact]
       public async Task Get_ReturnsGeoObjectsList()
       {
           // Arrange
           var geoObjectsDTO = new List<GeoObjectDTO>()
           {
               new GeoObjectDTO() { Id = Guid.NewGuid(), Name = "Object1" },
               new GeoObjectDTO() { Id = Guid.NewGuid(), Name = "Object2" }
           };
           _geoObjectServiceMock.Setup(s => s.Get()).ReturnsAsync(geoObjectsDTO);

           // Act
           var result = await _geoObjectController.Get();

           // Assert
           var okResult = Assert.IsType<ObjectResult>(result);
           Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
           var geoObjectsResult = Assert.IsType<List<GeoObjectDTO>>(okResult.Value);
           Assert.Equal(geoObjectsDTO.Count, geoObjectsResult.Count);
       }

       [Fact]
       public async Task Get_ReturnsGeoObject()
       {
           // Arrange
           var geoObjectDTO = new GeoObjectDTO() { Id = Guid.NewGuid(), Name = "Object" };
           _geoObjectServiceMock.Setup(s => s.Get(It.IsAny<Guid>()))
           .ReturnsAsync(geoObjectDTO);

           // Act
           var result = await _geoObjectController.Get((Guid)geoObjectDTO.Id);

           // Assert
           var okResult = Assert.IsType<ObjectResult>(result);
           Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
           var geoObjectResult = Assert.IsType<GeoObjectDTO>(okResult.Value);
           Assert.Equal(geoObjectDTO.Name, geoObjectResult.Name);
       }

       [Fact]
       public async Task CreateGeoObject_ReturnsCreatedAtActionResponse()
       {
           // Arrange
           var geoObjectDTO = new GeoObjectDTO() { Name = "Object1" };
           _geoObjectServiceMock.Setup(s => s.Add(It.IsAny<GeoObjectDTO>()))
           .ReturnsAsync((GeoObjectDTO geoObjectDTO) =>
           {
               var returnGeoObject = new GeoObjectDTO() { Id = Guid.NewGuid(), Name = geoObjectDTO.Name };
               return returnGeoObject;
           });

           // Act
           var result = await _geoObjectController.PostGeoObject(geoObjectDTO);

           // Assert
           var createdAtRouteResult = Assert.IsType<CreatedAtActionResult>(result.Result);
           Assert.Equal(StatusCodes.Status201Created, createdAtRouteResult.StatusCode);
           var createdGeoObject = Assert.IsType<GeoObjectDTO>(createdAtRouteResult.Value);
           Assert.Equal(geoObjectDTO.Name, createdGeoObject.Name);
       }
        
       [Fact]
       public async Task UpdateGeoObject_ReturnsNoContentResponse()
       {
           // Arrange
           var geoObjectDTOToUpdate = new GeoObjectDTO() { Id = Guid.NewGuid(), Name = "Updated object" };
           _geoObjectServiceMock.Setup(s => s.UpdateGeoObject(It.IsAny<GeoObjectDTO>()))
           .ReturnsAsync(geoObjectDTOToUpdate);

           // Act
           var result = await _geoObjectController.PutGeoObject((Guid)geoObjectDTOToUpdate.Id, geoObjectDTOToUpdate);

           // Assert
           var noContentResult = Assert.IsType<NoContentResult>(result);
           Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
       }
        
       [Fact]
       public async Task DeleteGeoObject_ReturnsNoContentResponse()
       {
           // Arrange
           var geoObjectDTO = new GeoObjectDTO() { Id = Guid.NewGuid(), Name = "Object" };
           _geoObjectServiceMock.Setup(s => s.Get(It.IsAny<Guid>()))
           .ReturnsAsync(geoObjectDTO);
           _geoObjectServiceMock.Setup(s => s.DeleteGeoObject(It.IsAny<Guid>()))
           .ReturnsAsync((true, "GeoObject got deleted"));

           // Act
           var result = await _geoObjectController.DeleteGeoObject((Guid)geoObjectDTO.Id);

           // Assert
           var noContentResult = Assert.IsType<NoContentResult>(result);
           Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
       }
   }
}
