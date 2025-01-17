﻿using GISServer.API.Model;
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

        public ClassifierDTO Init(ClassifierDTO classifierDTO)
        {
            classifierDTO.Id = Guid.NewGuid();
            return classifierDTO;
        }

        public async Task<ClassifierDTO> Add(ClassifierDTO classifierDTO)
        {
            try
            {
                classifierDTO = Init(classifierDTO);
                Classifier classifier = await _classifierMapper.DTOToClassifier(classifierDTO);
                return await _classifierMapper.ClassifierToDTO(await _repository.Add(classifier));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex. Message}");
                return null;
            }
        }
        
        public async Task<ClassifierDTO> Get(Guid id)
        {
            try
            {
                ClassifierDTO classifierDTO = await _classifierMapper.ClassifierToDTO(await _repository.Get(id));
                return classifierDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<ClassifierDTO>> Get()
        {
            try
            {
                List<Classifier> classifiersFromDB = new List<Classifier>(await _repository.Get());
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
        public async Task<(bool, string)> Delete(Guid id)
        {
            try
            {
                return await _repository.Delete(id);
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
