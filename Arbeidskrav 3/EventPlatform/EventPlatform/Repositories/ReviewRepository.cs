using Microsoft.Data.Sqlite;
using EventPlatform.Models;

namespace EventPlatform.Repositories;

/// <summary>
/// All SQLite queries for the Reviews table.
/// </summary>
public class ReviewRepository
{
    /// <summary>Inserts a new review. Returns the new ID.</summary>
    public int Insert(Review review)
    {
        using var connection = new SqliteConnection("Data Source=eventplatform.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO REVIEWS (BookingId, EventId, UserId, Rating, Comment)
            VALUES (@bookingId, @eventId, @userId, @rating, @comment);

            SELECT last_insert_rowid();
";
        command.Parameters.AddWithValue("@bookingId", review.BookingId);
        command.Parameters.AddWithValue("@eventId", review.EventId);
        command.Parameters.AddWithValue("@userId", review.UserId);
        command.Parameters.AddWithValue("@rating", review.Rating);
        command.Parameters.AddWithValue("@comment", review.Comment);

        var result = command.ExecuteScalar();
        return Convert.ToInt32(result);
    }

    /// <summary>Returns all reviews for the given event.</summary>
    public List<Review> GetByEvent(int eventId)
    {
        var reviews = new List<Review>();
        
        using var connection = new SqliteConnection("Data Source = eventplatform.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT ReviewId, BookingId, EventId, UserId, Rating, Comment
            FROM Reviews 
            WHERE EventId = @eventId
";
        command.Parameters.AddWithValue("@eventId", eventId);

        using var reader = command.ExecuteReader();
        
        While (reader.Read())
        {
            reviews.Add(new Review
            {
                ReviewId = reader.GetInt32(0),
                BookingId = reader.GetInt32(1),
                EventId = reader.GetInt32(2),
                UserId = reader.GetInt32(3),
                Rating = reader.GetInt32(4),
                Comment = reader.IsDBNull(5) ? "" : reader.GetString(5)
            });
        }
        return reviews;
    }

    /// <summary>Returns true if a review already exists for the given booking.</summary>
    public bool ExistsForBooking(int bookingId)
    {
        using var connection = new SqliteConnection("Data Source = eventplatform.db");
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT COUNT(*) FROM Reviews 
            WHERE BookingId = @bookingId; 
        ";
        command.Parameters.AddWithValue("@bookingId", bookingId);

        var count = Convert.ToInt32(command.ExecuteScalar());
        return count > 0;
    }
}
