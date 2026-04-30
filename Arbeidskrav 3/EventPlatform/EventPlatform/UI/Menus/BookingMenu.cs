using EventPlatform.Models;
using EventPlatform.Models.Events;
using EventPlatform.Services;
using EventPlatform.Enums;

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

    public void ShowBookTicket(Event ev)
    {
        var user = _userService.GetCurrentUser();
        if (user == null)
        {
            ConsoleHelper.PrintError("You must be logged in to book a ticket.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        // --- Load ticket types ---
        List<TicketType> tickets;
        try
        {
            tickets = _bookingService.GetTicketTypesForEvent(ev.EventId);
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Booking is not available yet.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        if (tickets.Count == 0)
        {
            ConsoleHelper.PrintError("No ticket types available for this event.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        // --- Show ticket selector ---
        ConsoleHelper.ClearAndPrintHeader("Book a Ticket");
        ConsoleHelper.PrintDivider();
        Console.WriteLine($"    Event: {ev.Title}");
        ConsoleHelper.PrintDivider();
        Console.WriteLine(
            "    " +
            "#".PadRight(4) +
            "Ticket Type".PadRight(24) +
            "Price".PadRight(12) +
            "Availability"
        );
        ConsoleHelper.PrintDivider();

        for (int i = 0; i < tickets.Count; i++)
        {
            var t = tickets[i];
            string availability = t.Remaining > 0 ? $"{t.Remaining} left" : "Sold Out";
            Console.WriteLine(
                "    " +
                $"{i + 1}".PadRight(4) +
                t.Name.PadRight(24) +
                $"kr {(int)t.Price}".PadRight(12) +
                availability
            );
        }

        ConsoleHelper.PrintDivider();
        Console.WriteLine(" 0. Go back");
        ConsoleHelper.PrintDivider();
        Console.Write("Select ticket type: ");
        string input = Console.ReadLine() ?? "";

        if (input == "0") return;

        if (!int.TryParse(input, out int choice) || choice < 1 || choice > tickets.Count)
        {
            ConsoleHelper.PrintError("Invalid selection.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        var selectedTicket = tickets[choice - 1];

        // --- Check if sold out ---
        if (selectedTicket.Remaining <= 0)
        {
            ConsoleHelper.PrintError("This ticket type is sold out.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        // --- Show booking summary ---
        ConsoleHelper.ClearAndPrintHeader("Confirm Booking");
        ConsoleHelper.PrintDivider();
        Console.WriteLine($"    Event:       {ev.Title}");
        Console.WriteLine($"    Ticket:      {selectedTicket.Name}");
        Console.WriteLine($"    Price:       kr {(int)selectedTicket.Price}");
        ConsoleHelper.PrintDivider();
        Console.Write("Confirm booking? (Y/N): ");
        string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "";

        if (confirm != "Y")
        {
            Console.WriteLine(" Booking cancelled.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        // --- Attempt booking ---
        try
        {
            var booking = _bookingService.BookTicket(user, ev, selectedTicket);

            if (booking == null)
            {
                ConsoleHelper.PrintError("Booking failed. You may have already booked this ticket type or this is your own event.");
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }

            ConsoleHelper.PrintSuccess("Booking confirmed!");
            Console.WriteLine($"    Reference: {booking.Reference}");
            Console.WriteLine("    Payment is handled externally. Enjoy the event!");
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Booking is not available yet.");
        }

        ConsoleHelper.PressAnyKeyToContinue();
    }

    public void ShowMyBookings() => throw new NotImplementedException();
    public void ShowCancelBooking(int bookingId) => throw new NotImplementedException();
}