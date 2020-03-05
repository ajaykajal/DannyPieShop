using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DannyPieShop.Models
{
    public interface IPieReository
    {
        IEnumerable<Pie> AllPies { get; }
        IEnumerable<Pie> PieOfTheWeek { get; }
        Pie GetPieById(int pieId);
    }
}
