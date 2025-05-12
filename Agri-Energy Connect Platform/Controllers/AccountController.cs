using Agri_Energy_Connect_Platform.Data;
using Agri_Energy_Connect_Platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agri_Energy_Connect_Platform.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        /*
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


      */
      /*  [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.password = _passwordHasher.HashPassword(user, user.password);
                _context.Users.Add(user);
                Console.WriteLine($"User to be saved: {user.name}, {user.email}");
                _context.SaveChanges();
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return RedirectToAction("Index", "Home");

        }
      */
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == email);
            if (user == null) {

                ModelState.AddModelError("", "Invalid login attempt.");
                return RedirectToAction("Index", "Home");
            }

            if (user.password == password)
            {
                HttpContext.Session.SetString("UserID", user.userID.ToString());
                HttpContext.Session.SetString("UserRole", user.role);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
