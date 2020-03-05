using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DannyPieShop.Models;
using DannyPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DannyPieShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPieReository pieReository;

        public HomeController(IPieReository pieReository)
        {
            this.pieReository = pieReository;
        }
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                PiesOfTheWeek=pieReository.PieOfTheWeek
            };
            return View(homeViewModel);
        }
    }
}