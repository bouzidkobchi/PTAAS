using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Enums;
using WebApi.Repositories;

/*
 * don't forget to add pagination to findings and methodology controller !!
 * remember to add the indexing to the test status attribute
 * add authentication & autorization
 * add rate limiting
 * add model validation (add the length and the model attributes validations)
 * documentation
 * make your foreignkeys indexed
 */

/*
 * assign test to pentester
 * search functionality
 * 
 */

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for handling penetration tests.
    /// </summary>
    [Tags("pentration tests")]
    [Route("api/tests")]
    [ApiController]
    public partial class PentrationTestController : ControllerBase
    {
        private readonly PentrationTestRepository _pentrationTestRepository;
        private readonly FindingRepository _findingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PentrationTestController"/> class.
        /// </summary>
        /// <param name="pentrationTestRepository">The repository for penetration tests.</param>
        /// <param name="findingRepository">The repository for findings.</param>
        public PentrationTestController(PentrationTestRepository pentrationTestRepository, FindingRepository findingRepository)
        {
            _pentrationTestRepository = pentrationTestRepository ?? throw new ArgumentNullException(nameof(pentrationTestRepository));
            _findingRepository = findingRepository ?? throw new ArgumentNullException(nameof(findingRepository));
        }


        /// <summary>
        /// Gets all penetration tests.
        /// </summary>
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>A list of all penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPage(int pageNumber, int pageSize)
        {
            var tests = _pentrationTestRepository.GetPage(pageNumber, pageSize);
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
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>The findings of the penetration test if found; otherwise, NotFound.</returns>
        /// <response code="200">Returns the findings of the penetration test</response>
        /// <response code="404">If the penetration test is not found</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("{id}/findings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetFindings(string id, int pageNumber , int pageSize)
        {
            try
            {
                var findings = _pentrationTestRepository.GetFindings(id,pageNumber , pageSize);

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

        //public IActionResult UpdateFindings(string id, FindingDTO finding)
        //{
        //    throw new NotImplementedException();
        //}

        //public IActionResult DeleteFindings(string id)
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// Updates a finding.
        /// </summary>
        /// <param name="id">The ID of the finding to update.</param>
        /// <param name="finding">The updated finding data.</param>
        /// <returns>An IActionResult that represents the update result.</returns>
        //[HttpPut("{id}")]
        //public IActionResult UpdateFindings(string id, FindingDTO finding)
        //{
        //    try
        //    {
        //        var findingToUpdate = _context.Findings.FirstOrDefault(f => f.Id == id);
        //        if (findingToUpdate == null)
        //        {
        //            return NotFound(new { Message = $"Finding with id {id} not found." });
        //        }

        //        // Map the updated data from the DTO to the existing finding
        //        // This depends on the structure of your Finding and FindingDTO classes
        //        findingToUpdate.SomeProperty = finding.SomeProperty;
        //        // Repeat for all properties

        //        _context.Findings.Update(findingToUpdate);
        //        _context.SaveChanges();

        //        return Ok(new { Message = $"Finding with id {id} updated successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        return StatusCode(500, new { Message = "An error occurred while updating the finding.", Error = ex.Message });
        //    }
        //}

        /// <summary>
        /// Deletes a finding.
        /// </summary>
        /// <remarks>
        /// This method deletes the finding with the specified ID. If the finding doesn't exist, it returns a 404 Not Found status code. If an error occurs while deleting the finding, it returns a 500 Internal Server Error status code.
        /// </remarks>
        /// <param name="id">The ID of the finding to delete.</param>
        /// <returns>An IActionResult that represents the delete result.</returns>
        /// <response code="200">Returns when the finding is deleted successfully.</response>
        /// <response code="404">Returns when the finding is not found.</response>
        /// <response code="500">Returns when an error occurs while deleting the finding.</response>
        [HttpDelete("findings/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFindings(string id)
        {
            try
            {
                var findingToDelete = _findingRepository.Get(id);
                if (findingToDelete == null)
                {
                    return NotFound(new { Message = $"Finding with id {id} not found." });
                }

                _findingRepository.Delete(findingToDelete);

                return Ok(new { Message = $"Finding with id {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the finding.", Error = ex.Message });
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
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>A list of all on-hold penetration tests for the specified page.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("on-hold")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get_OnHold(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.OnHold, pageNumber, pageSize);
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
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>A list of all cancelled penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("cancelled")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get_Cancelled(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.Cancelled, pageNumber, pageSize);
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
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>A list of all scheduled penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("scheduled")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll_Scheduled(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.Scheduled, pageNumber, pageSize);
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
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>A list of all completed penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("completed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll_Completed(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.Completed, pageNumber, pageSize);
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
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>A list of all in-progress penetration tests.</returns>
        /// <response code="200">Returns the list of tests</response>  
        /// <response code="500">If there is an internal server error</response>
        [HttpGet("in-progress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll_InProgress(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var tests = _pentrationTestRepository.SelectStatus(TestStatus.InProgress, pageNumber, pageSize);
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