using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using GISServer.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GISServer.Tests
{
    public class GeoObjectLinkTests
    {
        private Context GetDatabase()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new Context(options);
        }

        [Fact]
        public async void LinkTest()
        {
            // Arrange
            using var context = GetDatabase();
            TopologyLink topologyLink1 = new TopologyLink()
            {
                Predicate = "212101212",
                Status = Status.Actual,
                CommonBorder = "l1"
            };

            TopologyLink topologyLink2 = new TopologyLink()
            {
                Predicate = "212101212",
                Status = Status.Actual,
                CommonBorder = "l2"
            };

            GeoObject geoObject1 = new GeoObject
            {
                Name = "Объект1",
                Status = Status.Actual

            };
            geoObject1.InputTopologyLinks.Add(topologyLink1);
            geoObject1.OutputTopologyLinks.Add(topologyLink2);

            GeoObject geoObject2 = new GeoObject
            {
                Name = "Объект2",
                Status = Status.Actual
            };

            geoObject2.InputTopologyLinks.Add(topologyLink2);
            geoObject2.OutputTopologyLinks.Add(topologyLink1);
            geoObject2.OutputTopologyLinks[0].Status = Status.Procces;

            GeoObject geo3 = new GeoObject();
            geo3.Union(geoObject1, geoObject2);





            /*  GeoObject geoObj3 = new GeoObject
              {
                  Name = "",
                  InputTopologyLinks = new List<TopologyLink> { topologyLink1},
                  OutputTopologyLinks= new List<TopologyLink> { topologyLink1 }
              };*/

            //context.GeoObjects.AddRange(geoObject1, geoObject2);
            //context.TopologyLinks.AddRange(topologyLink1);
            context.Add(geoObject1);
            context.Add(geoObject2);
            context.Add(geo3);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            // Act
            var geoObjects = await context.GeoObjects.Include(gnf => gnf.GeoNameFeature)
                .Include(gv => gv.GeometryVersion)
                .Include(goi => goi.GeoObjectInfo)
                .Include(pgo => pgo.ParentGeoObjects)
                .Include(cgo => cgo.ChildGeoObjects)
                .Include(otl => otl.OutputTopologyLinks)
                .Include(itl => itl.InputTopologyLinks).AsNoTracking()
                .ToListAsync();

            var Links = context.TopologyLinks.ToList();
            var geoObjectInfos = context.GeoObjectInfos.ToList();
            context.ChangeTracker.Clear();
            // Assert

            Assert.Equal(3, geoObjects.Count);
            
            Assert.Equal(2, Links.Count);
            Assert.Equal(1, geoObject1.InputTopologyLinks.Count);
            Assert.Equal("Объект2", geoObject1.OutputTopologyLinks[0].GeographicalObjectIn.Name);
            Assert.Equal(Status.Archive, geoObject1.Status);
            Assert.Equal(Status.Archive, geoObject2.InputTopologyLinks[0].Status);


            /*Assert.Equal(Status.Actual, geoObject2.Status);
            Assert.NotEmpty(geoObjects);
            Assert.Equal(geoObject1.InputTopologyLinks, geoObject2.InputTopologyLinks);*/

        }

        [Fact]
        public async void LinkTestMany()
        {
            // 1) создаем 2 объекта (А и В) и 2 топологические связи, каждая из которых противополжно друг другу
            // внешняя (Out) или внутрення (In) связь для объектов (A и B) 
            // проверли проверки что все связи есть и для OutLink существ ObjectIn

            // Arrange 
            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);

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

            //Act 
            
            // Сделать через foreach чтобы все связи outObject или inObject перешли в архив если сам объект в архиве
            // Tplink1.Status = Status.Archive;
            // Tplink2.Status = Status.Archive;

            // создаем третий объект для будущего объединения
            GeoObject geoObject_AB = new GeoObject
            {
                Name = "Объект_AB",
                Status = Status.Actual
            };

            geoObject_A.Status = Status.Archive;
            geoObject_B.Status = Status.Archive;

            context.Add(geoObject_AB);
           

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

            //var compareObject = geoObject_C; // сравниваем с объектом C, какие связи у него были с другими объектами
            //противополжные связи out и in у соседних объектов - одинаковые
            // если объект получил статус архивный и имел связи со своим соседом
            // тогда этот сосед будет теперь связан с новым объектом
            // почему-то у объекта А после того как он передает свою связь другому объекту - АВ, она у первого исчезает

           
            // попробуем передать объект С и объект D через цикл 
            var useObject = geoObject_A;
            foreach (var geoObject_1 in context.GeoObjects)
            {
                if (geoObject_1.Status == Status.Actual)
                {
                    useObject = geoObject_1;
                
                   foreach (var geoObject in context.GeoObjects)
                   {

                      for (int i = 0; i < geoObject.OutputTopologyLinks.Count; i++)
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
            
          /*  foreach (var geoObject in context.GeoObjects)
            {

                    for (int i = 0; i < geoObject.OutputTopologyLinks.Count; i++)
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
            }*/

            // проверка на поиск объектов и их количества, будет сохранен последний объект с актуальным статусом
         /*   GeoObject newObject = new GeoObject();
            int k = 0;
            foreach (var geoObject in context.GeoObjects)
            {
                if (geoObject.Status == Status.Actual)
                {
                    newObject = geoObject;
                    k++;
                   // context.GeoObjects.Add(newObject);
                }
            }*/



            
            context.SaveChanges();

            //Assert
            // Assert.Equal("adadad", useObject.Name);
            //Assert.Equal("",compareObject_1.Name);
            // Assert.Equal(2,k );
            //  Assert.Equal("", newObject.Name);

             Assert.Equal(5, context.GeoObjects.Count());
             Assert.Equal(1, geoObject_A.InputTopologyLinks.Count());
             Assert.Equal(Status.Actual, geoObject_B.OutputTopologyLinks[0].Status);
             Assert.Equal(2, geoObject_AB.InputTopologyLinks.Count());
            Assert.Equal(2, geoObject_AB.OutputTopologyLinks.Count());
             Assert.Equal("Объект_C", geoObject_AB.InputTopologyLinks[0].GeographicalObjectOut.Name);
             Assert.Equal("Объект_C", geoObject_AB.OutputTopologyLinks[0].GeographicalObjectIn.Name);
             Assert.Equal("Объект_D", geoObject_AB.InputTopologyLinks[1].GeographicalObjectOut.Name);
             Assert.Equal("Объект_D", geoObject_AB.OutputTopologyLinks[1].GeographicalObjectIn.Name);
            Assert.Equal("Объект_A", geoObject_AB.ChildGeoObjects[0].ChildGeographicalObject.Name);
            Assert.Equal("Объект_B", geoObject_AB.ChildGeoObjects[1].ChildGeographicalObject.Name);
            Assert.Equal(1, geoObject_A.ParentGeoObjects.Count());
            Assert.Equal(1, geoObject_B.ParentGeoObjects.Count());
            Assert.Equal("Объект_AB", geoObject_B.ParentGeoObjects[0].ParentGeographicalObject.Name);
            Assert.Equal("Объект_AB", geoObject_A.ParentGeoObjects[0].ParentGeographicalObject.Name);

            //Assert.Equal("", geoObject_B.InputTopologyLinks[0].GeographicalObjectOut.Name);


            /* Assert.Equal(1, geoObject_A.InputTopologyLinks.Count());
             Assert.Equal(1, geoObject_B.InputTopologyLinks.Count());
             Assert.Equal(1, geoObject_A.OutputTopologyLinks.Count());
             Assert.Equal(1, geoObject_B.OutputTopologyLinks.Count());*/

            /* Assert.Equal("Объект_B", geoObject_A.InputTopologyLinks[0].GeographicalObjectOut.Name);
             Assert.Equal("Объект_A", geoObject_B.OutputTopologyLinks[0].GeographicalObjectIn.Name);
             Assert.Equal(Status.Archive, geoObject_B.OutputTopologyLinks[0].Status);
             Assert.Equal(Status.Archive, geoObject_A.OutputTopologyLinks[0].Status);*/

            /*Assert.Equal(1, geoObject_A.OutputTopologyLinks.Count());
            Assert.Equal(1, geoObject_B.OutputTopologyLinks.Count());*/

            /*Assert.Equal(geoObject_C, repository.GetGeoObject(geoObject_A.OutputTopologyLinks[1].GeographicalObjectIn.Id).Result);

            Assert.Equal(Status.Archive, geoObject_A.Status);
            Assert.Equal(Status.Archive, geoObject_B.Status);
            Assert.Equal(Status.Actual, geoObject_AB.Status);
            Assert.Equal(Status.Actual, geoObject_C.Status);*/



        }
       // [Fact]
        public async void TestRelatives()
        {
            // Arrange
            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);

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
            TopologyLink Tplink3 = new TopologyLink()
            {
                Status = Status.Actual,
            };
            TopologyLink Tplink4 = new TopologyLink()
            {
                Status = Status.Actual
            };

            geoObject_A.InputTopologyLinks.Add(Tplink4);
            geoObject_A.OutputTopologyLinks.Add(Tplink3);
            geoObject_B.InputTopologyLinks.Add(Tplink3);
            geoObject_B.OutputTopologyLinks.Add(Tplink4);

            context.Add(geoObject_A);
            context.Add(geoObject_B);

            //Act 
            GeoObject geoObject_AB = new GeoObject()
            {
                Name = "Объект_AB",
                Status = Status.Actual
            };

            ParentChildObjectLink child1 = new ParentChildObjectLink()
            {
                ChildGeographicalObject = geoObject_A,
                //ParentGeographicalObject = geoObject_AB
            };
            ParentChildObjectLink child2 = new ParentChildObjectLink()
            {
                ChildGeographicalObject = geoObject_B,
               // ParentGeographicalObject = geoObject_AB
            };
           /* ParentChildObjectLink parent1 = new ParentChildObjectLink()
            {
                ParentGeographicalObject = geoObject_AB
            };*/

           // geoObject_A.ParentGeoObjects.Add(child1);
          //  geoObject_B.ParentGeoObjects.Add(child2);

            geoObject_AB.ChildGeoObjects.Add(child1);
            geoObject_AB.ChildGeoObjects.Add(child2);

            context.Add(geoObject_AB);
            context.SaveChanges();

            //Assert
            Assert.Equal(2,geoObject_AB.ChildGeoObjects.Count);
            Assert.Equal(3, context.GeoObjects.Count());
            Assert.Equal("Объект_A", geoObject_AB.ChildGeoObjects[0].ChildGeographicalObject.Name);
            Assert.Equal("Объект_B", geoObject_AB.ChildGeoObjects[1].ChildGeographicalObject.Name);
            Assert.Equal("Объект_A", geoObject_B.OutputTopologyLinks[0].GeographicalObjectIn.Name);
            Assert.Equal(1, geoObject_A.ParentGeoObjects.Count());
            Assert.Equal(1, geoObject_B.ParentGeoObjects.Count());
            Assert.Equal("Объект_AB", geoObject_A.ParentGeoObjects[0].ParentGeographicalObject.Name);
            Assert.Equal("Объект_AB", geoObject_B.ParentGeoObjects[0].ParentGeographicalObject.Name);

            // Assert.Equal(0, geoObject_A.ChildGeoObjects.Count());
            // Assert.Equal(0, geoObject_B.ChildGeoObjects.Count());


        }
    //    [Fact]
        public async void TestLinkBetweenObjects()
        {
            //Arrange

            using var context = GetDatabase();
            var repository = new GeoObjectRepository(context);

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
            
            TopologyLink Tplink1 = new TopologyLink
            {
                Status = Status.Actual
            };
            TopologyLink Tplink2 = new TopologyLink
            {
                Status = Status.Actual
            };
            

            geoObject_A.OutputTopologyLinks.Add(Tplink1);
            geoObject_A.InputTopologyLinks.Add(Tplink2);
            geoObject_B.InputTopologyLinks.Add(Tplink1);
            geoObject_B.OutputTopologyLinks.Add(Tplink2);

            context.Add(geoObject_A);
            context.Add(geoObject_B);
            context.SaveChanges();
            context.ChangeTracker.Clear();

            //Act
            GeoObject geoObject_C = new GeoObject()
            {
                Name = "Объект_C",
                Status = Status.Actual
            };
            geoObject_C.OutputTopologyLinks.Add(Tplink2);
            geoObject_C.InputTopologyLinks.Add(Tplink1);
           // context.GeoObjects.AsNoTracking().ToListAsync();
           // context.TopologyLinks.AsNoTracking().ToListAsync();

            context.Add(geoObject_C);
//          context.SaveChanges();
            context.ChangeTracker.Clear();

            //Assert
            Assert.Equal(2, context.TopologyLinks.Count());
           // Assert.Equal(0, geoObject_B.OutputTopologyLinks.Count);
            Assert.Equal(1, geoObject_A.OutputTopologyLinks.Count);
            Assert.Equal("Объект_A", geoObject_C.InputTopologyLinks[0].GeographicalObjectOut.Name);
            Assert.Equal("Объект_C", geoObject_A.OutputTopologyLinks[0].GeographicalObjectIn.Name);
            Assert.Equal("Объект_A", geoObject_B.OutputTopologyLinks[0].GeographicalObjectIn.Name);
        }

      //  [Fact]
        public async void UnionObjectsAndLinks()
        {
            // Arrange
           using var context = GetDatabase();
           var repository = new GeoObjectRepository(context);
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

            TopologyLink Tplink3 = new TopologyLink()
            {
                Status = Status.Actual,
            };
            TopologyLink Tplink4 = new TopologyLink()
            {
                Status = Status.Actual
            };
            TopologyLink Tplink1 = new TopologyLink()
            {
                Status = Status.Actual,
            };
            TopologyLink Tplink2 = new TopologyLink()
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

            geoObject_A.InputTopologyLinks.Add(Tplink4);
            geoObject_A.OutputTopologyLinks.Add(Tplink3);
            geoObject_B.InputTopologyLinks.Add(Tplink3);
            geoObject_B.OutputTopologyLinks.Add(Tplink4);

            geoObject_A.InputTopologyLinks.Add(Tplink2);
            geoObject_A.OutputTopologyLinks.Add(Tplink1);
            geoObject_C.InputTopologyLinks.Add(Tplink1);
            geoObject_C.OutputTopologyLinks.Add(Tplink2);

            geoObject_B.InputTopologyLinks.Add(Tplink5);
            geoObject_B.OutputTopologyLinks.Add(Tplink6);
            geoObject_D.InputTopologyLinks.Add(Tplink6);
            geoObject_D.OutputTopologyLinks.Add(Tplink5);

            context.Add(geoObject_A);
            context.Add(geoObject_B);
            context.Add(geoObject_C);
            context.Add(geoObject_D);
            

           /* // Создаем новый объект АВ - объединение объектов А и В, он их родитель
            GeoObject geoObject_AB = new GeoObject
            {
                Status = Status.Actual
            };

            context.Add(geoObject_AB);*/
            context.SaveChanges();
           

            //Act 
            // Создаем новый объект АВ - объединение объектов А и В, он их родитель
            GeoObject geoObject_AB = new GeoObject
            {
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
                }
                foreach(var geoObject in context.GeoObjects)
                {
                    for (int i = 0; i < geoObject.OutputTopologyLinks.Count; i++) 
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
            context.Add(geoObject_AB);
            context.SaveChanges();

            // Assert 
            //Assert.Contains(geoObject_AB, context.GeoObjects);
            Assert.Equal(5, context.GeoObjects.Count());
            Assert.Equal(1, geoObject_D.OutputTopologyLinks.Count);
            Assert.Equal(1, geoObject_C.OutputTopologyLinks.Count);
            Assert.Equal("Объект_A", geoObject_AB.ChildGeoObjects[0].ChildGeographicalObject.Name);
            Assert.Equal("Объект_B", geoObject_AB.ChildGeoObjects[1].ChildGeographicalObject.Name);
            //Assert.Equal(2, geoObject_AB.OutputTopologyLinks.Count);
            Assert.Equal("??", geoObject_AB.OutputTopologyLinks[0].GeographicalObjectIn.Name);
        }

    }
}
