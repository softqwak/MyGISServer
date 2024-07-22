using GISServer.API.Model;
using GISServer.Domain.Model;

namespace GISServer.API.Service
{
    public interface IParentChildService
    {
        public ParentChildObjectLinkDTO CreateGuid(ParentChildObjectLinkDTO parentChildObjectLinkDTO);
        public Task<ParentChildObjectLinkDTO> AddParentChildLink(ParentChildObjectLinkDTO parentChildObjectLinkDTO);
        public Task<List<ParentChildObjectLinkDTO>> GetParentChildLinks();
        public Task<(bool, string)> DeleteParentChildLink(Guid id);
    }
}
