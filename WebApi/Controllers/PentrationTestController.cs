using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Enums;
using WebApi.Models;
using WebApi.Repositories;

/*
 * don't forget to add pagination to findings and methodology controller !!
 * remember to add the indexing to the test status attribute
 * add authentication & autorization
 * add rate limiting
 * add model validation (almost done)
 * documentation
 */

namespace WebApi.Controllers
{
    [Tags("pentration tests")]
    [Route("api/tests")]
    [ApiController]
    public partial class PentrationTestController : ControllerBase
    {
        private readonly PentrationTestRepository _pentrationTestRepository;
        private readonly FindingRepository _findingRepository;

        public PentrationTestController(PentrationTestRepository pentrationTestRepository , FindingRepository findingRepository)
        {
            _pentrationTestRepository = pentrationTestRepository;
            _findingRepository = findingRepository;
        }

        /// <summary>
        /// Gets all penetration tests.
        /// </summary>
        /// <returns>A list of all penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var tests = _pentrationTestRepository.GetAll();
            return Ok(tests);
        }


        /// <summary>
        /// Gets a specific penetration test.
        /// </summary>
        /// <param name="id">The ID of the penetration test.</param>
        /// <returns>The penetration test if found; otherwise, NotFound.</returns>
        /// <response code="200">Returns the penetration test</response>
        /// <response code="404">If the penetration test is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(string id)
        {
            try
            {
                var test = _pentrationTestRepository.Get(id);

                if (test == null)
                {
                    return NotFound();
                }

                return Ok(test);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// Creates a new penetration test.
        /// </summary>
        /// <param name="test">The penetration test to be created.</param>
        /// <returns>A newly created penetration test.</returns>
        /// <response code="201">Returns the newly created penetration test</response>
        /// <response code="400">If the penetration test is null</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] PentrationTestDTO test)
        {
            try
            {
                if (test == null)
                {
                    return BadRequest();
                }

                var newTest = test.ToPentrationTest();
                var createdId = _pentrationTestRepository.Create(newTest);

                return Created($"/tests/{createdId}", test);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// Updates a specific penetration test.
        /// </summary>
        /// <param name="id">The ID of the penetration test.</param>
        /// <param name="test">The updated penetration test.</param>
        /// <returns>No content if the update is successful; otherwise, BadRequest or NotFound.</returns>
        /// <response code="204">If the update is successful</response>
        /// <response code="400">If the penetration test is null or the TestId in the request path does not match the TestId in the payload</response>
        /// <response code="404">If the penetration test is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePentrationTest(string id, [FromBody] PentrationTestDTO test)
        {
            try
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

                _pentrationTestRepository.Update(test.ToPentrationTest());

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// Deletes a specific penetration test.
        /// </summary>
        /// <param name="id">The ID of the penetration test.</param>
        /// <returns>No content if the deletion is successful; otherwise, NotFound.</returns>
        /// <response code="204">If the deletion is successful</response>
        /// <response code="404">If the penetration test is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePentrationTest(string id)
        {
            try
            {
                var test = _pentrationTestRepository.Get(id);

                if (test == null)
                {
                    return NotFound();
                }

                _pentrationTestRepository.Delete(test);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }


        /// <summary>
        /// Gets all findings for a specific penetration test.
        /// </summary>
        /// <param name="id">The ID of the penetration test.</param>
        /// <returns>The findings of the penetration test if found; otherwise, NotFound.</returns>
        /// <response code="200">Returns the findings of the penetration test</response>
        /// <response code="404">If the penetration test is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("{id}/findings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetFindings(string id)
        {
            try
            {
                var findings = _pentrationTestRepository.Findings(id);

                if (findings == null)
                {
                    return NotFound();
                }

                return Ok(findings);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// Creates a new finding for a specific penetration test.
        /// </summary>
        /// <param name="id">The ID of the penetration test.</param>
        /// <param name="finding">The finding to be created.</param>
        /// <returns>A newly created finding for the penetration test.</returns>
        /// <response code="201">Returns the newly created finding</response>
        /// <response code="400">If the TestId in the request path does not match the TestId in the payload</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPost("{id}/findings")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostFindings(string id, [FromBody] FindingDTO finding)
        {
            try
            {
                if (finding.TestId != id)
                {
                    return BadRequest("The TestId in the request path does not match the TestId in the payload.");
                }

                var newFinding = finding.ToFinding();
                _findingRepository.Create(newFinding);

                return Created($"/{id}/findings/{newFinding.Id}", finding);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// Gets all on-hold penetration tests with pagination.
        /// </summary>
        /// <param name="page">The page number (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>A list of all on-hold penetration tests for the specified page.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("on-hold")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get_OnHold(int page = 1, int pageSize = 10)
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.OnHold, page, pageSize);
                return Ok(tests);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        /// <summary>
        /// Gets all cancelled penetration tests.
        /// </summary>
        /// <returns>A list of all cancelled penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("cancelled")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get_Cancelled()
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.Cancelled);
                return Ok(tests);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }


        /// <summary>
        /// Gets all scheduled penetration tests.
        /// </summary>
        /// <returns>A list of all scheduled penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("scheduled")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll_Scheduled()
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.Scheduled);
                return Ok(tests);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }


        /// <summary>
        /// Gets all completed penetration tests.
        /// </summary>
        /// <returns>A list of all completed penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("completed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll_Completed()
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.Completed);
                return Ok(tests);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }


        /// <summary>
        /// Gets all in-progress penetration tests.
        /// </summary>
        /// <returns>A list of all in-progress penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("in-progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll_InProgress()
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.InProgress);
                return Ok(tests);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }


        /// <summary>
        /// Changes the status of a specific penetration test.
        /// </summary>
        /// <param name="id">The ID of the penetration test.</param>
        /// <param name="newStatus">The new status for the penetration test.</param>
        /// <returns>The updated penetration test if found; otherwise, NotFound.</returns>
        /// <response code="200">Returns the updated penetration test</response>
        /// <response code="404">If the penetration test is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPut("change-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ChangeStatus(string id, TestStatus newStatus)
        {
            try
            {
                var selectedTest = _pentrationTestRepository.Get(id);
                if (selectedTest == null)
                {
                    return NotFound();
                }

                selectedTest.Status = newStatus;
                _pentrationTestRepository.Update(selectedTest);

                return Ok(selectedTest);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

    }

}
