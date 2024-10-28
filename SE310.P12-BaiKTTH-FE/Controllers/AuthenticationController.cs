using Microsoft.AspNetCore.Mvc;
using SE310_BKTTH.Models;

namespace ToeicStudyNetwork.Controllers;

public class AuthenticationController : Controller
{
    private readonly HttpClient _httpClient;

    public AuthenticationController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> LoginUser(string email, string password)
    {
        // Prepare login data from form input
        var loginData = new
        {
            Username = email,
            Password = password
        };

        // Call the authentication API
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5017/api/v1/Auth/login", loginData);

        if (response.IsSuccessStatusCode)
        {
            var tokens = await response.Content.ReadFromJsonAsync<TokenResponse>();
        

            return RedirectToAction("Index", "Home");
        }

        // Handle login failure
        ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
        return Redirect($"/Home/Index");
    }



    public IActionResult SignIn()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterUser(string username, string email, string password, string confirmPassword)
    {
        // Validate passwords match
        if (password != confirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Passwords do not match.");
            return View("SignUp");
        }

        // Prepare the registration data from form input
        var registerData = new
        {
            Username = username,
            Password = password,
            Email = email,
            Role = "User"  // Optional: Customize role as needed
        };

        // Send registration request to the API
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5017/api/v1/Auth/register", registerData);

        if (response.IsSuccessStatusCode)
        {
            // Redirect to login page on successful registration
            return RedirectToAction("SignIn");
        }
    
        // Handle registration failure
        ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
        return View("SignUp");  // Return to the SignUp view with error messages
    }

    public IActionResult SignUp()
    {
        return View();
    }
}
