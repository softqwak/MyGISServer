namespace GISServer.Domain.Model
{
    public interface IClassifierRepository
    {
        public Task<Classifier> AddClassifier(Classifier classifier);
        public Task<Classifier> GetClassifier(Guid? id);
        public Task<List<Classifier>> GetClassifiers();

        public Task<(bool, string)> DeleteClassifier(Guid id);

    }
}
