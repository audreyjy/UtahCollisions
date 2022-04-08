using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UtahCollisions.Models;
using UtahCollisions.Models.ViewModels;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;


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
        public IActionResult SummaryData(string adminroute, string adminmainroad, string admincity, string adminseverityID, int pageNum = 1)
        {
            int pageSize = 100;

            if (adminseverityID is null && admincity is null && adminmainroad is null && adminroute is null)
            {
                ViewBag.Header = "All Records";
            }
            else if (admincity is null && adminmainroad is null && adminroute is null && adminseverityID != null)
            {
                ViewBag.Header = "Level " + adminseverityID + " Severity Collision Records";
            }
            else if (admincity is null && adminmainroad is null && adminseverityID is null && adminroute != null)
            {
                ViewBag.Header = "Records for Route " + adminroute;
            }
            else if (adminmainroad is null && adminseverityID is null && adminroute is null && admincity != null)
            {
                ViewBag.Header = admincity + " City Collision Records";
            }
            else if (adminseverityID is null && adminroute is null && admincity is null && adminmainroad != null)
            {
                ViewBag.Header = "Records for Main Road " + adminmainroad; 
            }

            var x = new CollisionsViewModel
            {

                Utah_Crash_Data_2020 = repo.Utah_Crash_Data_2020
                .OrderBy(x => x.CRASH_DATETIME)
                .Where(x => x.CRASH_SEVERITY_ID.ToString() == adminseverityID || adminseverityID == null)
                .Where(x => x.CITY == admincity || admincity == null)
                .Where(x => x.MAIN_ROAD_NAME == adminmainroad || adminmainroad == null)
                .Where(x => x.ROUTE == adminroute || adminroute == null)
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
        public IActionResult SummaryDataNonAuth(string mainRoad, string city, string severityID, int pageNum = 1)
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
                .Where(x => x.MAIN_ROAD_NAME == mainRoad || mainRoad == null)
                .OrderBy(x => x.CRASH_DATETIME)
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

        // GET PRIVACY POLICY VIEW 
        public IActionResult Privacy()
        {
            return View();
        }

        // GET LOGIN

        [HttpGet]
        public IActionResult LoginTest()
        {
            return View();
        }


        // POST Login
        [HttpPost]
        public async Task<IActionResult> LoginTest(string username, string password)
        {


            var user = await userManager.FindByNameAsync(username);
            

            if (user != null)
            {
                // sign in 

                var signInResult = await signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    //adding 2FA

                    return RedirectToAction("VerifyAuth");
                    //


                    // return RedirectToAction("SummaryData");
                }
            };



            return RedirectToAction("LoginTest");
        }

        //adding 2FA 
        [HttpGet]
        public IActionResult VerifyAuth()
        {
            //Information to generate QR code: 
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string useruniquekey = Key;
            var user = User.ToString();
            var setupinfo = tfa.GenerateSetupCode("GoogleAuthentication Utah Collision", user, useruniquekey, false, 20);
            ViewBag.qrcode = setupinfo.QrCodeSetupImageUrl;

            return View(); 
        }

        string Key = "test12345"; 


        [HttpPost]
        public IActionResult VerifyAuth(string passcode)
        {

            var token = passcode; 
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
              
            string useruniquekey = Key;
     
            bool isvalid = tfa.ValidateTwoFactorPIN(useruniquekey, token);
            if (isvalid)
            {
                return RedirectToAction("SummaryData");
            }

            return RedirectToAction("LoginTest"); 
        }


        
        //public IActionResult AdminQR()
        //{
        //    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        //    string useruniquekey = Key;
        //    //Session["Useruniquekey"] = useruniquekey;
        //    var user = User.ToString();  
        //    var setupinfo = tfa.GenerateSetupCode("GoogleAuthentication test", user, useruniquekey, false, 20);
        //    ViewBag.qrcode = setupinfo.QrCodeSetupImageUrl;
            
        //    return View(); 
        //}
        //

        // GET Register

        [HttpGet]
        public IActionResult RegisterTest()
        {
            return View();
        }

        // POST Register

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
            
            return RedirectToAction("RegisterTest");
        }


        // ADD Collision ////////////////////////////////////////////////////////////////////

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
        // /////////////////////////////////////////////////////////////////////////////////



        // EDIT/ADD COLLISION VIEW PAGE //////////////////////////////////////////////////////////////////
        // HOW TO ROUTE to the two different confirmationpages ///////////////////////////////////////////
        [Authorize]
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
                utahCollisions.Update(c);
                utahCollisions.SaveChanges();

                return RedirectToAction("SummaryData");
            }
            else  //if invalid
            {

                return View(c);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////////
        // Delete Collision Confirmation ///////////////////////////////////////////////////////
        
        [HttpGet]
        public IActionResult DeleteCollision (string collisionid)
        {
            var crash = utahCollisions.Utah_Crash_Data_2020.Single(x => x.CRASH_ID == collisionid);


            return View("DeleteConfirmation", crash);
        }

        [HttpPost]
        public IActionResult DeleteCollision(Collision c)
        {
            utahCollisions.Utah_Crash_Data_2020.Remove(c);
            utahCollisions.SaveChanges();

            return RedirectToAction("SummaryData");
        }
        // //////////////////////////////////////////////////////////////////////////////////



        // CONFIRM ADD PAGE /////////////////////////////////////////////////
        [HttpPost]
        public IActionResult Confirmation(Collision c)
        {
            utahCollisions.Add(c);
            utahCollisions.SaveChanges();

            return RedirectToAction("SummaryData");
        }
        // /////////////////////////////////////////////////////////////////////////////////




        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }


        public IActionResult Map()
        {
            return View();
        }

    }
}
