using SportsStore.Domain.IProductRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;
        public NavController(IProductRepository repository)
        {
            this.repository = repository;
        }
        public PartialViewResult Menu(string category= null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Products.Select(s => s.Category).Distinct().OrderBy(x => x);
            return PartialView(categories);
        }
    }
}