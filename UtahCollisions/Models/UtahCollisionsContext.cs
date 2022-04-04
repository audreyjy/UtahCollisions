using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtahCollisions.Models
{
    public class UtahCollisionsContext : DbContext 
    {
        public UtahCollisionsContext()
        {
        }
            

        public UtahCollisionsContext(DbContextOptions<UtahCollisionsContext> options) : base(options)
        {

        }

        public DbSet<Collision> Utah_Crash_Data_2020 { get; set; }
        
    }
}
