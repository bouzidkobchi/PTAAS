using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/systems")]
    [ApiController]
    public class TargetSystemController : ControllerBase
    {
        private readonly TargetSystemRepository targetSystemRepository;
        public TargetSystemController(AppDbContext context)
        {
            targetSystemRepository = new TargetSystemRepository(context);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(targetSystemRepository.GetAll());
        }

        [HttpPost]
        public IActionResult CreateSystem(TargetSystemDTO system)
        {
            if(system == null)
            {
                return BadRequest();
            }

            var systemId = targetSystemRepository.Create(system.ToTargetSystem());

            return Created($"/systems/{systemId}", system);
        }

    }
}
