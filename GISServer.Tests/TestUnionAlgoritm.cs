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
    public class TestUnionAlgoritm
    {
        private Context GetDatabase()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new Context(options);
        }

        [Fact]
        public async void TestUnion()
        {
            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);
            //Arrange
            // Создаем 4 объекта: А, В, C и D делаем для них связи, добавляем их в БД
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
            
           geoObject_A = await repository.GetGeoObject(geoObject_A.Id);
           geoObject_B = await repository.GetGeoObject(geoObject_B.Id);
           


            // Создаем новый объект АВ - объединение объектов А и В, он их родитель
            GeoObject geoObject_AB = new GeoObject
            {
                Name = "Объект_AB",
                Status = Status.Actual
            };

            //Делаем объекты A и В архивными 
            geoObject_A.Status = Status.Archive;
            geoObject_B.Status = Status.Archive;

            // создаем объект родительско-дочерного класса, установить подобного рода связь между дочерними объектами А и B с объектом AB
            ParentChildObjectLink child_A = new ParentChildObjectLink()
            {
                ChildGeographicalObject = geoObject_A
            };
            ParentChildObjectLink child_B = new ParentChildObjectLink()
            {
                ChildGeographicalObject = geoObject_B
            };

            geoObject_AB.ChildGeoObjects.Add(child_A);
            geoObject_AB.ChildGeoObjects.Add(child_B);
            

            // производим перебор между архивными и акутальными объектами и их связми
            // если у них была общая связь, то она передается объекту АВ

            var useObject = geoObject_A; // вспомогальная переменная нужна для поиска актуального объекта 
            foreach (var geoObject_1 in context.GeoObjects)
            {
                if (geoObject_1.Status == Status.Actual)
                {
                    useObject = geoObject_1;

                    foreach(var geoObject in context.GeoObjects)
                    {
                        for (int i = 0; i <  geoObject.OutputTopologyLinks.Count; i++)
                        {
                            for (int j = 0; j < useObject.InputTopologyLinks.Count; j++)
                            {
                                if ((geoObject.Status == Status.Archive) && (useObject.InputTopologyLinks[j] == geoObject.OutputTopologyLinks[i]))
                                {
                                    geoObject.OutputTopologyLinks[i].Status = Status.Archive;
                                    geoObject.InputTopologyLinks[i].Status = Status.Archive;

                                    geoObject_AB.InputTopologyLinks.Add(useObject.OutputTopologyLinks[j]);
                                    geoObject_AB.OutputTopologyLinks.Add(useObject.InputTopologyLinks[j]);
                                }
                            }
                        }
                    }
                }
            }

            context.Add(geoObject_AB);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            geoObject_A = await repository.GetGeoObject(geoObject_A.Id);
            geoObject_B = await repository.GetGeoObject(geoObject_B.Id);
            geoObject_C = await repository.GetGeoObject(geoObject_C.Id);
            geoObject_D = await repository.GetGeoObject(geoObject_D.Id);
            geoObject_AB = await repository.GetGeoObject(geoObject_AB.Id);
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

            Assert.Equal(2, geoObject_AB.InputTopologyLinks.Count());
            Assert.Equal(2, geoObject_AB.OutputTopologyLinks.Count());
            Assert.Equal("Объект_C", geoObject_AB.InputTopologyLinks[0].GeographicalObjectOut.Name);
            Assert.Equal("Объект_C", geoObject_AB.OutputTopologyLinks[0].GeographicalObjectIn.Name);
            Assert.Equal("Объект_D", geoObject_AB.InputTopologyLinks[1].GeographicalObjectOut.Name);
            Assert.Equal("Объект_D", geoObject_AB.OutputTopologyLinks[1].GeographicalObjectIn.Name);


        }
    }
}
