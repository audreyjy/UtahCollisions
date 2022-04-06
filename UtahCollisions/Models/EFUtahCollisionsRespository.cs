using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UtahCollisions.Models
{
    public class EFUtahCollisionsRespository : iUtahCollisionRepository
    {
        private UtahCollisionsContext context { get; set; }
        
        public EFUtahCollisionsRespository (UtahCollisionsContext temp)
        {
            context = temp; 
        }
        public IQueryable<Collision> Utah_Crash_Data_2020 => context.Utah_Crash_Data_2020; // need to connect to the DB for this to work 


        public void SaveCrash(Collision c)
        {
            context.SaveChanges();
        }

        public void CreateCrash(Collision c)
        {
            context.Add(c);
            context.SaveChanges();
        }

        public void DeleteCrash(Collision c)
        {
            context.Remove(c);
            context.SaveChanges();
        }

    }
}
