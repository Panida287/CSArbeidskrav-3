using EventPlatform.Models;

namespace EventPlatform.Interfaces;

/// <summary>
/// Contract for entities that support user reviews.
/// </summary>
public interface IReviewable
{
    /// <summary>Returns the average star rating (1–5).</summary>
    double GetAverageRating();

    /// <summary>Returns all reviews for this entity.</summary>
    List<Review> GetReviews();
}
