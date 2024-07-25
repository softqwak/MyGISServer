namespace GISServer.Domain.Model
{
    public interface IAspectRepository
    {
        public Task<Aspect> Add(Aspect aspect);
        public Task<Aspect> Get(Guid? id);
        public Task<List<Aspect>> Get();
        public Task<(bool, string)> DeleteAspect(Guid id);
    }
}
