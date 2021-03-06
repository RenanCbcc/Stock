﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Back_End.Models.OrderModels
{
    public interface IOrderRepository
    {
        Task<Order> Read(int Id);
        IQueryable<Order> Browse();
        IQueryable<Item> Browse(int Id);
        Task<IEnumerable<Order>> Pending(int id);
        Task Add(Order order);
        Task Edit(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly StockContext context;

        public OrderRepository(StockContext context)
        {
            this.context = context;
        }

        public async Task Add(Order order)
        {
            await context.AddAsync(order);
            await context.SaveChangesAsync();
        }


        public IQueryable<Order> Browse()
        {
            return context.Orders.Include(o => o.Client);
        }

        public IQueryable<Item> Browse(int Id)
        {
            return context.Items.Include(i => i.Product).Where(i => i.OrderId == Id);
        }

        public async Task Edit(Order alteredOrder)
        {
            var order = context.Orders.Attach(alteredOrder);
            order.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<Order> Read(int Id)
        {
            return await context.Orders.FindAsync(Id);
        }

        public async Task<IEnumerable<Order>> Pending(int id)
        {
            return await context.Orders
                .Where(o => o.Client.Id == id)
                .Where(o => o.Status == Status.Pendende)
                .OrderBy(o => o.Value)
                .ToListAsync();
        }
    }
}
