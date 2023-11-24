using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("findings")]
    public class FindingController : BaseController<Finding>
    {
        public FindingController(AppDbContext context) : base(new BaseRepository<Finding>(context)) { }
    }
}
