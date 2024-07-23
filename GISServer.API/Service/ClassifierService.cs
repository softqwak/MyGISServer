using GISServer.API.Model;
using GISServer.Domain.Model;
using GISServer.API.Mapper;
using GISServer.Infrastructure.Service;
using GISServer.API.Interface;


namespace GISServer.API.Service
{
    public class ClassifierService : IClassifierService
    {
        private readonly IClassifierRepository _repository;
        private readonly ClassifierMapper _classifierMapper;

        public ClassifierService(IClassifierRepository repository, ClassifierMapper classifierMapper)
        {
            _repository = repository;
            _classifierMapper = classifierMapper;
        }

        public ClassifierDTO InitClassifier(ClassifierDTO classifierDTO)
        {
            classifierDTO.Id = Guid.NewGuid();
            return classifierDTO;
        }

        public async Task<ClassifierDTO> AddClassifier(ClassifierDTO classifierDTO)
        {
            try
            {
                classifierDTO = InitClassifier(classifierDTO);
                Classifier classifier = await _classifierMapper.DTOToClassifier(classifierDTO);
                return await _classifierMapper.ClassifierToDTO(await _repository.AddClassifier(classifier));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex. Message}");
                return null;
            }
        }
        
        public async Task<ClassifierDTO> GetClassifier(Guid id)
        {
            try
            {
                ClassifierDTO classifierDTO = await _classifierMapper.ClassifierToDTO(await _repository.GetClassifier(id));
                return classifierDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<ClassifierDTO>> GetClassifiers()
        {
            try
            {
                List<Classifier> classifiersFromDB = new List<Classifier>(await _repository.GetClassifiers());
                List<ClassifierDTO> classifiers = new List<ClassifierDTO>();
                foreach (var classifier in classifiersFromDB)
                {
                    classifiers.Add(await _classifierMapper.ClassifierToDTO(classifier));
                }
                return classifiers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<(bool, string)> DeleteClassifier(Guid id)
        {
            try
            {
                return await _repository.DeleteClassifier(id);
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
