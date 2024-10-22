using GISServer.Infrastructure.Data;
using GISServer.API.Interface;
using GISServer.API.Service;
using Microsoft.EntityFrameworkCore;
using GISServer.Domain.Model;
using GISServer.API.Mapper;
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

        public readonly IGeoObjectService _geoObjectService;
        public readonly IClassifierService _classifierService;
        public readonly IAspectService _aspectService;
        public readonly IGeoObjectClassifiersService _gocService;
        public readonly IParentChildService _parentChildService;
        public readonly ITopologyService _topologyService;

        public readonly GeoObjectMapper _geoObjectMapper;
        public readonly ClassifierMapper _classifierMapper;
        public readonly AspectMapper _aspectMapper;
        public readonly GeoObjectClassifiersMapper _gocMapper;
        public readonly ParentChildMapper _parentChildMapper;
        public readonly TopologyMapper _topologyMapper;

        public readonly Context _context;

        public int _geoObjectCount { get; set; }
        public int _classifierCount { get; set; }
        public int _aspectCount { get; set; }
        public int _topologyCount { get; set; }
        public int _parentChildCount { get; set; }

        public List<GeoObject> objects = new List<GeoObject>();
        public List<Aspect> aspects = new List<Aspect>();
        public List<Classifier> classifiers = new List<Classifier>();
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

            _geoObjectMapper = new GeoObjectMapper();
            _gocMapper = new GeoObjectClassifiersMapper();
            _classifierMapper = new ClassifierMapper();
            _aspectMapper = new AspectMapper();
            _parentChildMapper = new ParentChildMapper();
            _topologyMapper = new TopologyMapper();

            _geoObjectService = new GeoObjectService(
                _geoObjectRepository,
                _classifierRepository,
                _aspectRepository,
                _gocRepository,
                _parentChildRepository,
                _topologyRepository,
                _geoObjectMapper,
                _classifierMapper,
                _aspectMapper
            );

            _classifierService = new ClassifierService(
                _classifierRepository,
                _classifierMapper
            );

            _aspectService = new AspectService(
                _aspectRepository,
                _aspectMapper 
            );

            _gocService = new GeoObjectClassifiersService(
                _gocRepository,
                _gocMapper 
            );

            _parentChildService = new ParentChildService(
                _parentChildRepository,
                _parentChildMapper
            );

            _topologyService = new TopologyService(
                _topologyRepository,
                _geoObjectRepository,
                _topologyMapper
            );

            CreateDataAsync().Wait();
        }

        public async Task InitDataAsync()
        {
            await Task.Run(async () =>
            {
                var objectsDTO = await _geoObjectService.Get();
                foreach(var objectDTO in objectsDTO)
                    objects.Add(
                            await _geoObjectMapper.DTOToObject(
                                objectDTO));
                    
                var classifiersDTO = await _classifierService.Get();
                foreach(var classifierDTO in classifiersDTO)
                    classifiers.Add(
                            await _classifierMapper.DTOToClassifier(
                                classifierDTO));

                var aspectsDTO = await _aspectService.Get();
                foreach(var aspectDTO in aspectsDTO)
                    aspects.Add(
                            await _aspectMapper.DTOToAspect(
                                aspectDTO));

                var topologyLinksDTO = await _topologyService.Get();
                foreach(var topologyLinkDTO in topologyLinksDTO)
                    topologyLinks.Add(
                            await _topologyMapper.DTOToTopologyLink(
                                topologyLinkDTO));

                var parentChildLinksDTO = await _parentChildService.Get();
                foreach(var parentChildLinkDTO in parentChildLinksDTO)
                    parentChildLinks.Add(
                            await _parentChildMapper.DTOToParentChildObjectLink(
                                parentChildLinkDTO));
            });
        }

        // 10 obj, 4 classes, 10 aspects, 2 topolink, 2 parlink 
        public async Task CreateDataAsync()
        {
            await Task.Run(async () =>
            {
                for (_geoObjectCount = 0; _geoObjectCount < 10; _geoObjectCount++)
                {
                    await _geoObjectService.Add(
                            await _geoObjectMapper.ObjectToDTO(
                                CreateGeoObject()));
                }
                for (_classifierCount = 0; _classifierCount < 4; _classifierCount++)
                {
                    await _classifierService.Add(
                            await _classifierMapper.ClassifierToDTO(
                                CreateClassifier()));
                }
                for (_aspectCount = 0; _aspectCount < 10; _aspectCount++)
                {
                    await _aspectService.Add(
                            await _aspectMapper.AspectToDTO(
                                CreateAspect()));
                }
                for (_topologyCount = 0; _topologyCount < 2; _topologyCount++)
                {
                    await _topologyService.Add(
                            await _topologyMapper.TopologyLinkToDTO(
                                CreateTopologyLink()));
                }
                for (_parentChildCount = 0; _parentChildCount < 2; _parentChildCount++)
                {
                    await _parentChildService.Add(
                            await _parentChildMapper.ParentChildObjectLinkToDTO(
                                CreateParentChildObjectLink()));
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
