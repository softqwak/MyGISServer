namespace GISServer.Domain.Model
{
    public interface IGeoObjectsClassifiersRepository
    {
        public Task<List<GeoObjectsClassifiers>> GetGeoObjectsClassifiers();
        public Task<GeoObjectsClassifiers> GetGeoObjectsClassifiers(Guid geoObjectId, Guid classifierId);
        public Task<(bool, string)> DeleteGeoObjectClassifier(Guid geoObjectId, Guid classifierId);
    }
}
