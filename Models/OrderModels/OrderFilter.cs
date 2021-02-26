using System;
using System.Linq;

namespace Stock_Back_End.Models.OrderModels
{
    public static class OrderFilterExtentions
    {
        public static IQueryable<Order> AplyFilter(this IQueryable<Order> query, OrderFilter filter)
        {
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Client))
                {
                    query = query.Where(o => o.Client.Name.Contains(filter.Client));
                }

                if (filter.ClientId != 0)
                {
                    query = query.Where(o => o.Client.Id == filter.ClientId);
                }

                if (filter.ValueBiggerThan != 0)
                {
                    query = query.Where(o => o.Value > filter.ValueBiggerThan);
                }

                if (filter.ValueLessThan != 0)
                {
                    query = query.Where(o => filter.ValueLessThan > o.Value);
                }

                if (filter.Before != null)
                {
                    query = query.Where(o => filter.Before > o.Date);
                }

                if (filter.After != null)
                {
                    query = query.Where(o => o.Date > filter.After);
                }


            }
            return query;
        }

    }
    public class OrderFilter
    {
        public string Client { get; set; }
        public int ClientId { get; set; }
        public float ValueBiggerThan { get; set; }
        public float ValueLessThan { get; set; }
        public DateTime? Before { get; set; }
        public DateTime? After { get; set; }
    }
}
