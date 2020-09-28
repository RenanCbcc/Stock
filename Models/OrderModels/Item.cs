
using Estoque.Models.ProductModels;

namespace Estoque.Models.OrderModels
{
    public class Item : Base
    {

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public override bool Equals(object obj)
        {
            Item item = obj as Item;
            if (item == null)
            {
                return false;
            }

            return this.ProductId == item.ProductId;
        }

        public override int GetHashCode()
        {
            return this.ProductId.GetHashCode();
        }
    }
}
