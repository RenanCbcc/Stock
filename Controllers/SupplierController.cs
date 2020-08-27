using System.Collections.Generic;
using System.Threading.Tasks;
using Estoque.Models.SupplierModels;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository repository;

        public SupplierController(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/Product
        [HttpGet]
        public IEnumerable<Supplier> Get()
        {
            return repository.Browse();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var supplier = await repository.Read(id);
            if (supplier == null)
            {
                return NotFound(id);
            }
            return Ok(supplier);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Post(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var s = new Supplier
                {
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email
                };

                await repository.Add(s);
                var url = Url.Action("Get", new { id = s.Id });
                return Created(url, s);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/Product/
        [HttpPut]
        public async Task<IActionResult> Put(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var s = await repository.Read(model.Id);
                if (s == null)
                {
                    return NotFound(model.Id);
                }

                s.Name = model.Name;
                s.Email = model.Email;
                s.PhoneNumber = model.PhoneNumber;
                await repository.Edit(s);
                return Ok(s);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}