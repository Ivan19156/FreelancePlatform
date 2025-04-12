using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using FreelancePlatform.Models;

namespace FreelancePlatform.Services
{
    public class UserService
    {
        private readonly FreelancePlatformDbContext _context;
        public UserService(FreelancePlatformDbContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool UserExists(User user)
        {
            return _context.Users.Any(u => u.Email == user.Email);
        }
        public bool RegisterUser(User user)
        {
            if (!UserExists(user))
            {
                user.Password = HashPassword(user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                return true;
            }
            return false;


        }
        public bool AuthenticateUser(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return false;
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (isPasswordValid)
            {
                return true;
            }
            else { return false; }

        }

        public bool UpdateUserProfile(User user)
        {
            var existingUser = _context.Users.SingleOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email
                && !string.IsNullOrEmpty(user.Name) && user.Name != existingUser.Name
                && !string.IsNullOrEmpty(user.Password) && user.Password != existingUser.Password)
            {
                existingUser.Email = user.Email;
                existingUser.Name = user.Name;
                existingUser.Password = HashPassword(user.Password);
                _context.SaveChanges();
                return true;
            }

            return false;

        }
        public bool AdjustBalance(int userid, decimal amount)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userid);

            if (user == null)
            {
                return false;
            }
            user.Balance += amount;
            _context.SaveChanges();
            return true;
        }

        public List<User> GetFreelancers()
        {
            // Отримуємо список всіх фрілансерів
            var freelancers = _context.Users.ToList();

            return freelancers;
        }
    }
}
