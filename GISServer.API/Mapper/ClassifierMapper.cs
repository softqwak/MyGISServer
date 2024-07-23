using GISServer.API.Model;
using GISServer.Domain.Model;
using NetTopologySuite.Geometries;


namespace GISServer.API.Mapper
{
    public class ClassifierMapper
    {

        public async Task<Classifier> DTOToClassifier(ClassifierDTO classifierDTO)
        {
            Classifier classifier = new Classifier();
            await Task.Run(async () =>
            {
                classifier.Id = classifierDTO.Id;
                classifier.Name = classifierDTO.Name;
                classifier.Code = classifierDTO.Code;
                classifier.CommonInfo = classifierDTO.CommonInfo;
            });
            return classifier;
        }

        public async Task<ClassifierDTO> ClassifierToDTO(Classifier classifier)
        {

            ClassifierDTO classifierDTO = new ClassifierDTO();
            await Task.Run(async () =>
            {
                classifierDTO.Id = classifier.Id;
                classifierDTO.Name = classifier.Name;
                classifierDTO.Code = classifier.Code;
                classifierDTO.CommonInfo = classifier.CommonInfo;
            });
            return classifierDTO;
        }


    }
}
