using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using GISServer.Domain;
using GISServer.Infrastructure.Service;
using GISServer.Domain.Model;

namespace GISServer.Tests
{
    public class TestObjectRepository
    {
        [Fact]
        public void VoidTestHelper()
        {
            var testhelper = new TestHelper();
            var objectRepository = testhelper.GeoObjectRepository;

            Assert.Equal(1,1);
        }

        [Fact] 
        public void TestUpdateAdd()
        {
            var testHelper = new TestHelper();
            var geoObjectRepository = testHelper.GeoObjectRepository;
            var geoObject = geoObjectRepository.GetByNameAsync("ОбъектТест_1").Result;
            geoObjectRepository.ChangeTrackerClear();
            geoObject.Name = "Об_2";
            var inputTpLink = new TopologyLink { Status = Status.Actual, CommonBorder = "l3" };
            geoObject.InputTopologyLinks.Add(inputTpLink);

            geoObjectRepository.UpdateAsync(geoObject).Wait();

            Assert.Equal("Об_2", geoObjectRepository.GetByNameAsync("Об_2").Result.Name);
            Assert.Equal(2, geoObjectRepository.GetByNameAsync("Об_2").Result.InputTopologyLinks.Count);

        }

    }
}
