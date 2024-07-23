using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GISServer.Infrastructure.Service
{
    public class ClassifierRepository : IClassifierRepository
    {
        private readonly Context _context;

        public ClassifierRepository(Context context)
        {
            _context = context;
        }

        // ClassifierRepository
        public async Task<List<Classifier>> GetClassifiers()
        {
            return await _context.Classifiers
                .ToListAsync();
        }

        // ClassifierRepository
        public async Task<Classifier> GetClassifier(Guid? id)
        {
            try
            {
                var result = await _context.Classifiers
                    .Where(ci => ci.Id == id)
                    .FirstOrDefaultAsync();
                return result!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        // ClassifierRepository
        public async Task<Classifier> AddClassifier(Classifier classifier)
        {
            await _context.Classifiers.AddAsync(classifier);
            await _context.SaveChangesAsync();
            return await GetClassifier(classifier.Id);
        }
        public async Task<(bool, string)> DeleteClassifier(Guid id)
        {

            var dbClassifier = await GetClassifier(id);
            if (dbClassifier == null)
            {
                return (false, "GeoObeject could not be found");
            }
            _context.Classifiers.Remove(dbClassifier);
            await _context.SaveChangesAsync();
            return (true, "Classifier got deleted");
        }

        public async Task<(bool, string)> Archive(Guid id)
        {
            var dbClassifier = await GetClassifier(id);

            if (dbClassifier == null)
            {
                return (false, "Classifier could not be found");
            }
            
            dbClassifier.Status = Status.Archive;
            await _context.SaveChangesAsync();
            return (true, "Classifier got archived");
        }
    }
}
