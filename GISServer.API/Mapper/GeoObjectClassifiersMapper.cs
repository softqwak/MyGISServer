
using GISServer.API.Model;
using GISServer.Domain.Model;
using NetTopologySuite.Geometries;


namespace GISServer.API.Mapper
{
    public class GeoObjectClassifiersMapper
    {
        public async Task<GeoObjectsClassifiersDTO> GOCToDTO(GeoObjectsClassifiers geoObjectsClassifiers)
        {
            await Task.Run(async () =>
            {
                return new GeoObjectsClassifiersDTO
                {
                    GeoObjectId = geoObjectsClassifiers.GeoObjectId,
                    ClassifierId = geoObjectsClassifiers.ClassifierId
                };
            });
            return null;
        }

        public async Task<GeoObjectsClassifiers> DTOToGOC(GeoObjectsClassifiersDTO geoObjectsClassifiersDTO)
        {
            await Task.Run(async () =>
            {
                return new GeoObjectsClassifiers
                {
                    GeoObjectId = geoObjectsClassifiersDTO.GeoObjectId,
                    ClassifierId = geoObjectsClassifiersDTO.ClassifierId
                };
            });
            return null;
        }
    }
}
