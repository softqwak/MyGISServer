using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using GISServer.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GISServer.Tests
{
    public class TestUnionNew
    {
        private Context GetDatabase()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new Context(options);
        }

        [Fact]

        public async void TestNewUnion()
        {
            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);

            //Arrange

            GeoObject geoObject_A = new GeoObject()
            {
                Name = "Объект_A",
                Status = Status.Actual

            };
            GeoObject geoObject_B = new GeoObject()
            {
                Name = "Объект_B",
                Status = Status.Actual
            };
            GeoObject geoObject_C = new GeoObject()
            {
                Name = "Объект_C",
                Status = Status.Actual
            };
            GeoObject geoObject_D = new GeoObject()
            {
                Name = "Объект_D",
                Status = Status.Actual
            };

            TopologyLink Tplink1 = new TopologyLink
            {
                Status = Status.Actual
            };
            TopologyLink Tplink2 = new TopologyLink
            {
                Status = Status.Actual
            };
            TopologyLink Tplink3 = new TopologyLink()
            {
                Status = Status.Actual
            };
            TopologyLink Tplink4 = new TopologyLink()
            {
                Status = Status.Actual
            };
            TopologyLink Tplink5 = new TopologyLink()
            {
                Status = Status.Actual
            };
            TopologyLink Tplink6 = new TopologyLink()
            {
                Status = Status.Actual
            };

            geoObject_A.InputTopologyLinks.Add(Tplink1);
            geoObject_A.OutputTopologyLinks.Add(Tplink2);
            geoObject_B.InputTopologyLinks.Add(Tplink2);
            geoObject_B.OutputTopologyLinks.Add(Tplink1);

            geoObject_A.OutputTopologyLinks.Add(Tplink3);
            geoObject_A.InputTopologyLinks.Add(Tplink4);
            geoObject_C.OutputTopologyLinks.Add(Tplink4);
            geoObject_C.InputTopologyLinks.Add(Tplink3);

            geoObject_B.InputTopologyLinks.Add(Tplink5);
            geoObject_B.OutputTopologyLinks.Add(Tplink6);
            geoObject_D.InputTopologyLinks.Add(Tplink6);
            geoObject_D.OutputTopologyLinks.Add(Tplink5);


            context.Add(geoObject_A);
            context.Add(geoObject_B);
            context.Add(geoObject_C);
            context.Add(geoObject_D);

            context.SaveChanges();
            context.ChangeTracker.Clear();

            //Act

            geoObject_A = await repository.Get(geoObject_A.Id);
            geoObject_B = await repository.Get(geoObject_B.Id);

            geoObject_A.Status = Status.Archive;
            geoObject_B.Status = Status.Archive;

            ParentChildObjectLink child_A = new ParentChildObjectLink()
            {
                ChildGeographicalObject = geoObject_A
            };
            ParentChildObjectLink child_B = new ParentChildObjectLink()
            {
                ChildGeographicalObject = geoObject_B
            };

            GeoObject geoObject_AB = new GeoObject
            {
                Name = "Объект_AB",
                Status = Status.Actual
            };

            geoObject_AB.ChildGeoObjects.Add(child_A);
            geoObject_AB.ChildGeoObjects.Add(child_B);

            
            foreach (var Inputlink in geoObject_A.InputTopologyLinks)
            {
                if (Inputlink?.GeographicalObjectOut?.Status == Status.Archive)
                {
                    Inputlink.Status = Status.Archive;
                } 
                else if (Inputlink?.GeographicalObjectOut?.Status != Status.Archive)
                {
                    geoObject_AB.InputTopologyLinks.Add(Inputlink);
                }
            }

            foreach (var Outputlink in geoObject_A.OutputTopologyLinks)
            {
                if (Outputlink?.GeographicalObjectIn?.Status == Status.Archive)
                {
                    Outputlink.Status = Status.Archive;
                }
                else if (Outputlink?.GeographicalObjectIn?.Status != Status.Archive)
                {
                    geoObject_AB.OutputTopologyLinks.Add(Outputlink);
                }
            }

            foreach (var Inputlink in geoObject_B.InputTopologyLinks)
            {
                if (Inputlink?.GeographicalObjectOut?.Status == Status.Archive)
                {
                    Inputlink.Status = Status.Archive;
                }
                else if (Inputlink?.GeographicalObjectOut?.Status != Status.Archive)
                {
                    geoObject_AB.InputTopologyLinks.Add(Inputlink);
                }
            }

            foreach (var Outputlink in geoObject_B.OutputTopologyLinks)
            {
                if (Outputlink?.GeographicalObjectIn?.Status == Status.Archive)
                {
                    Outputlink.Status = Status.Archive;
                }
                else if (Outputlink?.GeographicalObjectIn?.Status != Status.Archive)
                {
                    geoObject_AB.OutputTopologyLinks.Add(Outputlink);
                }
            }



            context.Add(geoObject_AB);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            geoObject_A = await repository.Get(geoObject_A.Id);
            geoObject_B = await repository.Get(geoObject_B.Id);
            geoObject_C = await repository.Get(geoObject_C.Id);
            geoObject_D = await repository.Get(geoObject_D.Id);
            geoObject_AB = await repository.Get(geoObject_AB.Id);

            //Assert
            Assert.Equal(5, context.GeoObjects.Count());

            Assert.Equal(1, geoObject_D.OutputTopologyLinks.Count);
            Assert.Equal(1, geoObject_D.InputTopologyLinks.Count);
            Assert.Equal(1, geoObject_C.OutputTopologyLinks.Count);
            Assert.Equal(1, geoObject_D.InputTopologyLinks.Count);

            Assert.Equal("Объект_A", geoObject_AB.ChildGeoObjects[0].ChildGeographicalObject.Name);
            Assert.Equal("Объект_B", geoObject_AB.ChildGeoObjects[1].ChildGeographicalObject.Name);
            Assert.Equal("Объект_AB", geoObject_B.ParentGeoObjects[0].ParentGeographicalObject.Name);
            Assert.Equal("Объект_AB", geoObject_A.ParentGeoObjects[0].ParentGeographicalObject.Name);

            Assert.Equal("Объект_AB", geoObject_C.OutputTopologyLinks[0].GeographicalObjectIn.Name);
            Assert.Equal("Объект_AB", geoObject_D.OutputTopologyLinks[0].GeographicalObjectIn.Name);

            Assert.Equal(Status.Archive, geoObject_A.InputTopologyLinks[0].Status);
            Assert.Equal(Status.Archive, geoObject_A.OutputTopologyLinks[0].Status);
            Assert.Equal(Status.Archive, geoObject_B.InputTopologyLinks[0].Status);
            Assert.Equal(Status.Archive, geoObject_B.OutputTopologyLinks[0].Status);
           
            Assert.Equal(1, geoObject_A.OutputTopologyLinks.Count);
            Assert.Equal(1, geoObject_B.OutputTopologyLinks.Count);
            
            Assert.Equal(2, geoObject_AB.InputTopologyLinks.Count());
            Assert.Equal(2, geoObject_AB.OutputTopologyLinks.Count());

            Assert.Equal("Объект_C", geoObject_AB.InputTopologyLinks[0].GeographicalObjectOut.Name);
            Assert.Equal("Объект_C", geoObject_AB.OutputTopologyLinks[0].GeographicalObjectIn.Name);
            Assert.Equal("Объект_D", geoObject_AB.InputTopologyLinks[1].GeographicalObjectOut.Name);
            Assert.Equal("Объект_D", geoObject_AB.OutputTopologyLinks[1].GeographicalObjectIn.Name);
        }
    }
}
