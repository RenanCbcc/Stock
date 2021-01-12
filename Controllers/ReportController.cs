using System.Threading.Tasks;
using Stock_Back_End.Models.OrderModels;
using Stock_Back_End.Models.PaymentModels;
using Stock_Back_End.Models.ReportModels;
using Microsoft.AspNetCore.Mvc;

namespace Stock_Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository repository;

        public ReportController(IReportRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("Balance")]
        public async Task<IActionResult> Balance()
        {
            var orders = await repository.Balance();
            return Ok(orders);
        }

    }
}