using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("methodologies")]
    public class PentestingMethodologyController : ControllerBase
    {
        private readonly BaseRepository<PentestingMethodology> _PentestingMethodologyRepository;

        public PentestingMethodologyController(AppDbContext context)
        {
            _PentestingMethodologyRepository = new PentestingMethodologyRepository(context);
        }

        [HttpGet]
        public IActionResult GetAllMethodologies()
        {
            var methodologies = _PentestingMethodologyRepository.GetAll();
            return Ok(methodologies);
        }

        [HttpGet("{id}")]
        public IActionResult GetPentestingMethodology(string id)
        {
            var PentestingMethodology = _PentestingMethodologyRepository.Get(id);

            if (PentestingMethodology == null)
            {
                return NotFound();
            }

            return Ok(PentestingMethodology);
        }

        [HttpPost]
        public IActionResult CreatePentestingMethodology([FromBody] PentestingMethodology PentestingMethodology)
        {
            if (PentestingMethodology == null)
            {
                return BadRequest();
            }

            var createdId = _PentestingMethodologyRepository.Create(PentestingMethodology);

            return Created($"/methodologies/{createdId}", PentestingMethodology);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePentestingMethodology(string id, [FromBody] PentestingMethodology PentestingMethodology)
        {
            if (PentestingMethodology == null || PentestingMethodology.Id != id)
            {
                return BadRequest();
            }

            var existingPentestingMethodology = _PentestingMethodologyRepository.Get(id);

            if (existingPentestingMethodology == null)
            {
                return NotFound();
            }

            _PentestingMethodologyRepository.Update(PentestingMethodology);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePentestingMethodology(string id)
        {
            var PentestingMethodology = _PentestingMethodologyRepository.Get(id);

            if (PentestingMethodology == null)
            {
                return NotFound();
            }

            _PentestingMethodologyRepository.Delete(PentestingMethodology);

            return NoContent();
        }
    }

}
