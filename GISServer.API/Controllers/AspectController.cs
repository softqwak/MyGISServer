using Microsoft.AspNetCore.Mvc;
using GISServer.API.Service.Interface;
using GISServer.API.Model;

namespace GISServer.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AspectController : ControllerBase
    {

        private readonly IAspectService _aspectService;

        public AspectController(IGeoObjectService service, IAspectService aspectService)
        {
            _aspectService = aspectService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAspect()
        {
            var dbAspects = await _aspectService.GetAspects();
            if (dbAspects == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No Aspects in database.");
            }
            return StatusCode(StatusCodes.Status200OK, dbAspects);
        }

        [HttpPost]
        public async Task<ActionResult> PostAspect(AspectDTO aspectDTO)
        {
            aspectDTO.Id = Guid.NewGuid();
            var dbAspect = await _aspectService.AddAspect(aspectDTO);

            if (dbAspect == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "The Aspect could not be added.");
            }

            return CreatedAtAction("GetAspect", new { aspectDTO });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAspect(Guid id)
        {
            var dbAspect = await _aspectService.GetAspect(id);
            if (dbAspect == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No Aspect in database.");
            }
            return StatusCode(StatusCodes.Status200OK, dbAspect);
        }

        [HttpGet("CallAspect")]
        public async Task<ActionResult> CallAspect(String endPoint)
        {
            // something
            //
            String reportAspect = _aspectService.CallAspect(endPoint);
            return StatusCode(StatusCodes.Status200OK, reportAspect);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAspect(Guid id)
        {
            (bool status, string message) = await _aspectService.DeleteAspect(id);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            return NoContent();
        }
    }
}
