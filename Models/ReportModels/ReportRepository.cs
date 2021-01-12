using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Back_End.Models.ReportModels
{
    public interface IReportRepository
    {
        Task<IEnumerable<object>> Balance();
    }
    public class ReportRepository : IReportRepository
    {
        private readonly StockContext context;

        public ReportRepository(StockContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<object>> Balance()
        {
            var query = from o in context.Orders
                        group o by new { month = o.Date.Month, year = o.Date.Year } into grouped
                        select new { date = string.Format("{0}/{1}", grouped.Key.month, grouped.Key.year), sale = grouped.Sum(o => o.Value) };

            return await query.ToListAsync();
        }


    }
}
