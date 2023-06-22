using System.Threading.Tasks;
using DShopAPI.Models;

namespace DShopAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string email, string userName);
        Task AddUser(Users user);
        Users GetUserByEmailAndPassword(string email, string password);
        Users GetUserByEmailAndConfirmationCode(string email, string confirmationCode);
        void UpdateUser(Users user);
    }
}
