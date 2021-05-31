using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Models
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string Intent { get; set; }
        public Payer Payer { get; set; }
        public Transaction[] Transactions { get; set; }
        public RedirectUrls redirectUrls { get; set; }
        

    }

    
}
