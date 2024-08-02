using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using GISServer.Infrastructure.Service;
using GISServer.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GISServer.Tests.Archive
{
    public class Archive
    {
        private TestData _data { get; set; }

        public Archive()
        {
            _data = new TestData();
        }

        [Fact]
        public void ArchiveGeoObject()
        {
            // Arrange
            _data.InitDataAsync().Wait();
            var obj0 = _data.objects[0];
            var obj1 = _data.objects[1];
            var aspect0 = _data.aspects[0];
            var aspect1 = _data.aspects[1];
            var class0 = _data.classifiers[0];
            var class1 = _data.classifiers[1];
            var topology0 = _data.topologyLinks[0];
            var topology1 = _data.topologyLinks[1];
            var parentChildLink0 = _data.parentChildLinks[0];
            var parentChildLink1 = _data.parentChildLinks[1];

            _data._geoObjectService.AddClassifier(obj0.Id, class0.Id);
            _data._geoObjectService.AddClassifier(obj0.Id, class1.Id);
            //_data._geoObjectRepository.AddAspect(aspect0);
            //_data._geoObjectRepository.AddAspect(aspect1);

            // Act
            _data._geoObjectService.Archive(obj0.Id);
            obj0 = _data._geoObjectRepository.GetByNameAsync("obj0").Result;

            // Assert
            Assert.Equal(Status.Archive, obj0.Status);
            // Assert.Equal(Status.Archive, class0.Status);
            // Assert.Equal(Status.Archive, class1.Status);
            // Assert.Equal(Status.Archive, aspect0.Status);
            // Assert.Equal(Status.Archive, aspect1.Status);

            // TODO добавить проверку на архивацию связей, аспектов и геометрии объекта
        }

        [Fact]
        public void ArchiveClassifier()
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(1, 1);
        }

        [Fact]
        public void ArchiveGOC()
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(1, 1);
        }

        [Fact]
        public void ArchiveAspect()
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(1, 1);
        }

        [Fact]
        public void ArchiveParentChildLink()
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(1, 1);
        }

        [Fact]
        public void ArchiveTopologyLink()
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(1, 1);
        }


    }
}
