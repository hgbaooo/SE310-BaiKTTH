using Microsoft.AspNetCore.Mvc;
using SE310_BKTTH.Models;

namespace SE310_BKTTH.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5017/api/v1/");
        }

        public async Task<IActionResult> Dashboard()
        {
            var products = await _httpClient.GetFromJsonAsync<List<ProductModel>>("Product");

            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return View(products);
        }
        
        public async Task<IActionResult> Products()
        {
            var products = await _httpClient.GetFromJsonAsync<List<ProductModel>>("Product");
            return View(products);
        }
    }
}
