using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtahCollisions.Models;

namespace UtahCollisions.Components
{
    public class AdminSeverityViewComponent : ViewComponent
    {
        // get data for component here 
        //private iUtahCollisionRepository repo { get; set; }

        ////Contructor
        //public AdminSeverityViewComponent(iUtahCollisionRepository temp)
        //{
        //    repo = temp;
        //}

        private iUtahCollisionRepository repo;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private UtahCollisionsContext utahCollisions;
        public AdminSeverityViewComponent(iUtahCollisionRepository temp, UserManager<IdentityUser> um, SignInManager<IdentityUser> sim, UtahCollisionsContext UCC)
        {
            repo = temp;
            userManager = um;
            signInManager = sim;
            utahCollisions = UCC;
        }


        // invoke method 
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedSeverity = RouteData.Values["adminseverityID"];

            // get category types 
            var adminSeverity = repo.Utah_Crash_Data_2020
                .Select(x => x.CRASH_SEVERITY_ID.ToString())
                .Distinct()
                .OrderBy(x => x);


            return View(adminSeverity);
        }
    }
}

