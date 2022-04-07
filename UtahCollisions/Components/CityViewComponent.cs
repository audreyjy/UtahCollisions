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
    public class CityViewComponent : ViewComponent
    {
        //get data for component here
        private iUtahCollisionRepository repo { get; set; }

        //Contructor
        public CityViewComponent(iUtahCollisionRepository temp)
        {
            repo = temp;
        }


        // get data for component here 
        //private iUtahCollisionRepository repo;
        //private SignInManager<IdentityUser> signInManager;
        //private UserManager<IdentityUser> userManager;
        //private UtahCollisionsContext utahCollisions;
        //public CityViewComponent(iUtahCollisionRepository temp, UserManager<IdentityUser> um, SignInManager<IdentityUser> sim, UtahCollisionsContext UCC)
        //public CityViewComponent(iUtahCollisionRepository temp)
        //{
        //repo = temp;
        //userManager = um;
        //signInManager = sim;
        //utahCollisions = UCC;
        //}


        // invoke method 
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCity = RouteData.Values["city"];

            // get category types 
            var city = repo.Utah_Crash_Data_2020
                .Select(x => x.CITY)
                .Distinct()
                .OrderBy(x => x);


            return View(city);
        }

        
    }
}
