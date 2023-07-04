//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using DShopAPI.Interfaces;
//using DShopAPI.Models;
//using DShopAPI.Repositories;
//using Microsoft.AspNetCore.Mvc;

//namespace DShopAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ReviewController : ControllerBase
//    {
//        private readonly IReviewRepository<Review> _reviewRepository;

//        public ReviewController(IReviewRepository<Review> reviewRepository)
//        {
//            _reviewRepository = reviewRepository;
//        }

//        [HttpGet]
//        public IActionResult GetReviews()
//        {
//            var reviews = _reviewRepository.GetAll();
//            return Ok(reviews);
//        }

//        [HttpGet("{id}")]
//        public IActionResult GetReviewById(int id)
//        {
//            var review = _reviewRepository.GetById(id);
//            if (review == null)
//            {
//                return NotFound();
//            }
//            return Ok(review);
//        }

//        [HttpPost]
//        public IActionResult CreateReview(Review review)
//        {
//            _reviewRepository.Add(review);
//            _reviewRepository.SaveChanges();
//            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
//        }

//        [HttpPut("{id}")]
//        public IActionResult UpdateReview(int id, Review review)
//        {
//            var existingReview = _reviewRepository.GetById(id);
//            if (existingReview == null)
//            {
//                return NotFound();
//            }

//            existingReview.Title = review.Title;
//            existingReview.Description = review.Description;
//            existingReview.Rating = review.Rating;

//            _reviewRepository.Update(existingReview);
//            _reviewRepository.SaveChanges();

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public IActionResult DeleteReview(int id)
//        {
//            var review = _reviewRepository.GetById(id);
//            if (review == null)
//            {
//                return NotFound();
//            }

//            _reviewRepository.Delete(review);
//            _reviewRepository.SaveChanges();

//            return NoContent();
//        }
//    }
//}
