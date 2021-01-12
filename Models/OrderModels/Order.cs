using Stock_Back_End.Models.ClientModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Stock_Back_End.Models.OrderModels
{
    public class Order : Base
    {
        public Order()
        {
            this.Items = new HashSet<Item>();
            this.Status = Status.Pendende;
        }
        public int CLientId { get; set; }
        public Client Client { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        public float Value { get; set; }
        public Status Status { get; set; }

        [Required]
        [JsonIgnore]
        public HashSet<Item> Items { get; set; }

    }
}
