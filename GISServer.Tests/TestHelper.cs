using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GISServer.Domain.Model;
using GISServer.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISServer.Tests
{
    public class TestHelper
    {
        private readonly Context _context;

        public TestHelper()
        {
            var contextOptions = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;


            _context = new Context(contextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();


            var obj_1 = new GeoObject
            {
                Name = "ОбъектТест_1",
                Status = Status.Actual
            };

            obj_1.InputTopologyLinks.Add(new TopologyLink { Status = Status.Actual, CommonBorder = "l1" });
            obj_1.OutputTopologyLinks.Add(new TopologyLink { Status = Status.Actual, CommonBorder = "l2" });

            _context.GeoObjects.Add(obj_1);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public GeoObjectRepository GeoObjectRepository
        {
            get
            {
                return new GeoObjectRepository(_context);
            }
        }
    }
}
