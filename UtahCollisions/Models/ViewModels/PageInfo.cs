using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtahCollisions.Models.ViewModels
{
    public class PageInfo
    {
        public int TotalNumCollisions { get; set; } // keeps track of how many projects there are 
        public int CollisionsPerPage { get; set; }// keeps track of how many projects we're going to show on page
        public int CurrentPage { get; set; } // keeps track of what page I'm on 
        public int TotalPages => (int)Math.Ceiling((double)TotalNumCollisions / CollisionsPerPage); // determine how many pages we need. cast totalnumporjects as a double, then cast the calculation to double, then force it to integer
    }
}
