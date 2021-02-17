using Stock_Back_End.Models.ProductModels;
using System.Collections.Generic;

namespace Stock_Back_End.Models.CategoryModels
{
    public class Category : Base
    {
        public Category()
        {
            Discount = 0;
            Products = new HashSet<Product>();
        }

        public string Title { get; set; }

        public float Discount { get; set; }

       
        public ISet<Product> Products { get; set; }
    }
}
