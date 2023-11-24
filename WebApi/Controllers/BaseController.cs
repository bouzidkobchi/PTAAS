using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    /*
     * To Do :
     * don't forget to implement DTOs for showing some properties from the objects like Methodology with contains test .. etc
     */
    public class BaseController<T,TDTO> : Controller where T : class, IHasId
    {
        protected readonly BaseRepository<T> _repository;

        public BaseController(BaseRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public virtual IActionResult GetAll()
        {
            var entities = _repository.GetAll();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public virtual IActionResult GetById(string id)
        {
            var entity = _repository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost]
        public virtual IActionResult Create([FromBody] T entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object response ;
            try
            {
                string id = _repository.Create(entity);
                response = new { id };
            }
            catch(Exception e)
            {
                return Json(new { e.Message, e.Data });
            }
            return Ok(response);
        }
        //public virtual IActionResult Create([FromBody] IDTO<T> entityDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    object response ;
        //    try
        //    {
        //        string id = _repository.Create(entityDTO.ToBase());
        //        response = new { id };
        //    }
        //    catch(Exception e)
        //    {
        //        return Json(new { e.Message, e.Data });
        //    }
        //    return Ok(response);
        //}

        

        [HttpPut("{id}")]
        public virtual IActionResult Update(string id, [FromBody] T entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEntity = _repository.Get(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            _repository.Update(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual IActionResult Delete(string id)
        {
            var entity = _repository.Get(id);
            if (entity == null)
            {
                return NotFound();
            }

            _repository.Delete(id);

            return Ok(entity);
        }

        [HttpGet("page")]
        public virtual IActionResult GetPage(int page, int pageSize)
        {
            var entities = _repository.GetPage(page, pageSize);
            return Ok(entities);
        }
    }
}
