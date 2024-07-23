using Microsoft.AspNetCore.Mvc;
using GISServer.API.Interface;
using GISServer.API.Model;

namespace GISServer.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GOCController : ControllerBase
    {

        private readonly IGeoObjectClassifiersService _geoObjectClassifiersService;

        public GOCController(IGeoObjectClassifiersService geoObjectClassifiersService)
        {
            _geoObjectClassifiersService = geoObjectClassifiersService;
        }

        [HttpGet("GetObjectsClassifiers")]
        public async Task<ActionResult> GetClassifiers()
        {
            var GetClassifiers = await _geoObjectClassifiersService.Get();
            if (GetClassifiers == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Classifier in database");
            }

            return StatusCode(StatusCodes.Status200OK, GetClassifiers);
        }

    }
}
