using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewOfAPokemon(int pokiId);
        bool ReviewExits(int id);

        bool CreateReview(Review review);

        bool UpdateReview(Review review);
        bool DeleteReview(Review review);

        bool DeleteReviews(List<Review> reviews);

        bool save();


    }
}
