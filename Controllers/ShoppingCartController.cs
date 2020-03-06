using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DannyPieShop.Models;
using DannyPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DannyPieShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPieReository pieRepository;
        private readonly ShoppingCart shoppingCart;

        public ShoppingCartController(IPieReository pieRepository, ShoppingCart shoppingCart)
        {
            this.pieRepository = pieRepository;
            this.shoppingCart = shoppingCart;
        }
        // GET: /<controller>/
        public ViewResult Index()
        {
            var item = shoppingCart.GetShoppingCartItems();
            shoppingCart.ShoppingCartItems = item;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = shoppingCart.GetShoppingCartTotal()
            };
            return View(shoppingCartViewModel);
        }


        public RedirectToActionResult AddToShoppingCart(int pieId)
        {
            var selectedPie = pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);

            if (selectedPie != null)
            {
                shoppingCart.AddToCart(selectedPie, 1);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int pieId)
        {
            var selectedPie = pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);

            if (selectedPie != null)
            {
                shoppingCart.RemoveFromCart(selectedPie);
            }
            return RedirectToAction("Index");
        }
    }
}