using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estoque.Models.ProductModels;
using Microsoft.AspNetCore.Http;
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

        // GET: api/Product
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return repository.Browse();
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
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
                    //CatgoryId = model.CatgoryId,
                    //SupplierId = model.SupplierId
                };

                await repository.Add(p);
                var url = Url.Action("Read", new { id = p.Id });
                return Created(url, p);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
