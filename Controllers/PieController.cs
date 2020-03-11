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


        //public IActionResult List()
        //{
        //    PiesListViewModel piesListviewModels = new PiesListViewModel
        //    {
        //        Pies = _pieRepository.AllPies,
        //        CurrentCategory = "Cheese Cakes"
        //    };
        //    return View(piesListviewModels);
        //}

        public ViewResult List(string category)
        {
            IEnumerable<Pie> pies;
            string currentCategory;

            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
            }
            else
            {
                pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category)
                    .OrderBy(p => p.PieId);
                currentCategory = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryName == category)?.CategoryName;
            }

            return View(new PiesListViewModel
            {
                Pies = pies,
                CurrentCategory = currentCategory
            });
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