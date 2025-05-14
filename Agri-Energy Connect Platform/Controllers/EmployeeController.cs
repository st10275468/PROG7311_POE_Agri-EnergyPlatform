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


        public IActionResult ViewFarmerProducts(string filterType, string filterValue, string category, DateTime? startDate, DateTime? endDate)
        {
            var productsQuery = _context.Products.Include(p => p.Farmer).AsQueryable();

            var farmers = _context.Users
                .Where(u => u.role == "Farmer")
                .Select(f => new
                {
                    f.userID,
                    FullName = f.name + " " + f.surname
                })
                .ToList();

            ViewBag.Farmers = farmers;

            var categories = new List<string>
    {
        "Green Energy", "Organic", "Sustainable", "Local", "Fresh"
    };
            ViewBag.Categories = categories;

            if (filterType == "Farmer" && int.TryParse(filterValue, out int selectedFarmerID))
            {
                productsQuery = productsQuery.Where(p => p.userID == selectedFarmerID);

                // Optional secondary filters
                if (!string.IsNullOrWhiteSpace(category))
                {
                    productsQuery = productsQuery.Where(p => p.category.ToLower().Contains(category.ToLower()));
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    productsQuery = productsQuery.Where(p => p.CreatedDate >= startDate && p.CreatedDate <= endDate);
                }
            }
            else if (filterType == "Category" && !string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.category.ToLower().Contains(category.ToLower()));
            }
            else if (filterType == "DateRange" && startDate.HasValue && endDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CreatedDate >= startDate && p.CreatedDate <= endDate);
            }

            var products = productsQuery.ToList();

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
