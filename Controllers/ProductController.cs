using System.Collections.Generic;
using System.Threading.Tasks;
using Estoque.Models;
using Estoque.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository repository;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "page")] int page, [FromQuery(Name = "per_page")] int per_page)
        {
            var paginatedList = await PaginatedList<Product>.CreateAsync(repository.Browse(), page, per_page);
            return Ok(new { Data = paginatedList, Page = paginatedList.PageIndex, Total = paginatedList.Total });
        }


        [HttpGet]
        [Route("Code")]
        public async Task<IActionResult> Code([FromQuery(Name = "code")] string code)
        {
            var product = await repository.Read(code);
            if (product == null)
            {
                return NotFound(code);
            }
            return Ok(product);
        }

        [HttpGet]
        [Route("ByCategory")]
        public IEnumerable<Product> ByCategory([FromQuery(Name = "id")] int id)
        {
            return repository.Browse(id);
        }


        [HttpGet]
        [Route("RunningLow")]
        public async Task<IActionResult> LowStock([FromQuery(Name = "page")] int page, [FromQuery(Name = "per_page")] int per_page)
        {
            var paginatedList = await PaginatedList<Product>.CreateAsync(repository.RunningLow(), page, per_page);
            return Ok(new { Data = paginatedList, Page = paginatedList.PageIndex, Total = paginatedList.Total });
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await repository.Read(id);
            if (product == null)
            {
                return NotFound(id);
            }
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Post(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var p = new Product
                {
                    Discount = model.Discount,
                    PurchasePrice = model.PurchasePrice,
                    SalePrice = model.SalePrice,
                    Description = model.Description,
                    Code = model.Code,
                    Quantity = model.Quantity,
                    MinimumQuantity = model.MinimumQuantity,
                    CategoryId = model.CategoryId,
                    SupplierId = model.SupplierId
                };

                await repository.Add(p);
                var url = Url.Action("Get", new { id = p.Id });
                return Created(url, p);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/Product/
        [HttpPut]
        public async Task<IActionResult> Put(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var p = await repository.Read(model.Id);
                if (p == null)
                {
                    return NotFound(model.Id);
                }

                p.Discount = model.Discount;
                p.SalePrice = model.SalePrice;
                p.PurchasePrice = model.PurchasePrice;
                p.Description = model.Description;
                p.Quantity = model.Quantity;
                p.MinimumQuantity = model.MinimumQuantity;
                await repository.Edit(p);
                return Ok(p);
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
