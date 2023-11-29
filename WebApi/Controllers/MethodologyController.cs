using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Tags("pentration methodologies")]
    [Route("methodologies")]
    public class PentestingMethodologyController : ControllerBase
    {
        private readonly PentestingMethodologyRepository _PentestingMethodologyRepository;

        public PentestingMethodologyController(AppDbContext context)
        {
            _PentestingMethodologyRepository = new PentestingMethodologyRepository(context);
        }

        [HttpGet]
        public IActionResult GetAllMethodologies()
        {
            var methodologies = _PentestingMethodologyRepository.GetAllAsDTOs();
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
        public IActionResult CreatePentestingMethodology([FromBody] PentestingMethodologyDTO pentestingMethodology)
        {
            if (pentestingMethodology == null)
            {
                return BadRequest();
            }
            var method = pentestingMethodology.ToPentestingMethodology();
            var createdId = _PentestingMethodologyRepository.Create(method);

            return Created($"/methodologies/{createdId}", method);
        }

        //[HttpPut("{id}")] // no need for it
        //public IActionResult UpdatePentestingMethodology(string id, [FromBody] PentestingMethodologyDTO pentestingMethodology)
        //{
        //    if (pentestingMethodology == null)
        //    {
        //        return BadRequest();
        //    }

        //    var existingPentestingMethodology = _PentestingMethodologyRepository.Get(id);

        //    if (existingPentestingMethodology == null)
        //    {
        //        return NotFound();
        //    }

        //    existingPentestingMethodology.Name = pentestingMethodology.Name;

        //    _PentestingMethodologyRepository.Update(existingPentestingMethodology);

        //    return NoContent();
        //}

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

        /// <summary>
        /// not implemented
        /// </summary>
        [HttpGet("{id}/tests")]
        public IActionResult GetPentestingMethodologyTests(string id)
        {
            throw new NotImplementedException();
        }

    }

}
