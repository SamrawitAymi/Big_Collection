using System;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Payments.Service
{
    public class CheckOutService
    {

        static String clientId = "AXfgT7UNWGhNEaxlf33P2hDC1j-wLkwagQxWRo7pERswcm0i_hmimWKMTAqRb74zw4zYi8wzSKQHuDmB";
        static String secret = "ENV3ZECKLPoGlQYlIAoeSL7If8c6rhW3ffaWaKhJALTavk__KD4Xg0wWnnQeRqi-bzgZXeIPEu4Y74Ti";

        public string Amount { get; set; }
        public string Currency { get; set; }

        public CheckOutService(string amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public CheckOutService()
        {
        }
        public  HttpClient Client()
        {
            // Creating a sandbox environment
            PayPalEnvironment environment = new SandboxEnvironment(clientId, secret);

            // Creating a client for the environment
            PayPalHttpClient client = new PayPalHttpClient(environment);
            return client;
        }

        public async Task<HttpResponse> CreateOrder()
        {
            HttpResponse response;
            // Construct a request object and set desired parameters
            // Here, OrdersCreateRequest() creates a POST request to /v2/checkout/orders
            var order = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = Currency,
                            Value = Amount
                        }
                    }
                },
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = "https://localhost:44367/api/Payment/verifypayment",
                    CancelUrl = "https://localhost:44367/api//api/Payment/create"
                }
            };


            // Call API with your client and get a response for your call
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(order);
            response =await Client().Execute(request);
            var statusCode = response.StatusCode;
            Order result = response.Result<Order>();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("Status: {0}", result.Status);
            Console.WriteLine("Order Id: {0}", result.Id);
            Console.WriteLine("Intent: {0}", result.CheckoutPaymentIntent);
            Console.WriteLine("Links:");
            foreach (LinkDescription link in result.Links)
            {
                Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
            }
            
            return response;

        }


        public async Task<HttpResponse> CaptureOrder(string orderId)
        {
           
            // Construct a request object and set desired parameters
            // Replace ORDER-ID with the approved order id from create order
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());
            HttpResponse response = await Client().Execute(request);
            var statusCode = response.StatusCode;
            Order result = response.Result<Order>();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("Status: {0}", result.Status);
            Console.WriteLine("Capture Id: {0}", result.Id);
            return response;
        }
    }
}
