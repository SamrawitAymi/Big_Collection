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
        public async Task<ActionResult<Order>> CreatePayment(string amount, string currency)
        {
            var checkoutPayment = new CheckOutService(amount, currency);
            var result = await checkoutPayment.CreateOrder();
            Order order = result.Result<Order>();
            SaveOrderId(order.Id);
            return Ok(order);      
        }


        [HttpGet("verifypayment")]
        public async Task<ActionResult<Order>>VerifyPayment()
        {
            var checkoutPayment = new CheckOutService();
            var result = await checkoutPayment.CaptureOrder(GetOrderId());
            Order order = result.Result<Order>();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            Console.WriteLine("Status: {0}", order.Status);
            Console.WriteLine("Capture Id: {0}", order.Id);
            return Ok(order);
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
