namespace GISServer.Domain.Model
{
    public interface IClassifierRepository
    {
        public Task<Classifier> Add(Classifier classifier);
        public Task<Classifier> Get(Guid? id);
        public Task<List<Classifier>> Get();

        public Task<(bool, string)> Delete(Guid id);

    }
}
