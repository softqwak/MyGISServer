namespace GISServer.Domain.Model
{
    public interface IParentChildRepository
    {
        public Task<ParentChildObjectLink> Add(ParentChildObjectLink parentChildObjectLink);
        public Task<List<ParentChildObjectLink>> Get();
        public Task<ParentChildObjectLink> Get(Guid? id);
        public Task<(bool, string)> DeleteParentChildLink(Guid id);
    }
}
