using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context) 
        {
            _context = context;
        }

        public bool CreateReview( Review review)
        {
            
            _context.Add(review);

            return save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return save();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Review> GetReviewOfAPokemon(int pokiId)
        {
            return _context.Reviews.Where(x => x.Pokemon.Id == pokiId).ToList();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public bool ReviewExits(int id)
        {
            return _context.Reviews.Any(x => x.Id == id);
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return save();
        }
    }
}
