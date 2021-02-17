using Stock_Back_End.Models.CategoryModels;
using Stock_Back_End.Models.SupplierModels;

namespace Stock_Back_End.Models.ProductModels
{
    public class Product : Base
    {
        public Product()
        {
            Discount = 0;
        }

        public Category Category { get; set; }

        public Supplier Supplier { get; set; }

        public float Discount { get; set; }

        public float PurchasePrice { get; set; }

        public float SalePrice { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public int Quantity { get; set; }

        public int MinimumQuantity { get; set; }

    }
}
