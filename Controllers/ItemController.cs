using Estoque.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository repository;

        public ItemController(IItemRepository repository)
        {
            this.repository = repository;
        }

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