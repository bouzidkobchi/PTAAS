using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Controllers
{
    [Route("tests")]
    public class PentrationTestController : ControllerBase
    {
        private readonly BaseRepository<PentrationTest> _pentrationTestRepository;

        public PentrationTestController(AppDbContext context)
        {
            _pentrationTestRepository = new BaseRepository<PentrationTest>(context);
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
    }

}
