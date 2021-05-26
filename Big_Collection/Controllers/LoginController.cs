using Big_Collection.Common;
using Big_Collection.Models;
using Big_Collection.Services;
using Big_Collection.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Big_Collection.Controllers
{
    public class LoginController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICookieHandler _cookieHandler;

        public LoginController(ICookieHandler cookieHandler, IClientService clientService)
        {
            _cookieHandler = cookieHandler;
            _clientService = clientService;
        }

        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterPage()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> RegisterPageAsync(UserRegistrationViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.REGISTER_ENDPOINT, HttpMethod.Post, userRegister);

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    ViewBag.Exists = "User with Email " + userRegister.Email + " already exist!";
                }
                else if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LoginPage");
                }
            }

            return View();
        }
     

        [HttpPost]
        public async Task<IActionResult> LoginPageAsync(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.LOGIN_ENDPOINT, HttpMethod.Post, userLogin);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.Message = "Incorrect login credential";
                }
                else if (response.IsSuccessStatusCode)
                {
                    var logedInUser = await _clientService.ReadResponseAsync<LogedInUser>(response.Content);
                    await _cookieHandler.CreateLoginCookiesAsync(logedInUser, userLogin.Remember);

                    return RedirectToAction("Index", "Home");
                }

            }
            return View("LoginPage");
        }
    }
}
