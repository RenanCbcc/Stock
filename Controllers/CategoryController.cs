using System.Collections.Generic;
using System.Threading.Tasks;
using Estoque.Models;
using Estoque.Models.CategoryModels;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository repository;

        public CategoryController(ICategoryRepository repository)
        {
            this.repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "page")] int page, [FromQuery(Name = "per_page")] int per_page)
        {
            var paginatedList = await PaginatedList<Category>.CreateAsync(repository.Browse(), page, per_page);
            return Ok(new { Data = paginatedList, Page = paginatedList.PageIndex, Total = paginatedList.Total });
        }

        [HttpGet]
        [Route("All")]
        public IEnumerable<Category> All()
        {
            return repository.Browse();
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await repository.Read(id);
            if (category == null)
            {
                return NotFound(id);
            }
            return Ok(category);
        }

        
        [HttpPost]
        public async Task<IActionResult> Post(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var p = new Category
                {
                    Title = model.Title,
                    Discount = model.Discount
                };

                await repository.Add(p);
                var url = Url.Action("Get", new { id = p.Id });
                return Created(url, p);
            }

            return BadRequest(ModelState);
        }

        
        [HttpPut]
        public async Task<IActionResult> Put(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var c = await repository.Read(model.Id);
                if (c == null)
                {
                    return NotFound(model.Id);
                }

                c.Title = model.Title;
                c.Discount = model.Discount;
                await repository.Edit(c);
                return Ok(c);
            }
            return BadRequest(ModelState);
        }

       
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}