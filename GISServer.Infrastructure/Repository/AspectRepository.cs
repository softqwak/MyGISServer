using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GISServer.Infrastructure.Service
{
    public class AspectRepository : IAspectRepository
    {
        private readonly Context _context;

        public AspectRepository(Context context)
        {
            _context = context;
        }

        // AspectRepository
        public async Task<Aspect> Get(Guid? id)
        {
            var result = await _context.Aspects
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            return result!;
        }

        // AspectRepository
        public async Task<List<Aspect>> Get()
        {
            return await _context.Aspects
                .ToListAsync();
        }

        // AspectRepository
        public async Task<Aspect> Add(Aspect aspect)
        {
            await _context.AddAsync(aspect);
            await _context.SaveChangesAsync();
            return await Get(aspect.Id);
        }
        public async Task<(bool, string)> DeleteAspect(Guid id)
        {

            var dbAspect = await Get(id);
            if (dbAspect == null)
            {
                return (false, "Aspect could not be found");
            }
            _context.Aspects.Remove(dbAspect);
            await _context.SaveChangesAsync();
            return (true, "Aspect got deleted");
        }
        public async Task<(bool, string)> Archive(Guid id)
        {
            var dbAspect = await Get(id);

            if (dbAspect == null)
            {
                return (false, "Aspect could not be found");
            }

            dbAspect.Status = Status.Archive;
            await _context.SaveChangesAsync();
            return (true, "Aspect got archived");
        }


    }
}
