﻿using Microsoft.AspNetCore.Mvc;
using GISServer.API.Interface;
using GISServer.API.Model;

namespace GISServer.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ParentChildController : ControllerBase
    {

        private readonly IParentChildService _parentChildService;

        public ParentChildController(IGeoObjectService service, IParentChildService parentChildService)
        {
            _parentChildService = parentChildService;
        }

        [HttpGet("Link")]
        public async Task<ActionResult> Get()
        {
            var dbParentChildLinks = await _parentChildService.Get();
            if (dbParentChildLinks == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No ParentChildLinks in database.");
            }

            return StatusCode(StatusCodes.Status200OK, dbParentChildLinks);
        }

        [HttpPost("Link")]
        public async Task<ActionResult> PostParentChildLinks(ParentChildObjectLinkDTO parentChildLinkDTO)
        {
            Guid guid = Guid.NewGuid();
            parentChildLinkDTO.Id = guid;

            var dbParentChildLinkDTO = await _parentChildService.Add(parentChildLinkDTO);

            if (dbParentChildLinkDTO == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "The relationship could not be added.");
            }
            return StatusCode(StatusCodes.Status200OK, dbParentChildLinkDTO);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParentChildLink(Guid id)
        {
            (bool status, string message) = await _parentChildService.DeleteParentChildLink(id);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            return NoContent();
        }
    }
}
