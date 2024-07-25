using System;
using System.Collections.Generic;
using System.Linq;
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
        private TestData _data { get; set;}
        public Archive()
        {
            _data = new TestData();
        }

        [Fact]
        public void ArchiveGeoObject()
        {
            // Arrange
            var obj1 = _data._geoObjectRepository.GetByNameAsync("obj1").Result;
            
            // Act
            _data._geoObjectRepository.Archive(obj1.Id);
            obj1 = _data._geoObjectRepository.GetByNameAsync("obj1").Result;

            // Assert
            Assert.Equal(Status.Archive, obj1.Status);
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