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
        public async Task<TopologyLink> GetTopologyLink(Guid? id)
        {
            try
            {
                return await _context.TopologyLinks
                    .Where(tl => tl.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
        }

        // TopologyRepository
        public async Task<List<TopologyLink>> GetTopologyLinks()
        {
            try
            {
                return await _context.TopologyLinks
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
        }

        // TopologyRepository
        public async Task<TopologyLink> AddTopologyLink(TopologyLink topologyLink)
        {
            await _context.TopologyLinks.AddAsync(topologyLink);
            await _context.SaveChangesAsync();
            return await GetTopologyLink(topologyLink.Id);
        }
        public async Task<(bool, string)> DeleteTopologyLink(Guid id)
        {
            var dbTopologyLink = await GetTopologyLink(id);
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
