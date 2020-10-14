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
                var c = await clientRepository.Read(model.CLientId);
                if (c == null)
                {
                    return NotFound($"Cliente com Id {model.CLientId} não foi encontrado.");
                }

                var o = new Order
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
                    p.Quantity -= item.Quantity;
                    o.Value += p.SalePrice * item.Quantity;                    
                }


                await orderRepository.Add(o);
                var url = Url.Action("Get", new { id = o.Id });
                var created = Created(url, o);
                return created;
            }

            return BadRequest(ModelState);
        }

    }
}