using Stock_Back_End.Models.ProductModels;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stock_Back_End.Models.SupplierModels
{
    public class Supplier : Base
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        
        public ISet<Product> Products { get; set; }
    }
}
