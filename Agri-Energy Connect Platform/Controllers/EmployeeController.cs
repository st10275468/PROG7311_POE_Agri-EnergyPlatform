using Agri_Energy_Connect_Platform.Data;
using Agri_Energy_Connect_Platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Globalization;

namespace Agri_Energy_Connect_Platform.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult AddFarmer()
        {
            return View();
        }

        public IActionResult ViewAllFarmers()
        {
            return View();
        }

        /*public IActionResult ViewFarmerProducts()
        {
            var products = _context.Products.ToList();
            return View(products);
        }*/


        public IActionResult ViewFarmerProducts(string  filterType, string filterValue, DateTime? startDate, DateTime? endDate) {


            var productsQuery = _context.Products
                 .Include(p => p.Farmer) 
                 .AsQueryable();

        

            if (!string.IsNullOrEmpty(filterType) && !string.IsNullOrEmpty(filterValue))
            {
                if (filterType == "Category")
                {
                    productsQuery = productsQuery
                        .Where(p => p.category.ToLower().Contains(filterValue.ToLower()));
                }
                else if (filterType == "Farmer")
                {
                    productsQuery = productsQuery
                        .Where(p => (p.Farmer.name + " " + p.Farmer.surname).ToLower().Contains(filterValue.ToLower()));
                }
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                productsQuery = productsQuery
                    .Where(p => p.CreatedDate >= startDate && p.CreatedDate <= endDate);
            }
            var products = productsQuery.ToList();

            var categories = new List<string>
            {
            "Green Energy", "Organic", "Sustainable", "Local", "Fresh"
            };

            ViewBag.Categories = categories;


            return View(products);

        }


        [HttpPost]
        public IActionResult AddFarmer(User user)
        {
            if (ModelState.IsValid)
            {
                user.role = "Farmer";
                var passwordHasher = new PasswordHasher<User>();
                user.password = passwordHasher.HashPassword(user, user.password);

                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Farmer created successfully!";
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.Include(p => p.Farmer).FirstOrDefault(p => p.productID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

    }
}
