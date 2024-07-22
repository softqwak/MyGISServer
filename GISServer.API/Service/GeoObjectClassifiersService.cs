using GISServer.API.Model;
using GISServer.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using GISServer.API.Mapper;


namespace GISServer.API.Service
{
    public class GeoObjectClassifiersService : IGeoObjectClassifiersService
    {
        private readonly IGeoObjectsClassifiersRepository _repository;
        private readonly GeoObjectClassifiersMapper _geoObjectClassifiersMapper;


        public GeoObjectClassifiersService(IGeoObjectsClassifiersRepository repository, GeoObjectClassifiersMapper geoObjectClassifiersMapper)
        {
            _repository = repository;
            _geoObjectClassifiersMapper = geoObjectClassifiersMapper;
        }


        public async Task<List<GeoObjectsClassifiersDTO>> GetGeoObjectsClassifiers()
        {
            try
            {
                List<GeoObjectsClassifiersDTO> geoObjectsClassifiersDTO = new List<GeoObjectsClassifiersDTO>();
                List<GeoObjectsClassifiers> geoObjectsClassifiers = await _repository.GetGeoObjectsClassifiers();

                foreach (var geoObjectsClassifier in geoObjectsClassifiers)
                {
                    geoObjectsClassifiersDTO.Add(await _geoObjectClassifiersMapper.GOCToDTO(geoObjectsClassifier));
                }
                return geoObjectsClassifiersDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }

        public async Task<(bool, string)> DeleteGeoObjectClassifier(Guid geoObjectId, Guid classifierId)
        {
            try
            {
                return await _repository.DeleteGeoObjectClassifier(geoObjectId, classifierId);
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
        
    }
}
