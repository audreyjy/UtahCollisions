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
    public class AdminCityViewComponent : ViewComponent
    {

        // get data for component here 
        //private iUtahCollisionRepository repo { get; set; }

        ////Contructor
        //public AdminCityViewComponent(iUtahCollisionRepository temp)
        //{
        //    repo = temp;
        //}

        // get data for component here 
        private iUtahCollisionRepository repo;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private UtahCollisionsContext utahCollisions;
        public AdminCityViewComponent(iUtahCollisionRepository temp, UserManager<IdentityUser> um, SignInManager<IdentityUser> sim, UtahCollisionsContext UCC)
        {
            repo = temp;
            userManager = um;
            signInManager = sim;
            utahCollisions = UCC;
        }


        // invoke method 
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCity = RouteData.Values["admincity"];

            // get category types 
            var admincity = repo.Utah_Crash_Data_2020
                .Select(x => x.CITY)
                .Distinct()
                .OrderBy(x => x);


            return View(admincity);
        }


    }
}
