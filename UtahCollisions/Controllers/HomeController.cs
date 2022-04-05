using GoogleAuthenticatorService.Core;
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
        public HomeController(iUtahCollisionRepository temp, UserManager<IdentityUser> um, SignInManager<IdentityUser> sim)
        {
            repo = temp;
            userManager = um;
            signInManager = sim;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SummaryData(string severityID, int pageNum = 1)
        {
            int pageSize = 100;

            if (severityID is null)
            {
                ViewBag.Header = "All Records";
            }
            else
            {
                ViewBag.Header = "Level " + severityID + " Severity";
            }

            var x = new CollisionsViewModel
            {

                Utah_Crash_Data_2020 = repo.Utah_Crash_Data_2020
                .Where(x => x.CRASH_SEVERITY_ID.ToString() == severityID)
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

        //TESTING MY MFA ACTION////////
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginmodel)
        {
            if (loginmodel.Username == "Admin" && loginmodel.Password == "Admin123")
            {
                Session tempid = loginmodel.Username;
                return RedirectToAction("VerifyAuth");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Username or Password");
                return View();
            }
        }

        [HttpGet]
        public IActionResult VerifyAuth()
        {
            if (Session.tempid!=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }

        [HttpPost]
        public IActionResult VerifyAuth(LoginModel loginmodel)
        {
            var token = loginmodel.passcode;
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator()
        }

    }
}
