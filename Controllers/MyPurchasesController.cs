﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Layout.Controllers
{
    public class MyPurchasesController : Controller
    {
        public IActionResult MyPurchases()
        {
            return View("MyPurchases");
        }
    }
}
