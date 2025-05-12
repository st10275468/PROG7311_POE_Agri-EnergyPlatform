using Agri_Energy_Connect_Platform.Data;
using Agri_Energy_Connect_Platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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

        public IActionResult ViewFarmerProducts()
        {
            return View();
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
