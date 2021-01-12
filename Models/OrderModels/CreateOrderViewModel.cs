using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.OrderModels
{
    public class CreateOrderViewModel
    {
        public CreateOrderViewModel()
        {
            Items = new HashSet<Item>();
        }

        public int CLientId { get; set; }
        
        [Required]
        public HashSet<Item> Items { get; set; }
    }
}
