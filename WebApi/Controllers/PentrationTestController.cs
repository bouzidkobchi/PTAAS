using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WebApi.Data;
using WebApi.Enums;
using WebApi.Models;
using WebApi.Repositories;

/*
 * don't forget to add pagination to findings and methodology controller !!
 */

namespace WebApi.Controllers
{
    [Tags("pentration tests")]
    [Route("tests")]
    [ApiController]
    public partial class PentrationTestController : ControllerBase
    {
        private readonly PentrationTestRepository pentrationTestRepository;
        private readonly FindingRepository findingRepository;

        public PentrationTestController(AppDbContext context)
        {
            pentrationTestRepository = new PentrationTestRepository(context);
            findingRepository = new FindingRepository(context);
        }

        [HttpGet]
        public IActionResult GetAllPentrationTests()
        {
            var tests = pentrationTestRepository.GetAll();
            return Ok(tests);
        }

        [HttpGet("{id}")]
        public IActionResult GetPentrationTest(string id)
        {
            var test = pentrationTestRepository.Get(id);

            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }

        [HttpGet("{id}/findings")]
        public IActionResult GetFindings(string testId)
        {
            return Ok(pentrationTestRepository.Findings(testId));
        }

        [HttpPost("{id}/findings")]
        public IActionResult PostFindings(string testId ,[FromBody] FindingDTO finding)
        {
            if(finding.TestId != testId)
            {
                return BadRequest("The TestId in the request path does not match the TestId in the payload.");
            }

            var newFinding = finding.ToFinding();
            findingRepository.Create(newFinding);

            return Created($"/{testId}/findings/{newFinding.Id}", finding);
        }


        [HttpPost]
        public IActionResult CreatePentrationTest([FromBody] PentrationTest test)
        {
            if (test == null)
            {
                return BadRequest();
            }

            var createdId = pentrationTestRepository.Create(test);

            return Created($"/tests/{createdId}", test);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePentrationTest(string id, [FromBody] PentrationTest test)
        {
            if (test == null || test.Id != id)
            {
                return BadRequest();
            }

            var existingTest = pentrationTestRepository.Get(id);

            if (existingTest == null)
            {
                return NotFound();
            }

            pentrationTestRepository.Update(test);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePentrationTest(string id)
        {
            var test = pentrationTestRepository.Get(id);

            if (test == null)
            {
                return NotFound();
            }

            pentrationTestRepository.Delete(test);

            return NoContent();
        }

        [HttpGet("on-hold")]
        public IActionResult GetAll_OnHold()
        {
            return Ok(pentrationTestRepository.SelectStatus(TestStatus.OnHold));
        }

        [HttpGet("cancelled")]
        public IActionResult GetAll_Cancelled()
        {
            return Ok(pentrationTestRepository.SelectStatus(TestStatus.Cancelled));
        }

        [HttpGet("scheduled")]
        public IActionResult GetAll_Scheduled()
        {
            return Ok(pentrationTestRepository.SelectStatus(TestStatus.Scheduled));
        }

        [HttpGet("completed")]
        public IActionResult GetAll_Completed()
        {
            return Ok(pentrationTestRepository.SelectStatus(TestStatus.Completed));
        }

        [HttpGet("in-progress")]
        public IActionResult GetAll_InProgress()
        {
            return Ok(pentrationTestRepository.SelectStatus(TestStatus.InProgress));
        }

        /// <summary>
        /// not tested
        /// </summary>
        [HttpPut("change-status")]
        public IActionResult ChangeStatus(string id , TestStatus newStatus)
        {
            var selectedTest = pentrationTestRepository.Get(id);
            if (selectedTest != null)
            {
                selectedTest.Status = newStatus;
            }
            return Ok(selectedTest);
        }

    }

}
