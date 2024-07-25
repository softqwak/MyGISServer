using GISServer.API.Model;
using GISServer.Domain.Model;

namespace GISServer.API.Interface
{
    public interface IClassifierService
    {
        public ClassifierDTO Init(ClassifierDTO classifierDTO);
        public Task<ClassifierDTO> Add(ClassifierDTO classifierDTO);
        public Task<ClassifierDTO> Get(Guid id);
        public Task<List<ClassifierDTO>> Get();
        public Task<(bool, string)> Delete(Guid id);
    }
}
