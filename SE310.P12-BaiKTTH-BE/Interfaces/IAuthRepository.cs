using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Interfaces
{
    public interface IAuthRepository
    {
        void CreatePasswordHash(string password, out byte[] passwordHash);
        bool VerifyPassword(string password, byte[] storedHash);
        string CreateToken(User user);
        string CreateRefreshToken();
        void SaveRefreshToken(int userId, string refreshToken);
        string GetRefreshToken(int userId);
        void UpdateRefreshToken(int userId, string newToken);
        void RevokeRefreshToken(int userId);
        string GetUserIdFromJwt(string token);
        void CleanupExpiredTokens();
    }
}