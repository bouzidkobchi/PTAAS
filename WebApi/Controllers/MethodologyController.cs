using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Tags("pentration methodologies")]
    [Route("api/methodologies")]
    public class PentestingMethodologyController : ControllerBase
    {
        private readonly PentestingMethodologyRepository _PentestingMethodologyRepository;

        public PentestingMethodologyController(PentestingMethodologyRepository pentestingMethodologyRepository)
        {
            _PentestingMethodologyRepository  = pentestingMethodologyRepository;
        }

        /// <summary>
        /// Retrieves a page of pentesting methodologies.
        /// </summary>
        /// <param name="pageNumber">The number of the page to retrieve. Page numbers start at 1.</param>
        /// <param name="pageSize">The number of methodologies to include on each page.</param>
        /// <returns>An IActionResult containing a list of methodologies.</returns>
        /// <response code="200">Returns the list of methodologies for the specified page.</response>
        /// <response code="400">Returns if the pageNumber or pageSize is less than 1.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetPage(int pageNumber = 1, int pageSize = 10)
        {
            var methodologies = _PentestingMethodologyRepository.GetPage(pageNumber, pageSize);
            return Ok(methodologies);
        }


        /// <summary>
        /// Retrieves a pentesting methodology by ID.
        /// </summary>
        /// <param name="id">The ID of the pentesting methodology to retrieve.</param>
        /// <returns>An IActionResult containing the pentesting methodology if found, NotFound otherwise.</returns>
        /// <response code="200">Returns the pentesting methodology for the specified ID.</response>
        /// <response code="404">Returns if the pentesting methodology is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string id)
        {
            var PentestingMethodology = _PentestingMethodologyRepository.Get(id);

            if (PentestingMethodology == null)
            {
                return NotFound();
            }

            return Ok(PentestingMethodology);
        }


        /// <summary>
        /// Creates a new pentesting methodology.
        /// </summary>
        /// <param name="pentestingMethodology">The data of the pentesting methodology to create.</param>
        /// <returns>An IActionResult that represents the result of the creation operation.</returns>
        /// <response code="201">Returns when the pentesting methodology is created successfully.</response>
        /// <response code="400">Returns if the pentestingMethodology parameter is null.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] PentestingMethodologyDTO pentestingMethodology)
        {
            if (pentestingMethodology == null)
            {
                return BadRequest();
            }
            var method = pentestingMethodology.ToPentestingMethodology();
            var createdId = _PentestingMethodologyRepository.Create(method);

            return Created($"/methodologies/{createdId}", method);
        }


        /// <summary>
        /// Deletes a pentesting methodology by ID.
        /// </summary>
        /// <param name="id">The ID of the pentesting methodology to delete.</param>
        /// <returns>An IActionResult that represents the result of the deletion operation.</returns>
        /// <response code="204">Returns when the pentesting methodology is deleted successfully.</response>
        /// <response code="404">Returns if the pentesting methodology is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string id)
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
        /// Retrieves the tests for a pentesting methodology.
        /// </summary>
        /// <param name="id">The ID of the pentesting methodology.</param>
        /// <param name="pageNumber">The number of the page to retrieve. Page numbers start at 1.</param>
        /// <param name="pageSize">The number of tests to include on each page.</param>
        /// <returns>An IActionResult containing the tests for the pentesting methodology if found, NotFound otherwise.</returns>
        /// <response code="200">Returns the tests for the specified pentesting methodology.</response>
        /// <response code="404">Returns if the pentesting methodology is not found.</response>
        [HttpGet("{id}/tests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTests(string id, int pageNumber , int pageSize)
        {
            var tests = _PentestingMethodologyRepository.GetPage_Tests(id,pageNumber,pageSize);

            if (tests == null)
            {
                return NotFound();
            }

            return Ok(tests);
        }


    }

}
