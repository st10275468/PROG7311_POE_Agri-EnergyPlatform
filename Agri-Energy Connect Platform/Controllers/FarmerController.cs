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

        /// <summary>
        /// Method that displays all then products specific to the farmer that is logged in
        /// </summary>
        /// <returns></returns>
        public IActionResult ViewProducts()
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                TempData["LoginMessage"] = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }

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

        /// <summary>
        /// Method that displays more details and expands the product
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

            var product = _context.Products.FirstOrDefault(p => p.productID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        /// <summary>
        /// Method that opens the add product page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddProduct()
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                TempData["LoginMessage"] = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }

            var farmerID = HttpContext.Session.GetString("UserID");


            if (string.IsNullOrEmpty(farmerID)){
                return RedirectToAction("Login", "Account");
            }




            return View();
        }

        /// <summary>
        /// Method that handles the submission when the farmer is adding a product to the database
        /// </summary>
        /// <param name="product"></param>
        /// <param name="productImage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile? productImage)
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                TempData["LoginMessage"] = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }
            //Retreiving the userID to use as a foreign key
            var farmerID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(farmerID) )
            {
                return RedirectToAction("Login", "Account");
            }

            var farmer = await _context.Users.FindAsync(int.Parse(farmerID));
          
            if (farmer == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                product.userID = int.Parse(farmerID);
               
                //Handeling the file or image path being stored into the database
                if (productImage != null && productImage.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await productImage.CopyToAsync(stream);
                    }
                    
                    product.ImageUrl = "/images/products/" + fileName;

                }
                //Adding a new product and saving changes to the database
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                //prompting the user that they have added a product
                ModelState.Clear();
                ViewBag.SuccessMessage = "The product was added successfully!";
              
                return View(new Product());

            }

            return View(product);

        }
        
    }
}
/*OpenAI.2024. Chat-GPT(Version 3.5).[Large language model]. Available at: https://chat.openai.com/[Accessed: 13 May 2025]. */