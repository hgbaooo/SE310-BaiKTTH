using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SE310.P12_BaiKTTH_BE.Interfaces;
using SE310.P12_BaiKTTH_BE.Models;
using System.Collections.Concurrent;
using System.Linq;

namespace SE310.P12_BaiKTTH_BE.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<int, (string RefreshToken, DateTime ExpiryDate)> _refreshTokens;
        private readonly TimeSpan _refreshTokenExpiryTime = TimeSpan.FromDays(7);
        private static readonly byte[] FixedSalt = Encoding.UTF8.GetBytes("fixed_salt");

        public AuthRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _refreshTokens = new ConcurrentDictionary<int, (string, DateTime)>();
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash)
        {
            // Using a fixed salt for demonstration purposes (not secure for production)
            using (var hmac = new HMACSHA512(FixedSalt))
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPassword(string password, byte[] storedHash)
        {
            using (var hmac = new HMACSHA512(FixedSalt)) // Use the same fixed salt
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
        
        public string CreateToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var tokenSecret = _configuration["AppSettings:Token"];
            if (string.IsNullOrEmpty(tokenSecret))
            {
                throw new ArgumentNullException(nameof(tokenSecret), "Token secret must be provided in configuration.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(30), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

public string CreateRefreshToken()
{
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
}

public void SaveRefreshToken(int userId, string refreshToken)
{
    if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

    var expiryDate = DateTime.UtcNow.Add(_refreshTokenExpiryTime);
    _refreshTokens[userId] = (refreshToken, expiryDate);
}

public string GetRefreshToken(int userId)
{
    return _refreshTokens.TryGetValue(userId, out var tokenInfo) ? tokenInfo.RefreshToken : null;
}

public void UpdateRefreshToken(int userId, string newToken)
{
    if (string.IsNullOrEmpty(newToken)) throw new ArgumentNullException(nameof(newToken));

    var expiryDate = DateTime.UtcNow.Add(_refreshTokenExpiryTime);
    _refreshTokens[userId] = (newToken, expiryDate);
}

public void RevokeRefreshToken(int userId)
{
    _refreshTokens.TryRemove(userId, out _);
}

public string GetUserIdFromJwt(string token)
{
    if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));

    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

    return jwtToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
}

public void CleanupExpiredTokens()
{
    var expiredTokens = _refreshTokens
        .Where(kvp => kvp.Value.ExpiryDate < DateTime.UtcNow)
        .Select(kvp => kvp.Key)
        .ToList();

    foreach (var key in expiredTokens)
    {
        _refreshTokens.TryRemove(key, out _);
    }
}

        
    }
}
