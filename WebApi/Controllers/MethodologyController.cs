using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("Methodologies")]
    public class MethodologyController : BaseController<PentestingMethodology>
    {
        public MethodologyController(AppDbContext context) : base(new BaseRepository<PentestingMethodology>(context)) { }
        public override IActionResult Create([FromBody] PentestingMethodologyDTO entity)
        {
            return base.Create(PentestingMethodologyDTO(entity));
        }
    }
}
