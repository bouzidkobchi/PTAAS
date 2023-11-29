using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Tags("pentration tests")]
    [Route("tests")]
    [ApiController]
    public partial class PentrationTestController : ControllerBase
    {
        private readonly PentrationTestRepository _pentrationTestRepository;

        public PentrationTestController(AppDbContext context)
        {
            _pentrationTestRepository = new PentrationTestRepository(context);
        }

        [HttpGet]
        public IActionResult GetAllPentrationTests()
        {
            var tests = _pentrationTestRepository.GetAll();
            return Ok(tests);
        }

        [HttpGet("{id}")]
        public IActionResult GetPentrationTest(string id)
        {
            var test = _pentrationTestRepository.Get(id);

            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }

        /// <summary>
        /// not implemented
        /// </summary>
        [HttpGet("{id}/findings")]
        public IActionResult GetPentrationTestFindings(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult CreatePentrationTest([FromBody] PentrationTest test)
        {
            if (test == null)
            {
                return BadRequest();
            }

            var createdId = _pentrationTestRepository.Create(test);

            return Created($"/tests/{createdId}", test);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePentrationTest(string id, [FromBody] PentrationTest test)
        {
            if (test == null || test.Id != id)
            {
                return BadRequest();
            }

            var existingTest = _pentrationTestRepository.Get(id);

            if (existingTest == null)
            {
                return NotFound();
            }

            _pentrationTestRepository.Update(test);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePentrationTest(string id)
        {
            var test = _pentrationTestRepository.Get(id);

            if (test == null)
            {
                return NotFound();
            }

            _pentrationTestRepository.Delete(test);

            return NoContent();
        }

        /// <summary>
        /// not implemented
        /// </summary>
        [HttpGet("on-hold")]
        public IActionResult GetAll_OnHold()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// not implemented
        /// </summary>
        [HttpGet("cancelled")]
        public IActionResult GetAll_Cancelled()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// not implemented
        /// </summary>
        [HttpGet("scheduled")]
        public IActionResult GetAll_Scheduled()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// not implemented
        /// </summary>
        [HttpGet("completed")]
        public IActionResult GetAll_Completed()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// not implemented
        /// </summary>
        [HttpGet("in-progress")]
        public IActionResult GetAll_InProgress()
        {
            throw new NotImplementedException();
        }

    }

}
