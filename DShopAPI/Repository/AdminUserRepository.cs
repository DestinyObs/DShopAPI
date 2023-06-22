using System.Linq;
using DShopAPI.Data;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DShopAPI.Repositories
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly DShopDbContext _dbContext;

        public AdminUserRepository(DShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AdminUser GetAdminUserByEmail(string email)
        {
            return _dbContext.AdminUsers.SingleOrDefault(u => u.Email == email);
        }

        public AdminUser GetAdminUserById(int id)
        {
            return _dbContext.AdminUsers.Find(id);
        }

        public void AddAdminUser(AdminUser adminUser)
        {
            _dbContext.AdminUsers.Add(adminUser);
        }

        public void DeleteAdminUser(AdminUser adminUser)
        {
            _dbContext.AdminUsers.Remove(adminUser);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
        public AdminUser GetAdminUserByAdminId(string adminId)
        {
            return _dbContext.AdminUsers.SingleOrDefault(u => u.AdminId == adminId);
        }
        public AdminUser GetAdminUserByUsername(string username)
        {
            return _dbContext.AdminUsers.SingleOrDefault(u => u.UserName == username);
        }

    }
}
