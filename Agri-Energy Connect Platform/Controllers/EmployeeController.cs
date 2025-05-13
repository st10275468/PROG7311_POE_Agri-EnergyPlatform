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


        public IActionResult ViewFarmerProducts(string  filterType, string filterValue) {

            var productsQuery = _context.Products
                .Join(_context.Users,
                p => p.userID,
                u => u.userID,
                (p, u) => new { Product = p, Farmer = u })
                .AsQueryable();


            if (!string.IsNullOrEmpty(filterType) && !string.IsNullOrEmpty(filterValue))
            {
                if (filterType == "Category")
                {
                    // Filter by category
                    productsQuery = productsQuery.Where(p => p.Product.category.ToLower().Contains(filterValue.ToLower()));
                }
                else if (filterType == "Farmer")
                {
                    // Filter by farmer (User) using name or surname
                    productsQuery = productsQuery.Where(p =>
                        (p.Farmer.name + " " + p.Farmer.surname).ToLower().Contains(filterValue.ToLower()));
                }
            }
            var products = productsQuery.Select(p => p.Product).ToList();

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
    }
}
