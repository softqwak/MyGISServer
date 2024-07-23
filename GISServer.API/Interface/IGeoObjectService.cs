using GISServer.API.Model;
using GISServer.Domain.Model;

namespace GISServer.API.Interface
{
    public interface IGeoObjectService
    {
        public GeoObjectDTO Init(GeoObjectDTO geoObjectDTO);
        public Task<List<GeoObjectDTO>> Get();
        public Task<GeoObjectDTO> Get(Guid id);
        public Task<GeoObjectDTO> Update(GeoObjectDTO geoObjectDTO);
        public Task<GeoObjectDTO> Add(GeoObjectDTO geoObjectDTO);
        public Task<(bool, string)> Delete(Guid id);   
        public Task<(bool, string)> Archive(Guid id);   

        public Task<GeoObjectsClassifiersDTO> AddClassifier(GeoObjectsClassifiersDTO geoObjectsClassifiersDTO);
        public Task<List<GeoObjectsClassifiers>> GetClassifiers(Guid? geoObjectInfoId);

        public Task<GeoObjectDTO> AddAspect(Guid geoObjectId, Guid aspectId);
        public Task<List<AspectDTO>> GetAspects(Guid geoObjectId);
    }
}
