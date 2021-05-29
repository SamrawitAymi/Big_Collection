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
            private const string GATEWAY = "https://localhost:44341";

            public const string USERS_API_BASEURL = GATEWAY + "/user/";     
            public const string PRODUCTS_API_BASEURL = GATEWAY + "/product/";

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
        }
    }
}
