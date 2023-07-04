using System;

namespace DShopAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
