using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ProductsAndCategories.Models;
// Other using statements
namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _context = context;
        }

         // show all categories
        [HttpGet("")]
        public IActionResult Index()
        {   
            // gets all categories and puts them in a list 
            ViewBag.allCategories = _context.Categories.ToList();

            return View();
        }

        // show all products
        [HttpGet("AllProducts")]
        public IActionResult AllProducts()
            {
                ViewBag.allProducts = _context.Products.ToList();
                return View();
            }

        // Go to specific cat clicked on
        [HttpGet("category/{categoryId}")]
        public IActionResult OneCategory(int categoryId )
            {
                // get the specific category we selected in the DB by using its ID
                Category RetrievedCategory = _context.Categories
                    .SingleOrDefault(cat => cat.CategoryId == categoryId);
                ViewBag.OneCategory = RetrievedCategory;

                // give me all the products in this category
                ViewBag.thisCatsProducts = _context.Products
                    .Include(p => p.ProductsCategories)
                    .Where(p => p.ProductsCategories.Any(p => p.CategoryId == categoryId))
                    .ToList();
                   
                // gets the products this category this category dosnet have yet
                // Go in Products
                ViewBag.productsUnrelated = _context.Products
                    // include a list of the productsCategories that links the middle table
                    .Include(p => p.ProductsCategories)
                    // where the productCategories has a cat that == the catId passed in above
                    .Where(p => p.ProductsCategories.All(p => p.CategoryId != categoryId));
                return View();
            }
        
        // Go to specific product clicked on
        [HttpGet("product/{productId}")]
        public IActionResult OneProduct(int productId)
            {
                Product RetrievedProduct = _context.Products
                    .SingleOrDefault(product => product.ProductId == productId);
                ViewBag.OneProduct = RetrievedProduct;
                return View();
            }

        // Add category
        [HttpPost("CategoryAdd")]
        public IActionResult CategoryAdd(Category newCategory)
            {
                _context.Add(newCategory);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        
        // Add product
        [HttpPost("ProductAdd")]
        public IActionResult ProductAdd(Product newProduct)
            {
                _context.Add(newProduct);
                _context.SaveChanges();
                return RedirectToAction("AllProducts");
            }
        
        // Adding a product to our category
        [HttpPost("category/{categoryId}")]
        public IActionResult AddProductToCategory(Association newProductInCat)
            {
                _context.Associations.Add(newProductInCat);
                _context.SaveChanges();
                return RedirectToAction("OneCategory");
            }

    }
}
