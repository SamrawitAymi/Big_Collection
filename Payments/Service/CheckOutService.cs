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
                    //ReturnUrl = "https://localhost:44367/api/Payment/verifypayment",
                    ReturnUrl = "https://localhost:44343/Order/GetPayPalPayVerifyPayment",

                    //https://localhost:44362/Orders/OrderConfirmationPage?orderId=&token=66322200JJ012074J&PayerID=HU7CLT2D5N5KE
                    CancelUrl = "https://localhost:44367/api/Payment/create"
                }
            };


            // Call API with your client and get a response for your call
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(order);
            response =await Client().Execute(request);
            var statusCode = response.StatusCode;
            Order result = response.Result<Order>();   
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
            return response;
        }
    }
}
