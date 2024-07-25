using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GISServer.Infrastructure.Service
{
    public class ParentChildRepository : IParentChildRepository
    {
        private readonly Context _context;

        public ParentChildRepository(Context context)
        {
            _context = context;
        }

        public async Task<ParentChildObjectLink> Get(Guid? id)
        {
            try
            {
                var result = await _context.ParentChildObjectLinks
                    .Where(pcol => pcol.Id == id)
                    .FirstOrDefaultAsync();
                return result!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
                
        }

        public async Task<List<ParentChildObjectLink>> Get()
        {
            try
            {
                return await _context.ParentChildObjectLinks
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<ParentChildObjectLink> Add(ParentChildObjectLink parentChildObjectLink)
        {
            await _context.ParentChildObjectLinks.AddAsync(parentChildObjectLink);
            await _context.SaveChangesAsync();
            return await Get(parentChildObjectLink.Id);
        }

        public async Task<(bool, string)> DeleteParentChildLink(Guid id)
        {
            var dbParentChildLink = await Get(id);
            if (dbParentChildLink == null)
            {
                return (false, "ParentChildLink could not be found");
            }
            _context.ParentChildObjectLinks.Remove(dbParentChildLink);
            await _context.SaveChangesAsync();
            return (true, "ParentChildLink got deleted");
        }

    }
}
