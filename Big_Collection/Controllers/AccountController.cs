using Big_Collection.Common;
using Big_Collection.Models;
using Big_Collection.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Big_Collection.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICookieHandler _cookieHandler;

        public AccountController(IClientService clientService, ICookieHandler cookieHandler)
        {
            this._clientService = clientService;
            this._cookieHandler = cookieHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var user = await GetUserAsync();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            if (ModelState.IsValid)
            {
                var canBeRegistered = await EmailAvailableForRegistrationAsync(user.Email);
                if (canBeRegistered)
                {
                    var loggedInUserId = await _cookieHandler.GetClaimFromAuthenticationCookieAsync("UserId");
                    var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.USERS_API_BASEURL + loggedInUserId, HttpMethod.Put, user);

                    if (response.IsSuccessStatusCode)
                    {
                        var updatedUser = await _clientService.ReadResponseAsync<User>(response.Content);
                        TempData["UpdateSuccess"] = "success";
                        return RedirectToAction(nameof(Update));
                    }
                }
                else
                    ViewBag.EmailTaken = "Email Taken";
            }

            return View(user);
        }

        private async Task<bool> EmailAvailableForRegistrationAsync(string email)
        {
            if (await IsSubmitedEmailDifferentFromCurrentEmail(email))
                return true;

            return await IsNewEmailAlreadyRegisteredAsync(email);
        }

        private async Task<bool> IsSubmitedEmailDifferentFromCurrentEmail(string email)
        {
            var user = await GetUserAsync();
            var currentEmail = user.Email;
            return (currentEmail.Equals(email));
        }

        private async Task<bool> IsNewEmailAlreadyRegisteredAsync(string updateEmail)
        {
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.CHECK_EMAIL_AVAILABILITY + updateEmail, HttpMethod.Get);

            if (response.IsSuccessStatusCode)
            {
                var isRegistered = await _clientService.ReadResponseAsync<bool>(response.Content);
                return (isRegistered) ? false : true;
            }

            return false;
        }

        private async Task<User> GetUserAsync()
        {
            var userId = await _cookieHandler.GetClaimFromAuthenticationCookieAsync("UserId");
            var response = await _clientService.SendRequestToGatewayAsync(ApiGateways.ApiGateway.GET_USER + userId, HttpMethod.Get);
            var user = await _clientService.ReadResponseAsync<User>(response.Content);

            return user;
        }
    }
}
