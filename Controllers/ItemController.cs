using Stock_Back_End.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Stock_Back_End.Models.ErrorModels;

namespace Stock_Back_End.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository repository;

        public ItemController(IItemRepository repository)
        {
            this.repository = repository;
        }

        [SwaggerOperation(Summary = "Retrieve a item identified by it's {id}")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Item))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var items = await repository.Read(id);
            if (items == null)
            {
                return NotFound(id);
            }
            return Ok(items);
        }

    }
}