using Microsoft.AspNetCore.Mvc;
using GISServer.API.Interface;
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
        public async Task<ActionResult> Get()
        {
            var dbAspects = await _aspectService.Get();
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
            var dbAspect = await _aspectService.Add(aspectDTO);

            if (dbAspect == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "The Aspect could not be added.");
            }

            return CreatedAtAction("Get", new { aspectDTO });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var dbAspect = await _aspectService.Get(id);
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
            String reportAspect = await _aspectService.CallAspect(endPoint);
            if (reportAspect == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "dont can call aspect");
            }
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
