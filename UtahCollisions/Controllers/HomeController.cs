using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UtahCollisions.Models;
using UtahCollisions.Models.ViewModels;

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
            return View();
        }

        public IActionResult SummaryData(int pageNum = 1)
        {
            int pageSize = 100;

            var x = new CollisionsViewModel
            {
                Utah_Crash_Data_2020 = repo.Utah_Crash_Data_2020
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotalNumCollisions = repo.Utah_Crash_Data_2020.Count(),
                    CollisionsPerPage = pageSize,
                    CurrentPage = pageNum
                }

            }; 

            return View(x); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
