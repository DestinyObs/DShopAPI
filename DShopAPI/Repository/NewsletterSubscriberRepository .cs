using DShopAPI.Data;
using DShopAPI.Interfaces;
using DShopAPI.Models;

namespace DShopAPI.Repository
{
    public class NewsletterSubscriberRepository : INewsletterSubscriberRepository
    {
        private readonly DShopDbContext _context;

        public NewsletterSubscriberRepository(DShopDbContext context)
        {
            _context = context;
        }

        public void AddSubscriber(NewsletterSubscriber subscriber)
        {
            _context.NewsletterSubscribers.Add(subscriber);
        }

        public NewsletterSubscriber GetSubscriberByEmail(string email)
        {
            return _context.NewsletterSubscribers.FirstOrDefault(s => s.Email == email);
        }

        public List<NewsletterSubscriber> GetAllSubscribers()
        {
            return _context.NewsletterSubscribers.ToList();
        }

        public void DeleteSubscriber(NewsletterSubscriber subscriber)
        {
            _context.NewsletterSubscribers.Remove(subscriber);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
