using Big_Collection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Big_Collection.ViewModels
{
    public class OrderViewModel
    {
        public List<CartItem> Cart { get; set; }
        public Payment Payment { get; set; }
        public User User { get; set; }
       
    }

    public class OrderSuccessViewModel
    {
        public Order Order { get; set; }
        public User User { get; set; }
    }
}
