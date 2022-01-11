using SportsStore.Domain.Entities;
using SportsStore.Domain.IProductRepo;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private IProductRepository repository;
        //private IOrderProcessor orderProcessor;
        public CartController(IProductRepository repository)
        {
            this.repository = repository;
            //this.orderProcessor = order;
        }
        public ViewResult Index(Cart cart , string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        public RedirectToRouteResult AddToCart(Cart cart,int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(s => s.ProductID == productId);
            if(product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index",new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart ,int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(s => s.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public ViewResult Checkout()
        {
            return View(new ShoppringDetails());
        }
        
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShoppringDetails shoppringDetails)
        {
            /*
            // 구매 프로세스 - IOrder 프로세스 구현 안함
            if(cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shoppringDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shoppringDetails);
            }
            */
            return View(shoppringDetails);
        }

    }
    }
