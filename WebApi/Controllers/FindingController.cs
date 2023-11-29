using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    //[Route("tests/findings")]
    //public class FindingController : ControllerBase
    //{
    //    private readonly BaseRepository<Finding> _findingRepository;

    //    public FindingController(AppDbContext context)
    //    {
    //        _findingRepository = new BaseRepository<Finding>(context);
    //    }

    //    [HttpGet]
    //    public IActionResult GetAllFindings()
    //    {
    //        var findings = _findingRepository.GetAll();
    //        return Ok(findings);
    //    }

    //    [HttpGet("{id}")]
    //    public IActionResult GetFinding(string id)
    //    {
    //        var finding = _findingRepository.Get(id);

    //        if (finding == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(finding);
    //    }

    //    [HttpPost]
    //    public IActionResult CreateFinding([FromBody] FindingDTO finding)
    //    {
    //        if (finding == null)
    //        {
    //            return BadRequest();
    //        }

    //        var createdId = _findingRepository.Create(finding.ToFinding());

    //        return Created($"/findings/{createdId}", finding);
    //    }

    //    [HttpPut("{id}")]
    //    public IActionResult UpdateFinding(string id, [FromBody] FindingDTO finding)
    //    {
    //        if (finding == null || finding.Id != id)
    //        {
    //            return BadRequest();
    //        }

    //        var existingFinding = _findingRepository.Get(id);

    //        if (existingFinding == null)
    //        {
    //            return NotFound();
    //        }

    //        _findingRepository.Update(finding.ToFinding());

    //        return NoContent();
    //    }

    //    [HttpDelete("{id}")]
    //    public IActionResult DeleteFinding(string id)
    //    {
    //        var finding = _findingRepository.Get(id);

    //        if (finding == null)
    //        {
    //            return NotFound();
    //        }

    //        _findingRepository.Delete(finding);

    //        return NoContent();
    //    }
    //}

}
