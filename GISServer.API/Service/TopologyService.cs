using GISServer.API.Model;
using GISServer.API.Mapper;
using GISServer.API.Interface;
using GISServer.API.Service.Model;
using GISServer.Domain.Model;
using System.Text.Json;
using NetTopologySuite.Geometries;


namespace GISServer.API.Service
{
    public class TopologyService : ITopologyService
    {
        private readonly ITopologyRepository _repository;
        private readonly IGeoObjectRepository _geoObjectrepository;
        private readonly TopologyMapper _topologyMapper;

        public TopologyService(ITopologyRepository repository, IGeoObjectRepository geoObjectrepository, TopologyMapper topologyMapper)
        {
            _repository = repository;
            _topologyMapper = topologyMapper;
            _geoObjectrepository = geoObjectrepository;
        }

        public TopologyLinkDTO CreateGuid(TopologyLinkDTO topologyLinkDTO)
        {
            topologyLinkDTO.Id = Guid.NewGuid();
            return topologyLinkDTO;
        }

        public async Task<TopologyLinkDTO> AddTopologyLink(TopologyLinkDTO topologyLinkDTO)
        {
            try
            {
                topologyLinkDTO = CreateGuid(topologyLinkDTO);

                topologyLinkDTO = await GetCommonBorder(topologyLinkDTO);

                TopologyLink topologyLink = await _topologyMapper.DTOToTopologyLink(topologyLinkDTO);

                return await _topologyMapper.TopologyLinkToDTO(await _repository.AddTopologyLink(topologyLink));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                throw;
            }
        }

        public async Task<List<TopologyLinkDTO>> GetTopologyLinks()
        {
            try
            {
                List<TopologyLinkDTO> topologyLinksDTO = new List<TopologyLinkDTO>();
                List<TopologyLink> topologyLinks = await _repository.GetTopologyLinks();
                foreach (var topologyLink in topologyLinks)
                {
                    topologyLinksDTO.Add(await _topologyMapper.TopologyLinkToDTO(topologyLink));
                }
                return topologyLinksDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                throw;
            }
        }
        public async Task<(bool, string)> DeleteTopologyLink(Guid id)
        {
            try
            {
                return await _repository.DeleteTopologyLink(id);
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }

        public async Task<TopologyLinkDTO> GetCommonBorder(TopologyLinkDTO topologyLinkDTO)
        {
            try
            {
                CommonBorder border = new CommonBorder();
                var geometryFactory = new GeometryFactory();
                GeoObject objectIn = new();
                GeoObject objectOut = new();
                BorderGeocodes borderGeoCodesObjectIn = new();
                BorderGeocodes borderGeoCodesObjectOut = new();

                if (topologyLinkDTO.GeographicalObjectInId is not null && topologyLinkDTO.GeographicalObjectOutId is not null)
                {
                    objectIn = await _geoObjectrepository.Get((Guid)topologyLinkDTO.GeographicalObjectInId);
                    objectOut = await _geoObjectrepository.Get((Guid)topologyLinkDTO.GeographicalObjectOutId);
                }
                else return topologyLinkDTO;

                if (objectIn.Geometry.BorderGeocodes is not null && objectOut.Geometry.BorderGeocodes is not null)
                {
                    borderGeoCodesObjectIn = JsonSerializer.Deserialize<BorderGeocodes>(objectIn.Geometry.BorderGeocodes);
                    borderGeoCodesObjectOut = JsonSerializer.Deserialize<BorderGeocodes>(objectOut.Geometry.BorderGeocodes);
                }
                else return topologyLinkDTO;

                List<Coordinate> coordsIn = new List<Coordinate>();
                List<Coordinate> coordsOut = new List<Coordinate>();


                for (int i = 0; i < borderGeoCodesObjectIn.coordinates.Count; ++i)
                {
                    coordsIn.Add(new Coordinate
                    {
                        X = borderGeoCodesObjectIn.coordinates[i][0],
                        Y = borderGeoCodesObjectIn.coordinates[i][1]
                    });
                }

                for (int i = 0; i < borderGeoCodesObjectOut.coordinates.Count; ++i)
                {
                    coordsOut.Add(new Coordinate
                    {
                        X = borderGeoCodesObjectOut.coordinates[i][0],
                        Y = borderGeoCodesObjectOut.coordinates[i][1]
                    });
                }

                if (coordsIn[0] != coordsIn[coordsIn.Count - 1])
                {
                    coordsIn.Add(new Coordinate
                    {
                        X = borderGeoCodesObjectIn.coordinates[0][0],
                        Y = borderGeoCodesObjectIn.coordinates[0][1]
                    });
                }

                if (coordsOut[0] != coordsOut[coordsOut.Count - 1])
                {
                    coordsOut.Add(new Coordinate
                    {
                        X = borderGeoCodesObjectOut.coordinates[0][0],
                        Y = borderGeoCodesObjectOut.coordinates[0][1]
                    });
                }

                var polygonIn = geometryFactory.CreatePolygon(coordsIn.ToArray());
                var polygonOut = geometryFactory.CreatePolygon(coordsOut.ToArray());

                var intersection = polygonIn.Intersection(polygonOut);

                if (intersection.GeometryType == "LineString")
                {
                    border.type = intersection.GeometryType;

                    foreach (var item in intersection.Coordinates)
                    {
                        border.coordinates.Add(new double[] { item.X, item.Y });
                    }

                    topologyLinkDTO.CommonBorder = JsonSerializer.Serialize(border);
                }

                return topologyLinkDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return topologyLinkDTO;
            }
        }
    }
}
