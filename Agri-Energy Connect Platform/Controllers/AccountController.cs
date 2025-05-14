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

        /// <summary>
        /// Displays the login page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Method that handles the submission of the login form
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            //Finding user with that email
            var user = _context.Users.FirstOrDefault(u => u.email == email);
            
            //Error checking 
            if (user == null) {

                TempData["LoginError"] = "Invalid email or password.";
                return RedirectToAction("Index", "Home");
            }

            
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.password, password);

            //Checking if the passwords match
            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                HttpContext.Session.SetString("UserID", user.userID.ToString());
                HttpContext.Session.SetString("UserRole", user.role);
                return RedirectToAction("Index", "Home");
            }

            //Returning an error if they do not match
            TempData["LoginError"] = "Invalid email or password.";
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Method that logs the current user out of the website
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");


        }

    }
}
/*OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/[Accessed: 13 May 2025]. */