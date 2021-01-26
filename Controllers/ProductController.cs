using System.Threading.Tasks;
using Stock_Back_End.Models;
using Stock_Back_End.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Stock_Back_End.Models.ErrorModels;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Stock_Back_End.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository repository;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve a collections of products.")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Pagination<Product>))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        public async Task<IActionResult> Get([FromQuery] ProductFilter filter, [FromQuery] EntityOrder order, [FromQuery] PagingParams pagination)
        {
            var list = await repository.Browse()
                .AplyFilter(filter)
                .AplyOrder(order)
                .ToEntityPaginated(pagination);

            return Ok(list);
        }
                

        // GET: api/Product/5
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Retrieve a client identified by it's {id}")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Product))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await repository.Read(id);
            if (product == null)
            {
                return NotFound(id);
            }
            return Ok(product);
        }

        [HttpGet("{code}")]
        [SwaggerOperation(Summary = "Retrieve a client identified by it's {code}")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Product))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
        public async Task<IActionResult> GetByCode(string code)
        {
            var product = await repository.Read(code);
            if (product == null)
            {
                return NotFound(code);
            }
            return Ok(product);
        }


        // POST: api/Product
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new product.", Description = "Requires admin privileges")]
        [SwaggerResponse(201, "The client was created", typeof(string))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(400, "The was unable to processe the request.", typeof(ErrorResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post(CreatingProductModel model)
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

            return BadRequest(ErrorResponse.FromModelState(ModelState));
        }

        // PUT: api/Product/
        [HttpPut]
        [SwaggerOperation(Summary = "Modifies a client.", Description = "Requires admin privileges")]
        [SwaggerResponse(201, "The category modification has succeeded.", typeof(Product))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(400, "The was unable to processe the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(EditingProductModel model)
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
            return BadRequest(ErrorResponse.FromModelState(ModelState));
        }


    }
}
