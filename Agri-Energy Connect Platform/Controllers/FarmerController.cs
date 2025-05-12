using Agri_Energy_Connect_Platform.Data;
using Microsoft.AspNetCore.Mvc;
using Agri_Energy_Connect_Platform.Models;

namespace Agri_Energy_Connect_Platform.Controllers
{
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult ViewProducts()
        {
            var farmer = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(farmer) || !int.TryParse(farmer, out int farmerID))
            {
                return RedirectToAction("Login", "Account");
            }

            var products = _context.Products
                .Where(p => p.userID == farmerID)
                .ToList();

            ViewBag.HasProducts = products.Any();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var farmerID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(farmerID)){
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            var farmerID = HttpContext.Session.GetString("UserID");


            if (string.IsNullOrEmpty(farmerID) )
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                product.userID = int.Parse(farmerID);
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("ViewProducts");

            }
            return View(product);

        }
        
    }
}
