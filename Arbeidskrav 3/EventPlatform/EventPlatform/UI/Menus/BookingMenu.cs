using System;
using EventPlatform.Services;

namespace EventPlatform.UI.Menus;

/// <summary>
/// Book a ticket, My Bookings, and Cancel Booking screens.
/// </summary>
public class BookingMenu
{
    private readonly BookingService _bookingService;
    private readonly UserService _userService;

    public BookingMenu(BookingService bookingService, UserService userService)
    {
        _bookingService = bookingService;
        _userService = userService;
    }

    public void ShowBookTicket(int eventId) => throw new NotImplementedException();
    public void ShowMyBookings() => throw new NotImplementedException();
    public void ShowCancelBooking(int bookingId) => throw new NotImplementedException();
}
