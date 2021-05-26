using Big_Collection.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Big_Collection.Controllers
{
    public class LogoutController : ControllerBase
    {
        private readonly ICookieHandler _cookieHandler;

        public LogoutController(ICookieHandler cookieHandler)
        {
            _cookieHandler = cookieHandler;
        }

        public IActionResult Index()
        {
            _cookieHandler.DestroyAllCookies();
            return RedirectToAction("Index", "Home");
        }
    }
}
