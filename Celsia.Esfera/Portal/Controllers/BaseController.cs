using System.Collections.Generic;
using System.Threading.Tasks;
using Bussines;
using Entities.Data;
using Microsoft.AspNetCore.Mvc;

namespace Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity, TRepository> : ControllerBase where TEntity : class, IEntity where TRepository : IRepository<TEntity>
    {
        private readonly TRepository repository;

        protected BaseController(TRepository repository)
        {
            this.repository = repository;
        }


        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetAsync()
        {
            return await this.repository.GetAsync();
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetAsync(int id)
        {
            TEntity enity = await this.repository.GetAsync(id);

            return enity == null ? this.NotFound() : (ActionResult<TEntity>)enity;
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TEntity enity)
        {
            if (id != enity.Id)
            {
                return this.BadRequest();
            }

            await this.repository.UpdateAsync(enity);
            return this.NoContent();
        }

        // POST: api/[controller]
        [HttpPost]
        public async Task<ActionResult<TEntity>> Post(TEntity enity)
        {
            await this.repository.AddAsync(enity);
            return this.CreatedAtAction("Get", new { id = enity.Id }, enity);
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TEntity>> Delete(int id)
        {
            TEntity enity = await this.repository.DeleteAsync(id);
            return enity == null ? this.NotFound() : (ActionResult<TEntity>)enity;
        }

    }
}