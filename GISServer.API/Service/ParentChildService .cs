using GISServer.API.Model;
using GISServer.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using GISServer.API.Mapper;


namespace GISServer.API.Service
{
    public class ParentChildService : IParentChildService
    {
        private readonly IParentChildRepository _repository;
        private readonly ParentChildMapper _parentChildMapper;

        public ParentChildService(IParentChildRepository repository, ParentChildMapper parentChildMapper)
        {
            _repository = repository;
            _parentChildMapper = parentChildMapper;
        }

        public ParentChildObjectLinkDTO CreateGuid(ParentChildObjectLinkDTO parentChildObjectLinkDTO)
        {
            parentChildObjectLinkDTO.Id = Guid.NewGuid();
            return parentChildObjectLinkDTO;
        }


        public async Task<ParentChildObjectLinkDTO> AddParentChildLink(ParentChildObjectLinkDTO parentChildObjectLinkDTO)
        {
            try
            {
                parentChildObjectLinkDTO = CreateGuid(parentChildObjectLinkDTO);
                ParentChildObjectLink parentChildObjectLink = await _parentChildMapper.DTOToParentChildObjectLink(parentChildObjectLinkDTO);
                return await _parentChildMapper.ParentChildObjectLinkToDTO(await _repository.AddParentChildLink(parentChildObjectLink));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }

            return null;
        }

        public async Task<List<ParentChildObjectLinkDTO>> GetParentChildLinks()
        {
            try
            {
                List<ParentChildObjectLinkDTO> parentChildObjectLinksDTO = new List<ParentChildObjectLinkDTO>();
                List<ParentChildObjectLink> parentChildObjectLinks = await _repository.GetParentChildLinks();
                foreach (var parentChildObjectLink in parentChildObjectLinks)
                {
                    parentChildObjectLinksDTO.Add(await _parentChildMapper.ParentChildObjectLinkToDTO(parentChildObjectLink));
                }
                return parentChildObjectLinksDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured. Error Message: {ex.Message}");
                return null;
            }
        }
        public async Task<(bool, string)> DeleteParentChildLink(Guid id)
        {
            try
            {
                return await _repository.DeleteParentChildLink(id);
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
