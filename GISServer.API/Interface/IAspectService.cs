using GISServer.API.Model;
using GISServer.Domain.Model;

namespace GISServer.API.Interface
{
    public interface IAspectService
    {
        public AspectDTO InitAspect(AspectDTO aspectDTO);
        public Task<AspectDTO> Add(AspectDTO AspectDTO);
        public Task<AspectDTO> Get(Guid id);
        public Task<List<AspectDTO>> Get();
        public Task<String> CallAspect(String endPoint);
        public Task<(bool, string)> DeleteAspect(Guid id);
    }
}
