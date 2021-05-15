using Big_Collection.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult RegisterPage(UserRegistrationViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
              
            }
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }
    }
}
