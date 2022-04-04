using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtahCollisions.Models
{
    public interface iUtahCollisionRepository
    {
        IQueryable<Collision> UtahCrashData { get; }  // <model name> and table name next to it 
    }
}
