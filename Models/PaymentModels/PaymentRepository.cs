using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Back_End.Models.PaymentModels
{
    public interface IPaymentRepository
    {
        IQueryable<Payment> Browse();
        Task Add(Payment payment);
        Task<float> Total(int id);
    }
    public class PaymentRepository : IPaymentRepository
    {
        private readonly StockContext context;

        public PaymentRepository(StockContext context)
        {
            this.context = context;
        }

        public async Task Add(Payment payment)
        {
            await context.AddAsync(payment);
            await context.SaveChangesAsync();
        }

        public IQueryable<Payment> Browse()
        {
            return context.Payments.Include(p => p.Client);
        }

        public async Task<float> Total(int id)
        {
            return await context.Payments.Where(p => p.Client.Id == id).SumAsync(p => p.Amount);
        }
    }
}
