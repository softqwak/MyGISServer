using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GISServer.Domain.Model;
using GISServer.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISServer.Tests.Data
{
    public class TestData
    {
        public readonly IGeoObjectRepository _geoObjectRepository;
        public readonly IClassifierRepository _classifierRepository;
        public readonly IAspectRepository _aspectRepository;
        public readonly IGeoObjectsClassifiersRepository _gocRepository;
        public readonly IParentChildRepository _parentChildRepository;
        public readonly ITopologyRepository _topologyRepository;
        public readonly Context _context;

        public int _geoObjectCount { get; set; }
        public int _classifierCount { get; set; }
        public int _aspectCount { get; set; }
        public int _topologyCount { get; set; }
        public int _parentChildCount { get; set; }

        public List<GeoObject> objects = new List<GeoObject>();
        public List<Aspect> aspects = new List<Aspect>();
        public List<TopologyLink> topologyLinks = new List<TopologyLink>();
        public List<ParentChildObjectLink> parentChildLinks = new List<ParentChildObjectLink>();

        public TestData()
        {
            var contextOptions = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new Context(contextOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _geoObjectRepository = new GeoObjectRepository(_context);
            _classifierRepository = new ClassifierRepository(_context);
            _aspectRepository = new AspectRepository(_context);
            _gocRepository = new GeoObjectsClassifiersRepository(_context);
            _parentChildRepository = new ParentChildRepository(_context);
            _topologyRepository = new TopologyRepository(_context);

            CreateDataAsync().Wait();
        }

        public async Task InitDataAsync()
        {
            await Task.Run(async () =>
            {
                objects = await _geoObjectRepository.Get();
                aspects = await _aspectRepository.Get();
                topologyLinks = await _topologyRepository.Get();
                parentChildLinks = await _parentChildRepository.Get();
            });
        }

        // 10 obj, 4 classes, 10 aspects, 2 topolink, 2 parlink 
        public async Task CreateDataAsync()
        {
            await Task.Run(async () =>
            {
                for (_geoObjectCount = 0; _geoObjectCount < 10; _geoObjectCount++)
                {
                    await _geoObjectRepository.Add(CreateGeoObject());
                }
                for (_classifierCount = 0; _classifierCount < 4; _classifierCount++)
                {
                    await _classifierRepository.Add(CreateClassifier());
                }
                for (_aspectCount = 0; _aspectCount < 10; _aspectCount++)
                {
                    await _aspectRepository.Add(CreateAspect());
                }
                for (_topologyCount = 0; _topologyCount < 2; _topologyCount++)
                {
                    await _topologyRepository.Add(CreateTopologyLink());
                }
                for (_parentChildCount = 0; _parentChildCount < 2; _parentChildCount++)
                {
                    await _parentChildRepository.Add(CreateParentChildObjectLink());
                }
            });
        }

        public GeoObject CreateGeoObject()
        {
            var guid = Guid.NewGuid();
            return new GeoObject
            {
                Id = guid,
                Name = $"obj{_geoObjectCount}",
                Status = Status.Actual,
                CreationTime = DateTime.UtcNow,
                Geometry = new GeometryVersion
                {
                    Id = guid,
                    Status = Status.Actual,
                    BorderGeocodes = "\"type\":\"Point\",\"coordinates\":[5, 5]"
                },
                GeoObjectInfo = new GeoObjectInfo
                {
                    Id = guid,
                    Status = Status.Actual,
                    CommonInfo = "common info",
                    Classifiers = new List<Classifier>()
                },
                Aspects = new List<Aspect>(),
                ParentGeoObjects = new List<ParentChildObjectLink>(),
                ChildGeoObjects = new List<ParentChildObjectLink>(),
                OutputTopologyLinks = new List<TopologyLink>(),
                InputTopologyLinks = new List<TopologyLink>()
            };
        }

        public Classifier CreateClassifier()
        {
            var guid = Guid.NewGuid();
            return new Classifier
            {
                Id = guid,
                Name = $"class{_classifierCount}",
                Status = Status.Actual
            };
        }

        public Aspect CreateAspect()
        {
            var guid = Guid.NewGuid();
            return new Aspect
            {
                Id = guid,
                Status = Status.Actual,
                Type = $"aspect{_aspectCount}"
            };
        }

        public TopologyLink CreateTopologyLink()
        {
            var guid = Guid.NewGuid();
            return new TopologyLink
            {
                Id = guid,
                Status = Status.Actual,
                Predicate = $"topology{_aspectCount}"
            };
        }

        public ParentChildObjectLink CreateParentChildObjectLink()
        {
            var guid = Guid.NewGuid();
            return new ParentChildObjectLink
            {
                Id = guid,
                Status = Status.Actual
            };
        }

    }
}