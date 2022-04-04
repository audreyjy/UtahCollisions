using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UtahCollisions.Models;

namespace UtahCollisions.Controllers
{
    public class HomeController : Controller
    {
        // added for repository pattern 
        private iUtahCollisionRepository repo; 
        public HomeController (iUtahCollisionRepository temp)
        {
            repo = temp; 
        }

        public IActionResult Index()
        {
            var blah = repo.UtahCrashData.ToList(); 
            return View();
        }

        public IActionResult SummaryData()
        {
            return View(); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
