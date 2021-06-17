using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Big_Collection.Common
{
    public class ApiGateways
    {
        public static class ApiGateway
        {
           
            // API gateway
            public const string GATEWAY = "https://localhost:44341";

            public const string USERS_API_BASEURL = GATEWAY + "/user/";     
            public const string PRODUCTS_API_BASEURL = GATEWAY + "/product/";
            public const string ORDERS_GATEWAY_BASEURL = GATEWAY + "/order/";
            public const string PAYMENT_GATEWAY_BASEURL = GATEWAY + "/payment/";


            // Users API endpoints
            public const string LOGIN_ENDPOINT = USERS_API_BASEURL + "login/";
            public const string REGISTER_ENDPOINT = USERS_API_BASEURL + "register/";
            public const string REQUEST_NEW_TOKEN_ENDPOINT = USERS_API_BASEURL + "token/";
            public const string GET_USER = USERS_API_BASEURL + "getuserbyid/";
            public const string CHECK_EMAIL_AVAILABILITY = USERS_API_BASEURL + "verifyemail/";

            // Products-api endpoints
            public const string ALL_PRODUCTS = PRODUCTS_API_BASEURL + "getall/"; // =  https://localhost:44341/api/product/{catchAll}
            public const string CREATE_PRODUCT = PRODUCTS_API_BASEURL + "create/";
            public const string EDIT_PRODUCT = PRODUCTS_API_BASEURL + "edit/";
            public const string DELETE_PRODUCT = PRODUCTS_API_BASEURL + "delete/";
            public const string GET_CATEGORY = PRODUCTS_API_BASEURL + "category?searchProduct=";

            // Orders-api endpoints
            public const string ALL_ORDERS = ORDERS_GATEWAY_BASEURL + "getall/";
            public const string CREATE_ORDER_GATEWAY = "https://localhost:44341/api/Aggregate/";
            public const string CREATE_ORDER = ORDERS_GATEWAY_BASEURL + "create/";
            public const string GET_ORDER_BY_USERID = ORDERS_GATEWAY_BASEURL + "user/";

            // payment-api endpoints
            public const string PAYMENT_CREATE = PAYMENT_GATEWAY_BASEURL + "create";
            public const string VERIFY_PAYMENT = PAYMENT_GATEWAY_BASEURL + "verifypayment/";

        }
    }
}
