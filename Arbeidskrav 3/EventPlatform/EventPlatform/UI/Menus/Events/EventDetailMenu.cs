using EventPlatform.Services;
using EventPlatform.Enums;
using EventPlatform.Models;
using EventPlatform.Models.Events;

namespace EventPlatform.UI.Menus.Events;

/// <summary>
/// Full information screen for a selected event.
/// Read-only display with context-aware action options.
/// </summary>
public class EventDetailMenu
{
    private readonly EventService _eventService;
    private readonly UserService _userService;
    private readonly BookingService _bookingService;
    private readonly ReviewService _reviewService;

    public EventDetailMenu(EventService eventService, UserService userService,
        BookingService bookingService, ReviewService reviewService)
    {
        _eventService = eventService;
        _userService = userService;
        _bookingService = bookingService;
        _reviewService = reviewService;
    }

    public void Show(int eventId)
    {
        var user = _userService.GetCurrentUser();

        Event? ev;
        try
        {
            ev = _eventService.GetById(eventId);
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Event details are not available yet.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        if (ev == null)
        {
            ConsoleHelper.PrintError("Event not found.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        while (true)
        {
            ConsoleHelper.ClearAndPrintHeader("Event Details");
            ConsoleHelper.PrintDivider();

            // --- Event info ---
            Console.WriteLine($"    Title:       {ev.Title}");
            Console.WriteLine($"    Organiser:   {ev.OrganiserId}");
            Console.WriteLine($"    Type:        {ev.EventType}");
            Console.WriteLine($"    Category:    {ev.Category}");
            Console.WriteLine($"    Date:        {ev.EventDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"    Venue:       {ev.Venue}");
            Console.WriteLine($"    Status:      {ev.Status}");
            Console.WriteLine($"    Description: {ev.Description}");

            // --- Average rating ---
            double? avg = _reviewService.GetAverageRating(eventId);
            var reviews = _reviewService.GetReviewsForEvent(eventId);
            string ratingLine = avg.HasValue
                ? $"★ {avg:F1} / 5 ({reviews.Count} reviews)"
                : "No reviews yet.";
            Console.WriteLine($"    Rating:      {ratingLine}");

            ConsoleHelper.PrintDivider();

            // --- Ticket types table (not yet available) ---
            Console.WriteLine("    Ticket info not available yet.");

            ConsoleHelper.PrintDivider();

            // --- Context-aware options ---
            bool isOrganiser = user != null && user.UserId == ev.OrganiserId;

            if (isOrganiser)
            {
                Console.WriteLine(" 1. Edit Event");
                Console.WriteLine(" 2. Cancel Event");
            }
            else
            {
                Console.WriteLine(" 1. Book a Ticket");
                Console.WriteLine(" 2. View Reviews");

                // Leave a Review — only if completed, user attended, and not yet reviewed
                if (ev.Status == EventStatus.Completed && user != null)
                {
                    try
                    {
                        var userBookings = _bookingService.GetUserBookings(user.UserId);
                        var booking = userBookings
                            .FirstOrDefault(b => b.EventId == eventId &&
                                                 b.Status == BookingStatus.Confirmed);

                        if (booking != null && !_reviewService.GetReviewsForEvent(eventId)
                                .Any(r => r.BookingId == booking.BookingId))
                        {
                            Console.WriteLine(" 3. Leave a Review");
                        }
                    }
                    catch (NotImplementedException) { }
                }
            }

            Console.WriteLine(" 0. Back");
            ConsoleHelper.PrintDivider();
            Console.Write("Choose an option: ");
            string input = Console.ReadLine() ?? "";

            if (input == "0") return;

            if (isOrganiser)
            {
                if (input == "1")
                {
                    ConsoleHelper.PrintError("Edit: use My Events menu to edit this event.");
                    ConsoleHelper.PressAnyKeyToContinue();
                }
                else if (input == "2")
                {
                    ConsoleHelper.PrintError("Cancel: use My Events menu to cancel this event.");
                    ConsoleHelper.PressAnyKeyToContinue();
                }
                else
                {
                    ConsoleHelper.PrintError("Invalid option.");
                    ConsoleHelper.PressAnyKeyToContinue();
                }
            }
            else
            {
                if (input == "1")
                {
                    var bookingMenu = new BookingMenu(_bookingService, _userService);
                    bookingMenu.ShowBookTicket(eventId);
                }
                else if (input == "2")
                {
                    var reviewMenu = new ReviewMenu(_reviewService, _userService);
                    reviewMenu.ShowEventReviews(eventId);
                }
                else if (input == "3")
                {
                    try
                    {
                        var userBookings = _bookingService.GetUserBookings(user!.UserId);
                        var booking = userBookings
                            .FirstOrDefault(b => b.EventId == eventId &&
                                                 b.Status == BookingStatus.Confirmed);

                        if (booking != null)
                        {
                            var reviewMenu = new ReviewMenu(_reviewService, _userService);
                            reviewMenu.ShowLeaveReview(booking, ev);
                        }
                    }
                    catch (NotImplementedException)
                    {
                        ConsoleHelper.PrintError("Reviews not available yet.");
                        ConsoleHelper.PressAnyKeyToContinue();
                    }
                }
                else
                {
                    ConsoleHelper.PrintError("Invalid option.");
                    ConsoleHelper.PressAnyKeyToContinue();
                }
            }
        }
    }
}