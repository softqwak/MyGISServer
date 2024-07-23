namespace GISServer.Domain.Model
{
    public interface IGeoObjectsClassifiersRepository
    {
        public Task<List<GeoObjectsClassifiers>> Get();
        public Task<GeoObjectsClassifiers> Get(Guid geoObjectId, Guid classifierId);
        public Task<List<GeoObjectsClassifiers>> GetByGeoObjectId(Guid geoObjectId);
        public Task<List<GeoObjectsClassifiers>> GetByClassifierId(Guid classifierId);
        public Task<(bool, string)> Delete(Guid geoObjectId, Guid classifierId);
        public Task<(bool, string)> Archive(Guid geoObjectId);
    }
}
