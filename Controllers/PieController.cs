using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DannyPieShop.Models;
using DannyPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DannyPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieReository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieReository pieReository, ICategoryRepository categoryRepository)
        {
            this._pieRepository = pieReository;
            this._categoryRepository = categoryRepository;
        }


        public IActionResult List()
        {
            PiesListViewModel piesListviewModels = new PiesListViewModel
            {
                Pies = _pieRepository.AllPies,
                CurrentCategory = "Cheese Cakes"
            };
            return View(piesListviewModels);
        }

        /// <summary>
        /// get particular pie cakes using if
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
                return NotFound();
            return View(pie);
        }
    }
}