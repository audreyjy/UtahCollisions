using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtahCollisions.Models
{
    public class CollisionsDBContext : DbContext 
    {
        public CollisionsDBContext(DbContextOptions<CollisionsDBContext> options) : base(options)
        {

        }

        public DbSet<Collision> TableNameInDataBase { get; set; }
    }
}
