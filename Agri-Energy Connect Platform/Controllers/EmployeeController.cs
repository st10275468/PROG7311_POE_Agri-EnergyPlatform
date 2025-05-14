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

        //Displays the view if a valid employee is logged in
        public IActionResult AddFarmer()
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                TempData["LoginMessage"] = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// Displays the products to the employee. Allows the user to filter
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="filterValue"></param>
        /// <param name="category"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IActionResult ViewFarmerProducts(string filterType, string filterValue, string category, DateTime? startDate, DateTime? endDate)
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                TempData["LoginMessage"] = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }

            var productsQuery = _context.Products.Include(p => p.Farmer).AsQueryable();

            //Creating a list of farmers for the filter combo box
            var farmers = _context.Users
                .Where(u => u.role == "Farmer")
                .Select(f => new
                {
                    f.userID,
                    FullName = f.name + " " + f.surname
                })
                .ToList();

            ViewBag.Farmers = farmers;

            //List of categories for filter
            var categories = new List<string>
                {
                 "Solar Solutions", "Wind Energy Solutions", "Biogas Systems", "Irrigation Tools", "Organic Farming", "Smart Farming Devices", "Green Energy Services", "Educational Resources","Machinery"
                };
            ViewBag.Categories = categories;

            //Applying the filtering
            if (filterType == "Farmer" && int.TryParse(filterValue, out int selectedFarmerID))
            {
                productsQuery = productsQuery.Where(p => p.userID == selectedFarmerID);

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

        /// <summary>
        /// Method that allows employee to create a new farmer
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddFarmer(User user)
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                TempData["LoginMessage"] = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                //Setting the role of the user to Farmer
                user.role = "Farmer";
                var passwordHasher = new PasswordHasher<User>();
                user.password = passwordHasher.HashPassword(user, user.password);

                //Adding and saving changes to the database
                _context.Users.Add(user);
                _context.SaveChanges();

                ViewBag.SuccessMessage = "Farmer profile created successfully!";
                ModelState.Clear();
                return View();
            }

            return View(user);
        }

        /// <summary>
        /// Expands the selected product and displays all details on it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ProductDetails(int id)
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                TempData["LoginMessage"] = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }

            var product = _context.Products.Include(p => p.Farmer).FirstOrDefault(p => p.productID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

    }
}
/*OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/[Accessed: 13 May 2025]. */