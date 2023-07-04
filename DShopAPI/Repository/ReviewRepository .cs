//using System;
//using System.Collections.Generic;
//using System.Linq;
//using DShopAPI.Data;
//using DShopAPI.Interfaces;
//using DShopAPI.Models;

//namespace DShopAPI.Repositories
//{
//    public class ReviewRepository : IReviewRepository<Review>
//    {
//        private readonly DShopDbContext _context;

//        public ReviewRepository(DShopDbContext context)
//        {
//            _context = context;
//        }

//        public IEnumerable<Review> GetAll()
//        {
//            return _context.Reviews.ToList();
//        }

//        public Review GetById(int id)
//        {
//            return _context.Reviews.FirstOrDefault(r => r.Id == id);
//        }

//        public void Add(Review entity)
//        {
//            _context.Reviews.Add(entity);
//        }

//        public void Update(Review entity)
//        {
//            _context.Reviews.Update(entity);
//        }

//        public void Delete(Review entity)
//        {
//            _context.Reviews.Remove(entity);
//        }

//        public void SaveChanges()
//        {
//            _context.SaveChanges();
//        }
//    }
//}
