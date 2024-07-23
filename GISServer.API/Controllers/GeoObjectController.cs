using Microsoft.AspNetCore.Mvc;
using GISServer.API.Interface;
using GISServer.API.Model;

namespace GISServer.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GeoObjectController : ControllerBase
    {
        private readonly IGeoObjectService _geoObjectService;
        private readonly IGeoObjectClassifiersService _geoObjectClassifierService;

        public GeoObjectController(
                IGeoObjectService service, 
                IGeoObjectClassifiersService geoObjectClassifierService)
        {
            _geoObjectService = service;
            _geoObjectClassifierService = geoObjectClassifierService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var geoObjects = await _geoObjectService.Get();
            if (geoObjects == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No GeoObjects in database");
            }

            return StatusCode(StatusCodes.Status200OK, geoObjects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var geoObject = await _geoObjectService.Get(id);

            if (geoObject == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No GeoObject found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, geoObject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeoObject(Guid id, GeoObjectDTO geoObjectDTO)
        {
            if (id != geoObjectDTO.Id)
            {
                return BadRequest();
            }

            var dbGeoObject = await _geoObjectService.Update(geoObjectDTO);

            if (dbGeoObject == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{geoObjectDTO.Name} could not be updated");
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<GeoObjectDTO>> PostGeoObject(GeoObjectDTO geoObjectDTO)
        {
            var dbGeoObject = await _geoObjectService.Add(geoObjectDTO);

            if (dbGeoObject == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{geoObjectDTO.Name} could not be added.");
            }

            return CreatedAtAction("Get", new { id = geoObjectDTO.Id }, geoObjectDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeoObject(Guid id)
        {
            (bool status, string message) = await _geoObjectService.Archive(id);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return NoContent();
        }


        [HttpPost("AddClassifier")]
        public async Task<ActionResult<GeoObjectsClassifiersDTO>> PostGeoObjectsClassifiers(Guid geoObjectId, Guid classifierId)
        {
            try
            {
                var geoObjectsClassifiersDTO = new GeoObjectsClassifiersDTO
                {
                    GeoObjectId = geoObjectId,
                    ClassifierId = classifierId
                };

                var dbGeoObjectsClassifiers = await _geoObjectService.AddClassifier(geoObjectsClassifiersDTO);
                if (dbGeoObjectsClassifiers == null)
                {
                    Console.WriteLine("Problem's here");
                    return StatusCode(StatusCodes.Status500InternalServerError, "The relationship could not be added.");
                }

                return StatusCode(StatusCodes.Status200OK, geoObjectsClassifiersDTO);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "The relationship could not be added.");
            }
        }

        [HttpDelete("DeleteClassifier")]
        public async Task<IActionResult> DeleteGeoObjectClassifier(Guid geoObjectId, Guid classifierId)
        {
            (bool status, string message) = await _geoObjectClassifierService.Delete(geoObjectId, classifierId);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return NoContent();
        }

      
        [HttpGet("GetAspects/{id}")]
        public async Task<ActionResult> GetAspects(Guid geoObjectId)
        {
            var dbAspects = await _geoObjectService.GetAspects(geoObjectId);
            if (dbAspects == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No Aspects of GeoObject in database.");
            }
            return StatusCode(StatusCodes.Status200OK, dbAspects);
            
        }

        [HttpPost("AddAspect")]
        public async Task<ActionResult> PostGeoObjectAspect(Guid geoObjectId, Guid aspectId)
        {
            var dbgeoObject = await _geoObjectService.AddAspect(geoObjectId, aspectId);
            if (dbgeoObject == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No Aspect or GeoObject in database.");
            }
            return StatusCode(StatusCodes.Status200OK, dbgeoObject);
        }
    }
}
