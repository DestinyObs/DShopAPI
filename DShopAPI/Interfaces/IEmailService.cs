using System.Threading.Tasks;

namespace DShopAPI.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string subject, string body);
    }
}
