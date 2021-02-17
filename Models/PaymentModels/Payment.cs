using Stock_Back_End.Models.ClientModels;
using System;

namespace Stock_Back_End.Models.PaymentModels
{
    public class Payment : Base
    {
        public Client Client { get; set; }

        public DateTime Date { get; set; }

        public float Amount { get; set; }
                
    }
}
