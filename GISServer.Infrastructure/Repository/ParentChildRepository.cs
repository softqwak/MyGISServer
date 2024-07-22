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

        public async Task<ParentChildObjectLink> GetParentChildLink(Guid? id)
        {
            try
            {
                return await _context.ParentChildObjectLinks
                    .Where(pcol => pcol.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
                
        }

        public async Task<List<ParentChildObjectLink>> GetParentChildLinks()
        {
            try
            {
                return await _context.ParentChildObjectLinks
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
        }

        public async Task<ParentChildObjectLink> AddParentChildLink(ParentChildObjectLink parentChildObjectLink)
        {
            await _context.ParentChildObjectLinks.AddAsync(parentChildObjectLink);
            await _context.SaveChangesAsync();
            return await GetParentChildLink(parentChildObjectLink.Id);
        }

        public async Task<(bool, string)> DeleteParentChildLink(Guid id)
        {
            var dbParentChildLink = await GetParentChildLink(id);
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
