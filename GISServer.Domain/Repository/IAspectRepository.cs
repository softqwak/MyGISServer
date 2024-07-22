namespace GISServer.Domain.Model
{
    public interface IAspectRepository
    {
        public Task<Aspect> AddAspect(Aspect aspect);
        public Task<Aspect> GetAspect(Guid? id);
        public Task<List<Aspect>> GetAspects();
        public Task<(bool, string)> DeleteAspect(Guid id);
    }
}
