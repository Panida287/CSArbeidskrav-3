using System;
using System.Collections.Generic;
using EventPlatform.Models;

namespace EventPlatform.Services;

/// <summary>
/// Handles review submission, retrieval, and eligibility checks.
/// No Console I/O — pure business logic only.
/// </summary>
public class ReviewService
{
    /// <summary>Adds a review. Returns false if user is not eligible.</summary>
    public bool AddReview(int userId, int bookingId, int rating, string comment) => throw new NotImplementedException();

    /// <summary>Returns all reviews for a given event.</summary>
    public List<Review> GetReviews(int eventId) => throw new NotImplementedException();

    /// <summary>Returns true if the user is eligible to review (attended + not already reviewed).</summary>
    public bool IsEligible(int userId, int bookingId) => throw new NotImplementedException();

    /// <summary>Returns the average rating for an event, or null if no reviews.</summary>
    public double? GetAverageRating(int eventId) => throw new NotImplementedException();
}
