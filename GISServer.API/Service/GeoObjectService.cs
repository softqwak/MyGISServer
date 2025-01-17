﻿using GISServer.API.Model;
using GISServer.API.Mapper;
using GISServer.API.Interface;
using GISServer.API.Service.Model;
using GISServer.Domain.Model;

namespace GISServer.API.Service
{
    public class GeoObjectService : IGeoObjectService
    {
        private readonly IGeoObjectRepository _geoObjectRepository;
        private readonly IClassifierRepository _classifierRepository;
        private readonly IAspectRepository _aspectRepository;
        private readonly IGeoObjectsClassifiersRepository _gocRepository;
        private readonly IParentChildRepository _parentChildRepository;
        private readonly ITopologyRepository _topologyRepository;

        private readonly GeoObjectMapper _geoObjectMapper;
        private readonly AspectMapper _aspectMapper;
        private readonly ClassifierMapper _classifierMapper;

        public GeoObjectService(
                IGeoObjectRepository geoObjectRepository, 
                IClassifierRepository classifierRepository,
                IAspectRepository aspectRepository,
                IGeoObjectsClassifiersRepository gocRepository,
                IParentChildRepository parentChildRepository,
                ITopologyRepository topologyRepository,
                GeoObjectMapper geoObjectMapper, 
                ClassifierMapper classifierMapper,
                AspectMapper aspectMapper)
        {
            _geoObjectRepository = geoObjectRepository;
            _aspectRepository = aspectRepository;
            _classifierRepository = classifierRepository;
            _gocRepository = gocRepository;
            _parentChildRepository = parentChildRepository;
            _topologyRepository = topologyRepository;

            _geoObjectMapper = geoObjectMapper;
            _classifierMapper = classifierMapper;
            _aspectMapper = aspectMapper;
        }

        public GeoObjectDTO Init(GeoObjectDTO geoObjectDTO)
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

        public async Task<GeoObjectDTO> Add(GeoObjectDTO geoObjectDTO)
        {
            try
            {
                geoObjectDTO = Init(geoObjectDTO);
                GeoObject geoObject = await _geoObjectMapper.DTOToObject(geoObjectDTO);
                return await _geoObjectMapper.ObjectToDTO(await _geoObjectRepository.Add(geoObject));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<GeoObjectDTO>> Get()
        {
            try
            {
                List<GeoObject> geoObjectsFromDB = new List<GeoObject>(await _geoObjectRepository.Get());
                List<GeoObjectDTO> geoObjects = new List<GeoObjectDTO>();
                foreach (var geoObject in geoObjectsFromDB)
                {
                    List<GeoObjectsClassifiers> geoObjectsClassifiersFromDB = new List<GeoObjectsClassifiers>(
                            await _geoObjectRepository.GetClassifiers(geoObject.Id));

                    foreach (var goc in geoObjectsClassifiersFromDB)
                    {
                        Console.WriteLine(goc.ClassifierId);
                        geoObject.GeoObjectInfo.Classifiers.Add(
                                await _classifierRepository.Get(goc.ClassifierId));

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
        public async Task<GeoObjectDTO> Get(Guid id)
        {
            try
            {
                GeoObjectDTO geoObject = await _geoObjectMapper.ObjectToDTO(await _geoObjectRepository.Get(id));
                
                List<GeoObjectsClassifiers> geoObjectsClassifiersFromDB = new List<GeoObjectsClassifiers>(
                        await _geoObjectRepository.GetClassifiers(id));

                foreach (var gogc in geoObjectsClassifiersFromDB)
                {
                   geoObject.GeoObjectInfo.Classifiers.Add(
                       await _classifierMapper.ClassifierToDTO(
                           await _classifierRepository.Get(gogc.ClassifierId)));

                }

                return geoObject;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<GeoObjectDTO> Update(GeoObjectDTO geoObjectDTO)
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
        public async Task<(bool, string)> Delete(Guid id)
        {
            try
            {
                return await _geoObjectRepository.Delete(id);
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }

        public async Task<(bool, string)> Archive(Guid id)
        {
            List<(bool, string)> replys = new List<(bool, string)>();
            try
            {
                // replys.Add(await _classifierRepository.Archive(id));
                // replys.Add(await _geoObjectRepository.Archive(id));
                replys.Add(await _geoObjectRepository.Archive(id));
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
            return (true, $"GeoObject Archived");

        }

        //
        // ВАЖНЫЙ МОМЕНТ. из репозитория возвращается список классификаторов данного объекта
        // но сервис в этой функции игнорирует это и возвращает тот же самый объект
        // скорее всего в этом объекте нет нового классификатора(а может всех)
        public async Task<GeoObjectsClassifiersDTO> AddClassifier(GeoObjectsClassifiersDTO geoObjectsClassifiersDTO)
        {
            try
            {
                var geoObjectClassifiers = new GeoObjectsClassifiers
                {
                    GeoObjectId = geoObjectsClassifiersDTO.GeoObjectId,
                    ClassifierId = geoObjectsClassifiersDTO.ClassifierId
                };

                await _geoObjectRepository.AddClassifier(geoObjectClassifiers);

                return geoObjectsClassifiersDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }

        public async Task AddClassifier(Guid geoObjectId, Guid classifierId)
        {
            var geoObjectsClassifiers = new GeoObjectsClassifiers() 
            {
                GeoObjectId = geoObjectId,
                ClassifierId = classifierId
            };
            await _geoObjectRepository.AddClassifier(geoObjectsClassifiers);
        }

        public async Task<List<GeoObjectsClassifiers>> GetClassifiers(Guid? geoObjectInfoId)
        {
            try
            {
                return await _geoObjectRepository.GetClassifiers(geoObjectInfoId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }

        public async Task<GeoObjectDTO> AddAspect(Guid geoObjectId, Guid aspectId)
        {
            try
            {
                return await _geoObjectMapper.ObjectToDTO(
                    await _geoObjectRepository.AddAspect(geoObjectId, aspectId)
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }

        public async Task<List<AspectDTO>> GetAspects(Guid geoObjectId)
        {
            try
            {
                List<AspectDTO> aspectsDTO = new List<AspectDTO>();
                List<Aspect> aspects = await _geoObjectRepository.GetAspects(geoObjectId);
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
