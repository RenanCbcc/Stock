using System.Threading.Tasks;
using Stock_Back_End.Models;
using Stock_Back_End.Models.CategoryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock_Back_End.Models.ErrorModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Stock_Back_End.Models.ProductModels;

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
        public async Task<IActionResult> Products(int id, [FromQuery] ProductFilter filter, [FromQuery] EntityOrder order, [FromQuery] PaginationEntry pagination)
        {
            var list = await repository.Browse(id)
              .AplyFilter(filter)
              .AplyOrder(order)
              .ToEntityPaginated(pagination);

            return Ok(list);
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            var items = await repository.All();
            return Ok(items);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] CreatingCategoryModel model)
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

            return BadRequest(ErrorResponse.FromModelState(ModelState));
        }


        [HttpPut]
        [Authorize]
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