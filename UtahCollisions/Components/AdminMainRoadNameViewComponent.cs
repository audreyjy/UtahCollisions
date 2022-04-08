using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtahCollisions.Models;

namespace UtahCollisions.Components
{
    public class AdminMainRoadNameViewComponent : ViewComponent
    {
        //get data for component here
        private iUtahCollisionRepository repo { get; set; }

        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private UtahCollisionsContext utahCollisions;
        public AdminMainRoadNameViewComponent(iUtahCollisionRepository temp, UserManager<IdentityUser> um, SignInManager<IdentityUser> sim, UtahCollisionsContext UCC)
        {
            repo = temp;
            userManager = um;
            signInManager = sim;
            utahCollisions = UCC;
        }

       
       


        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedRoute = RouteData.Values["adminRoute"];

            // get category types 
            var adminRoute = repo.Utah_Crash_Data_2020
                .Select(x => x.ROUTE)
                .Distinct()
                .OrderBy(x => x);


            return View(adminRoute);
        }
    }
}
