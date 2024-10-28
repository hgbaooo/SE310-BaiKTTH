using Microsoft.AspNetCore.Mvc;
using SE310_BKTTH.Models;

namespace SE310_BKTTH.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5017/api/v1/");
        }

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<ProductModel>>("Product");
            return View(products);
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<ProductModel>($"Product/{id}");
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                product.ImageUrl = string.IsNullOrWhiteSpace(product.ImageUrl) ? "product-1.png" : product.ImageUrl;

                var response = await _httpClient.PostAsJsonAsync("Product", product);
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("http://localhost:5056/Admin/Dashboard");
                }
                ModelState.AddModelError("", "Error creating product. Please try again.");
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<ProductModel>($"Product/{id}");
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"Product/{id}", product);
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("http://localhost:5056/Admin/Dashboard");
                }
                ModelState.AddModelError("", "Error updating product. Please try again.");
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                return Redirect("http://localhost:5056/Admin/Dashboard");
            }

            ModelState.AddModelError("", "Error deleting product. Please try again.");
            return RedirectToAction("Index");
        }



    }
}
