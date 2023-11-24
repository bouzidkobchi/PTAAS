using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Controllers
{
    [Route("tests")]
    public class PentrationTestController : BaseController<PentrationTest>
    {
        public PentrationTestController(AppDbContext context) : base(new BaseRepository<PentrationTest>(context)){}
    }
}
