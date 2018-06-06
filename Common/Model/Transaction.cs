using System;

namespace Common
{
    [Serializable ()]
    public class Transaction
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public double Amount { get; set; } 
    }
}