using System;
using System.Threading.Tasks;
using Stock_Back_End.Models;
using Stock_Back_End.Models.ClientModels;
using Stock_Back_End.Models.OrderModels;
using Stock_Back_End.Models.ProductModels;
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
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly IClientRepository clientRepository;
        private readonly IProductRepository productRepository;

        public OrderController(IOrderRepository orderRepository,
            IClientRepository clientRepository,
            IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.clientRepository = clientRepository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "per_page")] int per_page = 10)
        {
            var paginatedList = await PaginatedList<Order>.CreateAsync(orderRepository.Browse(), page, per_page);
            return Ok(new { Data = paginatedList, Page = paginatedList.PageIndex, Total = paginatedList.Total });
        }


        [SwaggerOperation(Summary = "Retrieve an order identified by it's {id}")]
        [SwaggerResponse(200, "The request has succeeded.", typeof(Order))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "The origin server did not find a current representation for the target resource or is not willing to disclose that one exists.", typeof(ErrorResponse))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await orderRepository.Read(id);
            if (order == null)
            {
                return NotFound(id);
            }
            return Ok(order);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new order.", Description = "Requires admin privileges")]
        [SwaggerResponse(201, "The category was created", typeof(string))]
        [SwaggerResponse(500, "The server encountered an unexpected condition that prevented it from fulfilling the request.", typeof(ErrorResponse))]
        [SwaggerResponse(400, "The was unable to processe the request.", typeof(ErrorResponse))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post(CreatingOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var client = await clientRepository.Read(model.CLientId);
                if (client == null)
                {
                    return NotFound($"Cliente com Id {model.CLientId} não foi encontrado.");
                }

                var order = new Order
                {
                    CLientId = model.CLientId,
                    Items = model.Items,
                    Date = DateTime.Now,
                };

                foreach (var item in model.Items)
                {
                    var p = await productRepository.Read(item.ProductId);
                    if (p == null)
                    {
                        return NotFound($"Produto com Id {item.ProductId} não foi encontrado.");
                    }
                    if (item.Quantity > p.Quantity)
                    {
                        return BadRequest("Quantidade em estoque insuficiente.");
                    }

                    item.Discound = p.Discount;
                    item.Value = p.SalePrice;
                    p.Quantity -= item.Quantity;
                    var total = (p.SalePrice * item.Quantity);
                    order.Value += total - (total * p.Discount);
                }

                client.Debt += order.Value;

                await orderRepository.Add(order);
                var url = Url.Action("Get", new { id = order.Id });
                var created = Created(url, order);
                return created;
            }

            return BadRequest(ModelState);
        }

    }
}