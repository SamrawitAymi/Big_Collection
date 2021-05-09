using System;
using System.ComponentModel.DataAnnotations;

namespace Orders
{
    public class OrderProducts
    {
        [Key]
        public Guid ProductId { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public int Amount { get; set; }

    }
}