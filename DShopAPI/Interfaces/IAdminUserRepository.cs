using DShopAPI.Models;

namespace DShopAPI.Interfaces
{
    public interface IAdminUserRepository
    {
        AdminUser GetAdminUserByEmail(string email);
        AdminUser GetAdminUserById(int id);
        void AddAdminUser(AdminUser adminUser);
        void DeleteAdminUser(AdminUser adminUser);
        void SaveChanges();
        AdminUser GetAdminUserByAdminId(string adminId);
        AdminUser GetAdminUserByUsername(string username);

    }
}
