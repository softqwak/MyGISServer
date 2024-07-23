using GISServer.API.Model;
using GISServer.Domain.Model;

namespace GISServer.API.Interface
{
    public interface IGeoObjectClassifiersService
    {
        public Task<List<GeoObjectsClassifiersDTO>> Get();
        public Task<(bool, string)> Delete(Guid geoObjectId, Guid classifierId);
    }
}
