namespace GISServer.Domain.Model
{
    public interface IGeoObjectRepository
    {
        public Task<List<GeoObject>> GetGeoObjects();
        public Task<GeoObject> GetGeoObject(Guid id);
        public Task<GeoObject> UpdateGeoObject(GeoObject geoObject);
        public Task<GeoObject> AddGeoObject(GeoObject geoObject);
        public Task<(bool, string)> DeleteGeoObject(Guid id);
        public Task UpdateAsync(GeoObject geoObject);
        public Task<GeoObject> GetByNameAsync(string name);

        public Task<List<GeoObjectsClassifiers>> AddGeoObjectsClassifiers (GeoObjectsClassifiers geoObjectsClassifiers);
        public Task<List<GeoObjectsClassifiers>> GetGeoObjectsClassifiers(Guid? geoObjectInfoId);

        public Task<GeoObject> AddGeoObjectAspect(Guid geoObjectId, Guid aspectId);
        public Task<List<Aspect>> GetGeoObjectAspects(Guid geoObjectId);
    }
}
