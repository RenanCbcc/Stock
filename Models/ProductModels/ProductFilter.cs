using System.Linq;

namespace Stock_Back_End.Models.ProductModels
{
    public static class ProductFilterExtentions
    {
        public static IQueryable<Product> AplyFilter(this IQueryable<Product> query, ProductFilter filter)
        {
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Description))
                {
                    query = query.Where(p => p.Description.Contains(filter.Description));
                }

                if (filter.DiscountBiggerThan != 0)
                {
                    query = query.Where(p => p.Discount > filter.DiscountBiggerThan);
                }

                if (filter.DiscountLessThan != 0)
                {
                    query = query.Where(p => p.Discount < filter.DiscountLessThan);
                }
            }
            return query;
        }

    }
    public class ProductFilter
    {
        public string Description { get; set; }
        public float DiscountBiggerThan { get; set; }
        public float DiscountLessThan { get; set; }
        public float PurchasePriceBiggerThan { get; set; }
        public float PurchasePriceLessThan { get; set; }
        public int QuantityBiggerThan { get; set; }
        public int QuantityLessThan { get; set; }
    }
}
