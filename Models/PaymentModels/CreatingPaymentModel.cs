﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stock_Back_End.Models.PaymentModels
{
    public class CreatingPaymentModel
    {
        public int ClientId { get; set; }

        [DataType(DataType.Currency)]
        public float Value { get; set; }
    }
}
