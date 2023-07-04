using DShopAPI.Models;

namespace DShopAPI.Interfaces
{
    public interface INewsletterSubscriberRepository
    {
        void AddSubscriber(NewsletterSubscriber subscriber);
        NewsletterSubscriber GetSubscriberByEmail(string email);
        List<NewsletterSubscriber> GetAllSubscribers();
        void DeleteSubscriber(NewsletterSubscriber subscriber);
        void SaveChanges();
    }
}
