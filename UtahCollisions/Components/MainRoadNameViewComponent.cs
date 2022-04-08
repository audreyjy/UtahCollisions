using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtahCollisions.Models;

namespace UtahCollisions.Components
{
    public class MainRoadNameViewComponent : ViewComponent
    {
        //get data for component here
        private iUtahCollisionRepository repo { get; set; }

        //Contructor
        public MainRoadNameViewComponent(iUtahCollisionRepository temp)
        {
            repo = temp;
        }


        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedRoad = RouteData.Values["mainRoad"];

            // get category types 
            var mainRoad = repo.Utah_Crash_Data_2020
                .Select(x => x.MAIN_ROAD_NAME)
                .Distinct()
                .OrderBy(x => x);


            return View(mainRoad);
        }




    }
}
