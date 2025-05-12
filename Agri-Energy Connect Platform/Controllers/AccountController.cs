using Agri_Energy_Connect_Platform.Data;
using Agri_Energy_Connect_Platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agri_Energy_Connect_Platform.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }


       
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == email);
            
            if (user == null) {

                ModelState.AddModelError("", "Invalid login attempt.");
                return RedirectToAction("Index", "Home");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.password, password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
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
