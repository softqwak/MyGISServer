namespace GISServer.Domain.Model
{
    public interface IParentChildRepository
    {
        public Task<ParentChildObjectLink> AddParentChildLink(ParentChildObjectLink parentChildObjectLink);
        public Task<List<ParentChildObjectLink>> GetParentChildLinks();
        public Task<ParentChildObjectLink> GetParentChildLink(Guid? id);
        public Task<(bool, string)> DeleteParentChildLink(Guid id);
    }
}
