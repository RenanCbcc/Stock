using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Models.PaymentModels
{
    public class CreatePaymentViewModel
    {
        public int ClientId { get; set; }

        [DataType(DataType.Currency)]
        public float Value { get; set; }
    }
}
