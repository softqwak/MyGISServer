using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GISServer.Infrastructure.Service
{
    public class GeoObjectsClassifiersRepository : IGeoObjectsClassifiersRepository
    {
        private readonly Context _context;

        public GeoObjectsClassifiersRepository(Context context)
        {
            _context = context;
        }

        // GeoObjectsCLassifiersRepository
        public async Task<List<GeoObjectsClassifiers>> GetGeoObjectsClassifiers()
        {
            return await _context.GeoObjectsClassifiers
                .Include(gogc => gogc.GeoObjectInfo)
                .Include(gogc => gogc.Classifier)
                .ToListAsync();
        }

        public async Task<GeoObjectsClassifiers> GetGeoObjectsClassifiers(Guid geoObjectId, Guid classifierId)
        {
            var result = await _context.GeoObjectsClassifiers
                .Where(e => e.ClassifierId == classifierId && e.GeoObjectId == geoObjectId)
                .FirstOrDefaultAsync();
            return result!;
        }

        public async Task<(bool, string)> DeleteGeoObjectClassifier(Guid geoObjectId, Guid classifierId)
        {

            var dbGeoObjectClassifier = await GetGeoObjectsClassifiers(geoObjectId, classifierId);
            if (dbGeoObjectClassifier == null)
            {
                return (false, "GeoObejectClassifier could not be found");
            }
            _context.GeoObjectsClassifiers.Remove(dbGeoObjectClassifier);
            await _context.SaveChangesAsync();
            return (true, "GeoObjectClassifier got deleted");
        }
    }
}
