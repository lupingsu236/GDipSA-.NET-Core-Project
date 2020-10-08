using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Layout.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            List<string> cart = new List<string>() { "product1", "product2", "product3" };
            ViewData["cart"] = cart;
            return View();
        }
    }
}
