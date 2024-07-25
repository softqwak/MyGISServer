using Microsoft.AspNetCore.Mvc;
using GISServer.API.Interface;
using GISServer.API.Model;

namespace GISServer.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ClassifierController : ControllerBase 
    {

        private readonly IClassifierService _classifierService;

        public ClassifierController(IClassifierService classifierService)
        {
            _classifierService = classifierService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var getClassifiers = await _classifierService.Get();
            if (getClassifiers == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Classifiers in database");
            }

            return StatusCode(StatusCodes.Status200OK, getClassifiers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var classifier = await _classifierService.Get(id);

            if (classifier == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No GeoObject found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, classifier);
        }

        [HttpPost]
        public async Task<ActionResult<ClassifierDTO>> PostClassifier(ClassifierDTO classifierDTO)
        {
            try{
            var dbClassifier = await _classifierService.Add(classifierDTO);
            if (dbClassifier == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{classifierDTO.Name} could not be added.");
            }
            return CreatedAtAction("Get", new {id = classifierDTO.Id}, new {classifierDTO});
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"{classifierDTO.Name} could not be added.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            (bool status, string message) = await _classifierService.Delete(id);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
            return NoContent();
        }
    }
}
