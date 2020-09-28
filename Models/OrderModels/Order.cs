using Estoque.Models.ClientModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Estoque.Models.OrderModels
{
    public class Order : Base
    {
        public Order()
        {
            this.Items = new HashSet<Item>();
        }
        public int CLientId { get; set; }
        public Client Client { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        public float Value { get; set; }
        public Status Status { get; set; }

        [Required]
        public HashSet<Item> Items { get; set; }

    }
}
