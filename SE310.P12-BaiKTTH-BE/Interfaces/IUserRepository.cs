using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(string email);
        User GetUserByEmail(string email);
        void AddUser(User user);
        void Save();
        User GetUserById(int Id);
    }
}