using System.Threading.Tasks;
using Stock_Back_End.Models;
using Stock_Back_End.Models.CategoryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock_Back_End.Models.ErrorModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Stock_Back_End.Models.ProductModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Stock_Back_End.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository repository;

        public CategoryController(ICategoryRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve a collections of categories.")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Pagination<Category>))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        public async Task<IActionResult> Get([FromQuery] CategoryFilter filter, [FromQuery] EntityOrder order, [FromQuery] PaginationEntry pagination)
        {
            var list = await repository.Browse()
                .AplyFilter(filter)
                .AplyOrder(order)
                .ToEntityPaginated(pagination);

            return Ok(list);
        }

        [HttpGet]
        [Route("{id}/Product")]
        [SwaggerOperation(Summary = "Retrieve a collection of products belonging to a specific category.")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Pagination<Product>))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        public async Task<IActionResult> Products(int id, [FromQuery] ProductFilter filter, [FromQuery] EntityOrder order, [FromQuery] PaginationEntry pagination)
        {
            var list = await repository.Browse(id)
              .AplyFilter(filter)
              .AplyOrder(order)
              .ToEntityPaginated(pagination);

            return Ok(list);
        }
                

        [SwaggerOperation(Summary = "Retrieve a category identified by it's {id}")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Category))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
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
        [SwaggerOperation(Summary = "Creates a new category.", Description = "Requires admin privileges")]
        [SwaggerResponse(201, "The category was created", typeof(string))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(400, "The was unable to processe the request.", typeof(ErrorResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] CreatingCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var c = new Category
                {
                    Title = model.Title,
                    Discount = model.Discount
                };

                await repository.Add(c);
                var url = Url.Action("Get", new { id = c.Id });
                return Created(url, c);
            }

            return BadRequest(ErrorResponse.FromModelState(ModelState));
        }


        [HttpPut]
        [SwaggerOperation(Summary = "Modifies a category.", Description = "Requires admin privileges")]
        [SwaggerResponse(201, "The category modification has succeeded.", typeof(Category))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(400, "The was unable to processe the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(EditingCategoryModel model)
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
            return BadRequest(ErrorResponse.FromModelState(ModelState));
        }


    }
}