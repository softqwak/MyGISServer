// using GISServer.API.Service;
// using GISServer.API.Service.Model;
using GISServer.Domain.Model;
using GISServer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GISServer.Infrastructure.Service
{
    public class TopologyRepository : ITopologyRepository
    {
        private readonly Context _context;

        public TopologyRepository(Context context)
        {
            _context = context;
        }

        // TopologyRepository
        public async Task<TopologyLink> Get(Guid? id)
        {
            try
            {
                var result = await _context.TopologyLinks
                    .Where(tl => tl.Id == id)
                    .FirstOrDefaultAsync();
                return result!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        // TopologyRepository
        public async Task<List<TopologyLink>> Get()
        {
            try
            {
                return await _context.TopologyLinks
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        // TopologyRepository
        public async Task<TopologyLink> Add(TopologyLink topologyLink)
        {
            await _context.TopologyLinks.AddAsync(topologyLink);
            await _context.SaveChangesAsync();
            return await Get(topologyLink.Id);
        }
        public async Task<(bool, string)> DeleteTopologyLink(Guid id)
        {
            var dbTopologyLink = await Get(id);
            if (dbTopologyLink == null)
            {
                return (false, "TopologyLink could not be found");
            }
            _context.TopologyLinks.Remove(dbTopologyLink);
            await _context.SaveChangesAsync();
            return (true, "TopologyLink got deleted");
        }

        
    }
}
