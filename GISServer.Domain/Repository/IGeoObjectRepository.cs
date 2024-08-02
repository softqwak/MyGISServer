namespace GISServer.Domain.Model
{
    public interface IGeoObjectRepository
    {
        public Task<List<GeoObject>> Get();
        public Task<GeoObject> Get(Guid id);
        // public Task<GeoObject> UpdateGeoObject(GeoObject geoObject);
        public Task<GeoObject> Add(GeoObject geoObject);
        public Task<(bool, string)> Delete(Guid id);
        public Task<(bool, string)> Archive(Guid id);
        public Task UpdateAsync(GeoObject geoObject);
        public Task<GeoObject> GetByNameAsync(string name);

        public Task<List<GeoObjectsClassifiers>> AddClassifier(GeoObjectsClassifiers geoObjectsClassifiers);
        public Task AddClassifier(Guid geoObjectId, Guid classifierId);
        public Task<List<GeoObjectsClassifiers>> GetClassifiers(Guid? geoObjectInfoId);

        public Task<GeoObject> AddAspect(Guid geoObjectId, Guid aspectId);
        public Task<List<Aspect>> GetAspects(Guid geoObjectId);
    }
}
