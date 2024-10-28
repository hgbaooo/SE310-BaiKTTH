using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SE310_BKTTH.Data;
using SE310_BKTTH.Models;

namespace SE310_BKTTH.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;
    
    public HomeController(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5017/api/v1/");
    }

    public async Task<IActionResult> Index()
    {
        var products = await _httpClient.GetFromJsonAsync<List<ProductModel>>("Product");
        return View(products);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
