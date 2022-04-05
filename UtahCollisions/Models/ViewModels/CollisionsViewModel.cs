using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtahCollisions.Models.ViewModels
{
    public class CollisionsViewModel
    {
        public IQueryable<Collision> Utah_Crash_Data_2020 { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
