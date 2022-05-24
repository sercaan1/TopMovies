using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopMovies.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
