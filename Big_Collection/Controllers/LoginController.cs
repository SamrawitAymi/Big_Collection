using Big_Collection.Models;
using Big_Collection.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Big_Collection.Controllers
{
    public class LoginController : Controller
    {
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
        public async Task<ActionResult> RegisterPageAsync(UserRegistrationViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44342/user/register");
                        string json = JsonConvert.SerializeObject(userRegister);
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await client.SendAsync(request);
                        var responseMessage = response.Content;
                        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            ViewBag.Exists = "Users with Email " + userRegister.Email + "  already exists!";
                        }
                        else if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("LoginPage");
                        }
                    }

                }
                return View();
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginPageAsync(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44342/user/login");
                    string json = JsonConvert.SerializeObject(userLogin);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.SendAsync(request);
                    var responseMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ViewBag.Message = "Incorrect login information";
                    }
                    else if (response.IsSuccessStatusCode)
                    {
                        LogedInUser loggedInUser = JsonConvert.DeserializeObject<LogedInUser>(responseMessage);
                        //ViewBag.Message = "Du är nu inloggad, " + loggedInUser.User.FirstName;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View("LoginPage");
        }
    }
}
