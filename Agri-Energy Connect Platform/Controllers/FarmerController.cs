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

        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.productID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
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
        public async Task<IActionResult> AddProduct(Product product, IFormFile? productImage)
        {
            var farmerID = HttpContext.Session.GetString("UserID");


            if (string.IsNullOrEmpty(farmerID) )
            {
                return RedirectToAction("Login", "Account");
            }

            product.CreatedDate = DateTime.UtcNow;

            if (ModelState.IsValid)
            {
                product.userID = int.Parse(farmerID);
                

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

                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("ViewProducts");

            }
            return View(product);

        }
        
    }
}
