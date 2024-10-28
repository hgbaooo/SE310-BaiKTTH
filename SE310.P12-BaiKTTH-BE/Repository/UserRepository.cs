using SE310.P12_BaiKTTH_BE.Data;
using SE310.P12_BaiKTTH_BE.Interfaces;
using SE310.P12_BaiKTTH_BE.Models;

namespace SE310.P12_BaiKTTH_BE.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool UserExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int Id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == Id);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}