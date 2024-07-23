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
        public async Task<List<GeoObjectsClassifiers>> Get()
        {
            return await _context.GeoObjectsClassifiers
                .Include(gogc => gogc.GeoObjectInfo)
                .Include(gogc => gogc.Classifier)
                .ToListAsync();
        }

        public async Task<GeoObjectsClassifiers> Get(Guid geoObjectId, Guid classifierId)
        {
            var result = await _context.GeoObjectsClassifiers
                .Where(e => e.ClassifierId == classifierId && e.GeoObjectId == geoObjectId)
                .FirstOrDefaultAsync();
            return result!;
        }

        public async Task<List<GeoObjectsClassifiers>> GetByGeoObjectId(Guid geoObjectId)
        {
            var relust = await _context.GeoObjectsClassifiers
                .Where(e => e.GeoObjectId == geoObjectId)
                .ToListAsync();
            return relust;
        }

        public async Task<List<GeoObjectsClassifiers>> GetByClassifierId(Guid classifierId)
        {
            var relust = await _context.GeoObjectsClassifiers
                .Where(e => e.ClassifierId == classifierId)
                .ToListAsync();
            return relust;
        }

        public async Task<(bool, string)> Delete(Guid geoObjectId, Guid classifierId)
        {

            var dbGeoObjectClassifier = await Get(geoObjectId, classifierId);
            if (dbGeoObjectClassifier == null)
            {
                return (false, "GeoObejectClassifier could not be found");
            }
            _context.GeoObjectsClassifiers.Remove(dbGeoObjectClassifier);
            await _context.SaveChangesAsync();
            return (true, "GeoObjectClassifier got deleted");
        }
        public async Task<(bool, string)> Archive(Guid geoObjectId)
        {

            var dbGOCs = await GetByGeoObjectId(geoObjectId);
            if (dbGOCs == null)
            {
                return (false, "GeoObject could not be found");
            }

            foreach (var goc in dbGOCs)
            {
                goc.Status = Status.Archive;
            }

            await _context.SaveChangesAsync();
            return (true, "GeoObject got archived");

        }
    }
}
