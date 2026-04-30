using System;
using System.Collections.Generic;
using EventPlatform.Models;
using EventPlatform.Models.Events;
using EventPlatform.Repositories;
using EventPlatform.Enums;

namespace EventPlatform.Services;

/// <summary>
/// Handles review submission, retrieval, and eligibility checks.
/// No Console I/O — pure business logic only.
/// </summary>
public class ReviewService
{
    private readonly ReviewRepository _reviewRepository;
    private readonly BookingRepository _bookingRepository;

    public ReviewService(ReviewRepository reviewRepository, BookingRepository bookingRepository)
    {
        _reviewRepository = reviewRepository;
        _bookingRepository = bookingRepository;
    }

    /// <summary>Adds a review. Returns false if user is not eligible.</summary>
    public bool AddReview(User user, Event ev, Booking booking, int rating, string comment)
    {
        if (!IsEligible(user, ev, booking))
            return false;

        if (rating < 1 || rating > 5)
            return false;

        var review = new Review
        {
            UserId    = booking.UserId,
            EventId   = booking.EventId,
            BookingId = booking.BookingId,
            Rating    = rating,
            Comment   = comment
        };

        _reviewRepository.Insert(review);

        return true;
    }
    /// <summary>Returns all reviews for a given event.</summary>
    public List<Review> GetReviewsForEvent(int eventId)
    {
        return _reviewRepository.GetByEvent(eventId);
    }

    /// <summary>Returns true if the user is eligible to review (attended + not already reviewed).</summary>
    public bool IsEligible(User user, Event ev, Booking booking)
    {
        // Check 1: event must be completed
        if (ev.Status != EventStatus.Completed)
            return false;

        // Check 2: booking must belong to user, match event, and be confirmed
        if (booking.UserId != user.UserId ||
            booking.EventId != ev.EventId ||
            booking.Status != BookingStatus.Confirmed)
            return false;

        // Check 3: organiser cannot review own event
        if (ev.OrganiserId == user.UserId)
            return false;

        // Check 4: booking must not already have a review
        if (_reviewRepository.ExistsForBooking(booking.BookingId))
            return false;

        return true;
    }

    /// <summary>Returns the average rating for an event, or null if no reviews.</summary>
    public double? GetAverageRating(int eventId)
    {
        var reviews = _reviewRepository.GetByEvent(eventId);

        if (reviews.Count == 0)
            return null;

        return Math.Round(reviews.Average(r => r.Rating), 1);
    }
}