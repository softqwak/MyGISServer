using GISServer.API.Model;
using GISServer.Domain.Model;

namespace GISServer.API.Interface
{
    public interface IParentChildService
    {
        public ParentChildObjectLinkDTO CreateGuid(ParentChildObjectLinkDTO parentChildObjectLinkDTO);
        public Task<ParentChildObjectLinkDTO> Add(ParentChildObjectLinkDTO parentChildObjectLinkDTO);
        public Task<List<ParentChildObjectLinkDTO>> Get();
        public Task<(bool, string)> DeleteParentChildLink(Guid id);
    }
}
