using GISServer.API.Model;
using GISServer.Domain.Model;

namespace GISServer.API.Service.Interface
{
    public interface IAspectService
    {
        public AspectDTO InitAspect(AspectDTO aspectDTO);
        public Task<AspectDTO> AddAspect(AspectDTO AspectDTO);
        public Task<AspectDTO> GetAspect(Guid id);
        public Task<List<AspectDTO>> GetAspects();
        public String CallAspect(String endPoint);
        public Task<(bool, string)> DeleteAspect(Guid id);
    }
}
