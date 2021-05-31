using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments.Models
{
    public class Ticket
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string Email { get; set; }
        public DateTime TourDate { get; set; }
        public string PayReferene { get; set; }
    }
}
