using Microsoft.AspNetCore.Mvc;
using SE310.P12_BaiKTTH_BE.Dto;
using SE310.P12_BaiKTTH_BE.Interfaces;
using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;

        public AuthController(IUserRepository userRepository, IAuthRepository authRepository)
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            // Check if the email already exists
            if (_userRepository.UserExists(registerDto.Email))
            {
                return BadRequest("Email already exists");
            }

            // Create password hash (no salt needed)
            _authRepository.CreatePasswordHash(registerDto.PassWord, out byte[] passwordHash);

            // Create user instance
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                PassWord = Convert.ToBase64String(passwordHash),
                Role = registerDto.Role
            };

            _userRepository.AddUser(user);
            _userRepository.Save();

            // Return the created user DTO
            return Ok(new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto login)
        {
            // Retrieve user by email
            var user = _userRepository.GetUserByEmail(login.Email);

            // Validate user and password
            if (user == null || !_authRepository.VerifyPassword(login.PassWord, Convert.FromBase64String(user.PassWord)))
            {
                return Unauthorized("Invalid Email or Password");
            }

            // Create tokens
            string token = _authRepository.CreateToken(user);
            string refreshToken = _authRepository.CreateRefreshToken();

            // Save the refresh token for the user
            _authRepository.SaveRefreshToken(user.Id, refreshToken);

            return Ok(new { token, refreshToken, role = user.Role });
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(string token, string refreshToken)
        {
            // Validate input tokens
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("Token and refresh token are required");
            }

            // Get userId from the JWT token
            var userIdStr = _authRepository.GetUserIdFromJwt(token);
            if (!int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized("Invalid token");
            }

            // Retrieve user by ID
            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return Unauthorized("Invalid token");
            }

            // Retrieve the stored refresh token for validation
            var storedRefreshToken = _authRepository.GetRefreshToken(userId);
            if (storedRefreshToken == null || storedRefreshToken != refreshToken)
            {
                return Unauthorized("Invalid refresh token");
            }

            // Generate new tokens
            string newToken = _authRepository.CreateToken(user);
            string newRefreshToken = _authRepository.CreateRefreshToken();

            // Update the stored refresh token
            _authRepository.UpdateRefreshToken(userId, newRefreshToken);

            return Ok(new { token = newToken, refreshToken = newRefreshToken });
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken(string refreshToken)
        {
            // Validate refresh token
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("Refresh token is required");
            }

            // Get userId from the refresh token
            var userIdStr = _authRepository.GetUserIdFromJwt(refreshToken);
            if (!int.TryParse(userIdStr, out int userId))
            {
                return NotFound("Refresh token not found");
            }

            // Revoke the refresh token
            _authRepository.RevokeRefreshToken(userId);

            return Ok("Refresh token revoked");
        }
    }
}
