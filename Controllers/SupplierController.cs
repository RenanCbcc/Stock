using System.Collections.Generic;
using System.Threading.Tasks;
using Stock_Back_End.Models;
using Stock_Back_End.Models.SupplierModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Stock_Back_End.Controllers
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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "page")] int page=1, [FromQuery(Name = "per_page")] int per_page=10)
        {
            var paginatedList = await PaginatedList<Supplier>.CreateAsync(repository.Browse(), page, per_page);
            return Ok(new { Data = paginatedList, Page = paginatedList.PageIndex, Total = paginatedList.Total });
        }

        [HttpGet]
        [Route("All")]
        public IEnumerable<Supplier> Get()
        {
            return repository.Browse();
        }

        
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

       
        [HttpPost]
        [Authorize]
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

       
        [HttpPut]
        [Authorize]
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

        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}