using System.Linq;
using System.Threading.Tasks;
using DShopAPI.Data;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DShopAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DShopDbContext _dbContext;

        public UserRepository(DShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UserExists(string email, string userName)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email || u.UserName == userName);
        }

        public async Task AddUser(Users user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public Users GetUserByEmailAndPassword(string email, string password)
        {
            return _dbContext.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        public Users GetUserByEmailAndConfirmationCode(string email, string confirmationCode)
        {
            return _dbContext.Users.SingleOrDefault(u => u.Email == email && u.ConfirmationCode == confirmationCode);
        }

        public void UpdateUser(Users user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }
        public async Task<Users> GetUserById(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }
    }
}
