using GISServer.API.Model;
using GISServer.API.Mapper;
using GISServer.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace GISServer.API.Service
{
    public class GeoObjectService : IGeoObjectService
    {
        private readonly IGeoObjectRepository _geoObjectRepository;
        private readonly IClassifierRepository _classifierRepository;
        private readonly GeoObjectMapper _geoObjectMapper;
        private readonly AspectMapper _aspectMapper;
        private readonly ClassifierMapper _classifierMapper;

        public GeoObjectService(
                IGeoObjectRepository repository, 
                IClassifierRepository classifierRepository,
                GeoObjectMapper geoObjectMapper, 
                ClassifierMapper classifierMapper,
                AspectMapper aspectMapper)
        {
            _geoObjectRepository = repository;
            _classifierRepository = classifierRepository;
            _geoObjectMapper = geoObjectMapper;
            _classifierMapper = classifierMapper;
            _aspectMapper = aspectMapper;
        }

        public GeoObjectDTO InitGeoObject(GeoObjectDTO geoObjectDTO)
        {
            Guid guid = Guid.NewGuid();

            geoObjectDTO.Id = guid;
            geoObjectDTO.GeoObjectInfo.Id = guid;
            geoObjectDTO.Geometry.Id = guid;
            
            geoObjectDTO.Status = Status.Actual;
            geoObjectDTO.UpdateTime = DateTime.UtcNow;
            geoObjectDTO.CreationTime = DateTime.UtcNow;

            return geoObjectDTO;
        }

        public async Task<GeoObjectDTO> AddGeoObject(GeoObjectDTO geoObjectDTO)
        {
            try
            {
                geoObjectDTO = InitGeoObject(geoObjectDTO);
                GeoObject geoObject = await _geoObjectMapper.DTOToObject(geoObjectDTO);
                return await _geoObjectMapper.ObjectToDTO(await _geoObjectRepository.AddGeoObject(geoObject));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<GeoObjectDTO>> GetGeoObjects()
        {
            try
            {
                List<GeoObject> geoObjectsFromDB = new List<GeoObject>(await _geoObjectRepository.GetGeoObjects());
                List<GeoObjectDTO> geoObjects = new List<GeoObjectDTO>();
                foreach (var geoObject in geoObjectsFromDB)
                {
                    List<GeoObjectsClassifiers> geoObjectsClassifiersFromDB = new List<GeoObjectsClassifiers>(
                            await _geoObjectRepository.GetGeoObjectsClassifiers(geoObject.Id));

                    foreach (var goc in geoObjectsClassifiersFromDB)
                    {
                        Console.WriteLine(goc.ClassifierId);
                        geoObject.GeoObjectInfo.Classifiers.Add(
                                await _classifierRepository.GetClassifier(goc.ClassifierId));

                    }

                    geoObjects.Add(await _geoObjectMapper.ObjectToDTO(geoObject));
                }
                return geoObjects;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\n{ex.ToString()}\n\n");
                return null;
            }
        }
        public async Task<GeoObjectDTO> GetGeoObject(Guid id)
        {
            try
            {
                GeoObjectDTO geoObject = await _geoObjectMapper.ObjectToDTO(await _geoObjectRepository.GetGeoObject(id));
                
                List<GeoObjectsClassifiers> geoObjectsClassifiersFromDB = new List<GeoObjectsClassifiers>(
                        await _geoObjectRepository.GetGeoObjectsClassifiers(id));

                foreach (var gogc in geoObjectsClassifiersFromDB)
                {
                   geoObject.GeoObjectInfo.Classifiers.Add(
                       await _classifierMapper.ClassifierToDTO(
                           await _classifierRepository.GetClassifier(gogc.ClassifierId)));

                }

                return geoObject;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<GeoObjectDTO> UpdateGeoObject(GeoObjectDTO geoObjectDTO)
        {
            try
            {
                GeoObject geoObject = await _geoObjectMapper.DTOToObject(geoObjectDTO);
                await _geoObjectRepository.UpdateAsync(geoObject);
                return geoObjectDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<(bool, string)> DeleteGeoObject(Guid id)
        {
            try
            {
                return await _geoObjectRepository.DeleteGeoObject(id);
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }

        //
        // ВАЖНЫЙ МОМЕНТ. из репозитория возвращается список классификаторов данного объекта
        // но сервис в этой функции игнорирует это и возвращает тот же самый объект
        // скорее всего в этом объекте нет нового классификатора(а может всех)
        public async Task<GeoObjectsClassifiersDTO> AddGeoObjectsClassifiers(GeoObjectsClassifiersDTO geoObjectsClassifiersDTO)
        {
            try
            {
                var geoObjectClassifiers = new GeoObjectsClassifiers
                {
                    GeoObjectId = geoObjectsClassifiersDTO.GeoObjectId,
                    ClassifierId = geoObjectsClassifiersDTO.ClassifierId
                };

                await _geoObjectRepository.AddGeoObjectsClassifiers(geoObjectClassifiers);

                return geoObjectsClassifiersDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }

        public async Task<List<GeoObjectsClassifiers>> GetGeoObjectsClassifiers(Guid? geoObjectInfoId)
        {
            try
            {
                return await _geoObjectRepository.GetGeoObjectsClassifiers(geoObjectInfoId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }

        public async Task<GeoObjectDTO> AddGeoObjectAspect(Guid geoObjectId, Guid aspectId)
        {
            try
            {
                return await _geoObjectMapper.ObjectToDTO(
                    await _geoObjectRepository.AddGeoObjectAspect(geoObjectId, aspectId)
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }

        public async Task<List<AspectDTO>> GetGeoObjectAspects(Guid geoObjectId)
        {
            try
            {
                List<AspectDTO> aspectsDTO = new List<AspectDTO>();
                List<Aspect> aspects = await _geoObjectRepository.GetGeoObjectAspects(geoObjectId);
                foreach(var aspect in aspects)
                {
                    aspectsDTO.Add(await _aspectMapper.AspectToDTO(aspect));
                }
                return aspectsDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
