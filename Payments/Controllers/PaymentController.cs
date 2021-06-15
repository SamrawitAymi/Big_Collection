using Microsoft.AspNetCore.Mvc;
using Payments.Service;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Payments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        

        [HttpGet("create")]
        public async Task<ActionResult<OrderPayment>> CreatePayment(string amount, string currency)
        {
            var checkoutPayment = new CheckOutService(amount, currency);
            var result = await checkoutPayment.CreateOrder();
            Order order = result.Result<Order>();
            var orderPayment = new OrderPayment
            {
                OrderPaymentId = order.Id
            };
            SaveOrderId(order.Id);
            foreach (var link in order.Links)
            {
                if (link.Rel.Equals("approve"))
                {
                    orderPayment.PayPalLink = link.Href;

                    return Ok(orderPayment);
                }   
            }
            return NotFound();      
        }


        [HttpGet("verifypayment")]
        public async Task<ActionResult<OrderPayment>>VerifyPayment()
        {
            var checkoutPayment = new CheckOutService();
            var result = await checkoutPayment.CaptureOrder(GetOrderId());
            Order order = result.Result<Order>();
            var orderPayment = new OrderPayment
            {
                OrderPaymentId = order.Id
            };
            return Ok(orderPayment.OrderPaymentId);
        }

        private string SaveOrderId(string orderId)
        {
             System.IO.File.WriteAllText(@"C:\Users\samri\source\repos\Big_Collection\Payments\Service\PaymentOrder.txt", orderId);
             return orderId;
        }

        private string GetOrderId()
        {
            return System.IO.File.ReadAllText(@"C:\Users\samri\source\repos\Big_Collection\Payments\Service\PaymentOrder.txt");
        }


    }
}
