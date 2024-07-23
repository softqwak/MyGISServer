using GISServer.API.Model;
using GISServer.API.Mapper;
using GISServer.API.Interface;
using GISServer.Domain.Model;

namespace GISServer.API.Service
{
    public class GeoObjectClassifiersService : IGeoObjectClassifiersService
    {
        private readonly IGeoObjectsClassifiersRepository _repository;
        private readonly GeoObjectClassifiersMapper _mapper;


        public GeoObjectClassifiersService(
            IGeoObjectsClassifiersRepository repository, 
            GeoObjectClassifiersMapper geoObjectClassifiersMapper)
        {
            _repository = repository;
            _mapper = geoObjectClassifiersMapper;
        }


        public async Task<List<GeoObjectsClassifiersDTO>> Get()
        {
            try
            {
                List<GeoObjectsClassifiersDTO> geoObjectsClassifiersDTO = new List<GeoObjectsClassifiersDTO>();
                List<GeoObjectsClassifiers> geoObjectsClassifiers = await _repository.Get();

                foreach (var geoObjectsClassifier in geoObjectsClassifiers)
                {
                    geoObjectsClassifiersDTO.Add(await _mapper.GOCToDTO(geoObjectsClassifier));
                }
                return geoObjectsClassifiersDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }

        public async Task<(bool, string)> Delete(Guid geoObjectId, Guid classifierId)
        {
            try
            {
                return await _repository.Delete(geoObjectId, classifierId);
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
        
    }
}
