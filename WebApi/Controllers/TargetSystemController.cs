using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Tags("target systems (web app , mobile app ..)")]
    [Route("api/systems")]
    [ApiController]
    public class TargetSystemController : ControllerBase
    {
        private readonly TargetSystemRepository _targetSystemRepository;
        public TargetSystemController(AppDbContext context, TargetSystemRepository targetSystemRepository)
        {
            _targetSystemRepository = targetSystemRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_targetSystemRepository.GetAll());
        }

        [HttpPost]
        public IActionResult CreateSystem(TargetSystemDTO system)
        {
            if(system == null)
            {
                return BadRequest();
            }

            var systemId = _targetSystemRepository.Create(system.ToTargetSystem());

            return Created($"/systems/{systemId}", system);
        }

        [HttpGet("{id}/tests")]
        public IActionResult GetTest([Required] string id, int pageNumber , int pageSize)
        {
            var system = _targetSystemRepository.Get(id);

            if(system == null)
            {
                return NotFound(new
                {
                    Message = $"there is no system with ID = {id}"
                });
            }

            var tests = system.Tests.ToList();

            return Ok(tests);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            // don't delet tests
            throw new NotImplementedException();
        }

    }
}
