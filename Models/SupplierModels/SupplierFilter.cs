using System.Linq;

namespace Stock_Back_End.Models.SupplierModels
{
    public static class SupplierFilterExtentions
    {
        public static IQueryable<Supplier> AplyFilter(this IQueryable<Supplier> query, SupplierFilter filter)
        {
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Name))
                {
                    query = query.Where(s => s.Name.Contains(filter.Name));
                }

                if (!string.IsNullOrEmpty(filter.Email))
                {
                    query = query.Where(s => s.Email.Contains(filter.Email));
                }

                if (!string.IsNullOrEmpty(filter.PhoneNumber))
                {
                    query = query.Where(s => s.PhoneNumber.Contains(filter.PhoneNumber));
                }


            }
            return query;
        }
    }

    public class SupplierFilter
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
