using SportsStore.Domain.Entities;
using SportsStore.Domain.IProductRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;
        public AdminController(IProductRepository productRepository)
        {
            repository = productRepository; 
        }
        public ViewResult Index()
        {
            return View(repository.Products);
        }
        public ViewResult Edit(int productId)
        {
            Product product = repository.Products.FirstOrDefault(s => s.ProductID == productId);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} - has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // 데이터 값이 문제가 있는 경우
                return View(product);
            }

        }
        public ViewResult Create()
        {
            return View("Edit",new Product());
        }

        // 원래는 HttpGet 방식으로 하면 안됨. 내가 페이지에서 귀찮아서 폼 구현을 안함        
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if(deletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedProduct.Name);
            }
            return RedirectToAction("Index");

        }
    }
}
