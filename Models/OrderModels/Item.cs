
using Stock_Back_End.Models.ProductModels;

namespace Stock_Back_End.Models.OrderModels
{
    public class Item : Base
    {

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public float Discound { get; set; }
        public float Value { get; set; }


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
