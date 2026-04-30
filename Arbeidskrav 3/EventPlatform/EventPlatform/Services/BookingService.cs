using EventPlatform.Models;
using EventPlatform.Repositories;
using EventPlatform.Models.Events;
using EventPlatform.Enums;

namespace EventPlatform.Services;

/// <summary>
/// Handles ticket booking, cancellation, and retrieval.
/// No Console I/O — pure business logic only.
/// </summary>
public class BookingService
{
    private readonly BookingRepository _bookingRepository;

    public BookingService(BookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    /// <summary>Returns all ticket types for a given event.</summary>
    public List<TicketType> GetTicketTypesForEvent(int eventId)
    {
        return _bookingRepository.GetTicketTypesByEvent(eventId);
    }

    /// <summary>Books a ticket for the user. Returns null on failure.</summary>
    public Booking? BookTicket(User user, Event ev, TicketType ticketType)
    {
        // Guard 1: user cannot book their own event
        if (user.UserId == ev.OrganiserId)
            return null;

        // Guard 2: ticket must have remaining capacity
        if (ticketType.Remaining <= 0)
            return null;

        // Guard 3: no duplicate booking
        var existingBookings = _bookingRepository.GetByUser(user.UserId);
        if (existingBookings.Any(b => b.EventId == ev.EventId && b.TicketTypeId == ticketType.TicketTypeId))
            return null;

        // Build the booking
        var booking = new Booking
        {
            UserId         = user.UserId,
            EventId        = ev.EventId,
            TicketTypeId   = ticketType.TicketTypeId,
            PriceAtBooking = ticketType.Price,
            BookingDate    = DateTime.UtcNow,
            Status         = BookingStatus.Confirmed,
            Reference      = GenerateReference()
        };

        // Save and decrement
        booking.BookingId = _bookingRepository.Insert(booking);
        _bookingRepository.DecrementRemaining(ticketType.TicketTypeId);

        return booking;
    }

    /// <summary>Cancels an existing booking. Returns false if not allowed.</summary>
    public bool CancelBooking(int bookingId, User requestingUser)
    {
        var userBookings = _bookingRepository.GetByUser(requestingUser.UserId);
        var booking = userBookings.FirstOrDefault(b => b.BookingId == bookingId);

        if (booking == null) return false;
        if (booking.UserId != requestingUser.UserId) return false;

        _bookingRepository.UpdateStatus(bookingId, BookingStatus.Cancelled.ToString());
        _bookingRepository.IncrementRemaining(booking.TicketTypeId);

        return true;
    }

    /// <summary>Returns all bookings for a given user.</summary>
    public List<Booking> GetUserBookings(int userId)
    {
        return _bookingRepository.GetByUser(userId);
    }
    
    /// <summary>Returns all bookings for a user with event and ticket details.</summary>
    public List<BookingDetail> GetUserBookingsWithDetails(int userId)
    {
        return _bookingRepository.GetByUserWithDetails(userId);
    }

    /// <summary>Generates a unique booking reference in BK-XXXXX format.</summary>
    private string GenerateReference()
    {
        var existing = _bookingRepository.GetAllReferences();
        int next = existing.Count + 1;
        return $"BK-{next:D5}";
    }
}