using System.Threading.Tasks;
using Estoque.Models;
using Estoque.Models.ClientModels;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository repository;

        public ClientController(IClientRepository repository)
        {
            this.repository = repository;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "page")] int page, [FromQuery(Name = "per_page")] int per_page)
        {
            var paginatedList = await PaginatedList<Client>.CreateAsync(repository.Browse(), page, per_page);
            return Ok(new { Data = paginatedList, Page = paginatedList.PageIndex, Total = paginatedList.Total });
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var client = await repository.Read(id);
            if (client == null)
            {
                return NotFound(id);
            }
            return Ok(client);
        }

        
        [HttpPost]
        public async Task<IActionResult> Post(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var c = new Client
                {
                    Name = model.Name,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                };

                await repository.Add(c);
                var url = Url.Action("Get", new { id = c.Id });
                return Created(url, c);
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

                c.Status = model.Status;
                c.Name = model.Name;
                c.Address = model.Address;
                c.PhoneNumber = model.PhoneNumber;

                await repository.Edit(c);
                return Ok(c);
            }
            return BadRequest(ModelState);
        }


    }
}