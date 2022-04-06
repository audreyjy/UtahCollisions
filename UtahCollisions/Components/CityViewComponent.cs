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
        // get data for component here 
        private iUtahCollisionRepository repo { get; set; }

        //Contructor
        public CityViewComponent(iUtahCollisionRepository temp)
        {
            repo = temp;
        }


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
