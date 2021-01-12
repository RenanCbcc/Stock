using System;
using System.Threading.Tasks;
using Stock_Back_End.Models;
using Stock_Back_End.Models.ClientModels;
using Stock_Back_End.Models.OrderModels;
using Stock_Back_End.Models.PaymentModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Stock_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IClientRepository clientRepository;
        private readonly IOrderRepository orderRepository;

        public PaymentController(
            IPaymentRepository paymentRepository,
            IClientRepository clientRepository,
            IOrderRepository orderRepository)
        {
            this.paymentRepository = paymentRepository;
            this.clientRepository = clientRepository;
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "page")] int page=1, [FromQuery(Name = "per_page")] int per_page=10)
        {
            var paginatedList = await PaginatedList<Payment>.CreateAsync(paymentRepository.Browse(), page, per_page);
            return Ok(new
            {
                Data = paginatedList,
                Page = paginatedList.PageIndex,
                Total = paginatedList.Total
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreatePaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var c = await clientRepository.Read(model.ClientId);
                if (c == null)
                {
                    return NotFound($"Cliente com Id {model.ClientId} não foi encontrado.");
                }

                if (model.Value <= 0 || model.Value > c.Debt)
                {
                    return BadRequest($"O valor precisa ser maior que R$ 0 e menor que R$ {c.Debt}");
                }

                c.Debt -= model.Value;
                c.LastPurchase = DateTime.Now;

                if (c.Debt == 0)
                {
                    var pendings = await orderRepository.Pending(c.Id);
                    foreach (var order in pendings)
                    {
                        order.Status = Models.OrderModels.Status.Pago;
                    }
                }

                var p = new Payment
                {
                    ClientId = model.ClientId,
                    Amount = model.Value,
                    Date = DateTime.Now,
                };

                await paymentRepository.Add(p);
                var url = Url.Action("Get", new { id = p.Id });
                var created = Created(url, p);
                return created;
            }

            return BadRequest(ModelState);
        }
    }
}