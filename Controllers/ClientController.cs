using System.Threading.Tasks;
using Stock_Back_End.Models;
using Stock_Back_End.Models.ClientModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Stock_Back_End.Models.ErrorModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Stock_Back_End.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository repository;

        public ClientController(IClientRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve a collections of clients.")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Pagination<Client>))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        public async Task<IActionResult> Get([FromQuery] ClientFilter filter, [FromQuery] EntityOrder order, [FromQuery] PagingParams pagination)
        {
            var list = await repository.Browse()
                .AplyFilter(filter)
                .AplyOrder(order)
                .ToEntityPaginated(pagination);

            return Ok(list);
        }


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieve a client identified by it's {id}")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Client))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
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
        [SwaggerOperation(Summary = "Creates a new client.", Description = "Requires admin privileges")]
        [SwaggerResponse(201, "The client was created", typeof(string))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(400, "The was unable to processe the request.", typeof(ErrorResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post(CreatingClientModel model)
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
        [SwaggerOperation(Summary = "Modifies a client.", Description = "Requires admin privileges")]
        [SwaggerResponse(201, "The category modification has succeeded.", typeof(Client))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(400, "The was unable to processe the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(EditingClientModel model)
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