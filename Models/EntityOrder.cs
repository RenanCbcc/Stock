using System.Linq;
using System.Linq.Dynamic.Core;

namespace Stock_Back_End.Models
{

    public static class EntityOrderExtentions
    {
        public static IQueryable<T> AplyOrder<T>(this IQueryable<T> query, EntityOrder order)
        {
            if (order != null)
            {
                query = query.OrderBy(order.OrderBy);
            }
            return query;
        }
    }

    public class EntityOrder
    {
        public string OrderBy { get; set; } = "Id";
    }
}
