using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private UtahCollisionsContext utahCollisions;
        public HomeController (iUtahCollisionRepository temp, UserManager<IdentityUser> um, SignInManager<IdentityUser> sim, UtahCollisionsContext UCC)
        {
            repo = temp;
            userManager = um;
            signInManager = sim;
            utahCollisions = UCC;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET SummaryData for Authenticated Users
        [Authorize] 
        public IActionResult SummaryData(string city, string severityID, int pageNum = 1)
        {
            int pageSize = 100;

            if (severityID is null && city is null)
            {
                ViewBag.Header = "All Records";
            }
            else if (city is null && severityID != null)
            {
                ViewBag.Header = "Level " + severityID + " Severity Collision Records"; 
            }
            else if (city != null)
            {
                ViewBag.Header = city + " City Collision Records"; 
            }

            var x = new CollisionsViewModel
            {

                Utah_Crash_Data_2020 = repo.Utah_Crash_Data_2020
                .Where(x => x.CRASH_SEVERITY_ID.ToString() == severityID || severityID == null)
                .Where(x => x.CITY == city || city == null)
                .OrderBy(x => x.CRASH_ID)
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

        // GET Summary Data for Non-Authenticated Users
        public IActionResult SummaryDataNonAuth(string city, string severityID, int pageNum = 1)
        {
            int pageSize = 100;

            if (severityID is null && city is null)
            {
                ViewBag.Header = "All Records";
            }
            else if (city is null && severityID != null)
            {
                ViewBag.Header = "Level " + severityID + " Severity Collision Records";
            }
            else if (city != null)
            {
                ViewBag.Header = city + " City Collision Records";
            }

            var x = new CollisionsViewModel
            {

                Utah_Crash_Data_2020 = repo.Utah_Crash_Data_2020
                .Where(x => x.CRASH_SEVERITY_ID.ToString() == severityID || severityID == null)
                .Where(x => x.CITY == city || city == null)
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

        [HttpGet]
        public IActionResult LoginTest()
        {
            return View();
        }

     





        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // this controller will only let you move forward if you successfully log in, then route to the next page
        [HttpPost]
        public async Task<IActionResult> LoginTest(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            
            if(user != null)
            {
                // sign in 
                var signInResult = await signInManager.PasswordSignInAsync(user, password, false, false);
                
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }; 

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult RegisterTest()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> RegisterTest(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
            };

            var result = await userManager.CreateAsync(user, password); 

            if (result.Succeeded)
            {
                // sign in 
                var signInResult = await signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }

            }
            
            return RedirectToAction("Index");
        }

            ModelState.AddModelError("", "Invalid Name or Password");
            return View(loginmodel);    
        }


        [HttpGet]
        public IActionResult AddCollision()
        {


            return View("EditCollision");
        }

        [HttpPost]
        public IActionResult AddCollision(Collision c)
        {
            if (ModelState.IsValid)
            {
                return View("Confirmation", c);
            }
            else  //if invalid
            {

                return View(c);
            }
        }


        [HttpGet]
        public IActionResult EditCollision (string collisionid)
        {
            var crash = utahCollisions.Utah_Crash_Data_2020.Single(x => x.CRASH_ID == collisionid);


            return View(crash);
        }

        [HttpPost]
        public IActionResult EditCollision(Collision c)
        {
            if (ModelState.IsValid)
            { 
                return View("Confirmation", c);
            }
            else  //if invalid
            {

                return View(c);
            }
        }
        
        [HttpGet]
        public IActionResult DeleteCollision (string collisionid)
        {
            var crash = utahCollisions.Utah_Crash_Data_2020.Single(x => x.CRASH_ID == collisionid);


            return View("Confirmation", crash);
        }

        [HttpPost]
        public IActionResult Confirmation(Collision c)
        {
            utahCollisions.Update(c);
            utahCollisions.SaveChanges();

            return View("SummaryData");
        }

    public async Task<IActionResult> LogOut()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }




    //[HttpGet]
    //public IActionResult Login(string returnURL)
    //{
    //    //return View(new LoginModel { returnURL = returnURL });
    //    return View();
    //}
    //[HttpPost]
    //public async Task<IActionResult> Login(LoginModel loginmodel)
    //{
    //    if( ModelState.IsValid )
    //    {
    //        IdentityUser user = await userManager.FindByNameAsync(loginmodel.Username);

    //        if (user != null)
    //        {
    //            await signInManager.SignOutAsync();

    //            if((await signInManager.PasswordSignInAsync(user, loginmodel.Password, false, false)).Succeeded)
    //            {
    //                //return Redirect(loginmodel ? returnURL ?? "/Admin");
    //                return RedirectToAction("SummaryData");
    //            }
    //        }
    //    }

    //    ModelState.AddModelError("", "Invalid Name or Password");
    //    return View(loginmodel);

    //}

}
}
