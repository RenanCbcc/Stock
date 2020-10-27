using System;
using System.Threading.Tasks;
using Estoque.Models;
using Estoque.Models.ClientModels;
using Estoque.Models.OrderModels;
using Estoque.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;

namespace Estoque.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Get([FromQuery(Name = "page")] int page, [FromQuery(Name = "per_page")] int per_page)
        {
            var paginatedList = await PaginatedList<Order>.CreateAsync(orderRepository.Browse(), page, per_page);
            return Ok(new { Data = paginatedList, Page = paginatedList.PageIndex, Total = paginatedList.Total });
        }


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
        public async Task<IActionResult> Post(CreateOrderViewModel model)
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

        [Route("ClientAmount")]
        public async Task<IActionResult> ClientAmount([FromQuery(Name = "clientId")] int clientId)
        {
            var amount = await orderRepository.Total(clientId);
            return Ok(amount);
        }
    }
}