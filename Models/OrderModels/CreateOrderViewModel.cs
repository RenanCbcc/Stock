using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Estoque.Models.OrderModels
{
    public class CreateOrderViewModel
    {
        public CreateOrderViewModel()
        {
            Items = new HashSet<Item>();
        }

        public int CLientId { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public HashSet<Item> Items { get; set; }
    }
}
