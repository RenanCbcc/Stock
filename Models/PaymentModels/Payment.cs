﻿using Stock_Back_End.Models.ClientModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace Stock_Back_End.Models.PaymentModels
{
    public class Payment : Base
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        
        [DataType(DataType.Currency)]
        public float Amount { get; set; }


    }
}
