using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DannyPieShop.Models
{
    public class PieRepository : IPieReository
    {
        //data context field to access all datbase related things
        private readonly AppDbContext appDbContext;

        //constructor and here we are creating object on aa db context using dependency injection
        public PieRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        //Getting all the list detail of pies
        public IEnumerable<Pie> AllPies
        {
            get
            {
                return appDbContext.Pies.Include(c => c.Category);
            }
        }

        //getting the pies which are pie of the week detail
        public IEnumerable<Pie> PieOfTheWeek
        {
            get
            {
                return appDbContext.Pies.Include(c => c.Category).Where(p=>p.IsPieOfTheWeek);
            }
        }

        //getting particular pie on the pie id
        public Pie GetPieById(int pieId)
        {
            return appDbContext.Pies.FirstOrDefault(p => p.PieId == pieId);
        }
    }
}
