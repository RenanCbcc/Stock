using System.Collections.Generic;
using System.Threading.Tasks;
using Stock_Back_End.Models;
using Stock_Back_End.Models.SupplierModels;
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
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository repository;

        public SupplierController(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieve a collections of suppliers.")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Pagination<Supplier>))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        public async Task<IActionResult> Get([FromQuery] SupplierFilter filter, [FromQuery] EntityOrder order, [FromQuery] PagingParams pagination)
        {
            var list = await repository.Browse()
                .AplyFilter(filter)
                .AplyOrder(order)
                .ToEntityPaginated(pagination);

            return Ok(list);
        }


        [SwaggerOperation(Summary = "Retrieve a supplier identified by it's {id}")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Supplier))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var supplier = await repository.Read(id);
            if (supplier == null)
            {
                return NotFound(id);
            }
            return Ok(supplier);
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new supplier.", Description = "Requires admin privileges")]
        [SwaggerResponse(201, "The category was created", typeof(string))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(400, "The was unable to processe the request.", typeof(ErrorResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post(CreatingSupplierModel model)
        {
            if (ModelState.IsValid)
            {
                var s = new Supplier
                {
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email
                };

                await repository.Add(s);
                var url = Url.Action("Get", new { id = s.Id });
                return Created(url, s);
            }

            return BadRequest(ErrorResponse.FromModelState(ModelState));
        }


        [HttpPut]
        [SwaggerOperation(Summary = "Modifies a supplier.", Description = "Requires admin privileges")]
        [SwaggerResponse(201, "The category modification has succeeded.", typeof(Supplier))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(400, "The was unable to processe the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(EditingSupplierModel model)
        {
            if (ModelState.IsValid)
            {
                var s = await repository.Read(model.Id);
                if (s == null)
                {
                    return NotFound(model.Id);
                }

                s.Name = model.Name;
                s.Email = model.Email;
                s.PhoneNumber = model.PhoneNumber;
                await repository.Edit(s);
                return Ok(s);
            }
            return BadRequest(ErrorResponse.FromModelState(ModelState));
        }

    }
}