using Stock_Back_End.Models.OrderModels;
using Stock_Back_End.Models.PaymentModels;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Stock_Back_End.Models.ClientModels
{
    public class Client : Base
    {
        public Client()
        {
            Status = Status.Ativo;
            Debt = 0;
            Payments = new HashSet<Payment>();
            Orders = new HashSet<Order>();
        }

        public ISet<Payment> Payments { get; set; }
        public ISet<Order> Orders { get; set; }

        public DateTime LastPurchase { get; set; }

        public float Debt { get; set; }

        [SwaggerSchema("Can be 'Inativo' or 'Ativo'.", Format = "enum")]
        public Status Status { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

    }
}
