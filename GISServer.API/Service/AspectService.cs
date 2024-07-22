using GISServer.API.Model;
using GISServer.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using GISServer.API.Mapper;

namespace GISServer.API.Service
{
    public class AspectService : IAspectService
    {
        private readonly IAspectRepository _repository;
        private readonly AspectMapper _aspectMapper;
        
        public AspectService(IAspectRepository repository, AspectMapper aspectMapper)
        {
            _repository = repository;
            _aspectMapper = aspectMapper;
        }
        
        public AspectDTO InitAspect(AspectDTO aspectDTO)
        {
            aspectDTO.Id = Guid.NewGuid();
            return aspectDTO;
        }

        public async Task<AspectDTO> AddAspect(AspectDTO aspectDTO)
        {
            try 
            {
                aspectDTO = InitAspect(aspectDTO);                
                Aspect aspect = await _aspectMapper.DTOToAspect(aspectDTO);
                return await _aspectMapper.AspectToDTO(await _repository.AddAspect(aspect));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }

        public async Task<AspectDTO> GetAspect(Guid id)
        {
            Aspect aspect = await _repository.GetAspect(id);
            return await _aspectMapper.AspectToDTO(aspect);
        }

        public async Task<List<AspectDTO>> GetAspects()
        {
            List<AspectDTO> aspectsDTO = new List<AspectDTO>();
            List<Aspect> aspects = await _repository.GetAspects();
            foreach(var aspect in aspects)
            {
                aspectsDTO.Add(await _aspectMapper.AspectToDTO(aspect));
            }
            return aspectsDTO;
        }

        public String CallAspect(String endPoint)
        {
            String report = "some information";
            return report;
        }
        public async Task<(bool, string)> DeleteAspect(Guid id)
        {
            try
            {
                return await _repository.DeleteAspect(id);
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
