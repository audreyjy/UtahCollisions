using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtahCollisions.Models;

namespace UtahCollisions.Components
{
    public class SeverityViewComponent : ViewComponent
    {
        // get data for component here 
        private iUtahCollisionRepository repo { get; set; }

        //Contructor
        public SeverityViewComponent(iUtahCollisionRepository temp)
        {
            repo = temp;
        }


        // invoke method 
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedSeverity = RouteData.Values["severityID"];

            // get category types 
            var severity = repo.Utah_Crash_Data_2020
                .Select(x => x.CRASH_SEVERITY_ID.ToString())
                .Distinct()
                .OrderBy(x => x); 
                

            return View(severity);
        }
    }
}
