using EventPlatform.Models;

namespace EventPlatform.Repositories;

/// <summary>
/// All SQLite queries for the Reviews table.
/// </summary>
public class ReviewRepository
{
    /// <summary>Inserts a new review. Returns the new ID.</summary>
    public int Insert(Review review) => throw new NotImplementedException();

    /// <summary>Returns all reviews for the given event.</summary>
    public List<Review> GetByEvent(int eventId) => throw new NotImplementedException();

    /// <summary>Returns true if a review already exists for the given booking.</summary>
    public bool ExistsForBooking(int bookingId) => throw new NotImplementedException();
}
