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

    public void ShowMyBookings()
    {
        var user = _userService.GetCurrentUser();
        if (user == null)
        {
            ConsoleHelper.PrintError("You must be logged in to view your bookings.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        while (true)
        {
            ConsoleHelper.ClearAndPrintHeader("My Bookings");
            ConsoleHelper.PrintDivider();

            List<BookingDetail> bookings;
            try
            {
                bookings = _bookingService.GetUserBookingsWithDetails(user.UserId);
            }
            catch (NotImplementedException)
            {
                ConsoleHelper.PrintError("My Bookings is not available yet.");
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }

            var upcoming = bookings
                .Where(b => b.EventDate >= DateTime.Today && b.Status == BookingStatus.Confirmed)
                .ToList();

            var past = bookings
                .Where(b => b.EventDate < DateTime.Today || b.Status == BookingStatus.Cancelled)
                .ToList();

            // --- Upcoming ---
            Console.WriteLine(" UPCOMING");
            ConsoleHelper.PrintDivider();

            if (upcoming.Count == 0)
                Console.WriteLine("    No upcoming bookings.");
            else
                PrintBookingTable(upcoming, showIndex: true);

            // --- Past ---
            ConsoleHelper.PrintDivider();
            Console.WriteLine(" PAST");
            ConsoleHelper.PrintDivider();

            if (past.Count == 0)
                Console.WriteLine("    No past bookings.");
            else
                PrintBookingTable(past, showIndex: false);

            ConsoleHelper.PrintDivider();

            if (upcoming.Count > 0)
                Console.WriteLine(" Enter booking number to manage, or 0 to go back.");
            else
                Console.WriteLine(" 0. Go back");

            ConsoleHelper.PrintDivider();
            Console.Write("Choose: ");
            string input = Console.ReadLine() ?? "";

            if (input == "0") return;

            if (upcoming.Count > 0 &&
                int.TryParse(input, out int choice) &&
                choice >= 1 && choice <= upcoming.Count)
            {
                ShowManageBooking(upcoming[choice - 1]);
            }
            else
            {
                ConsoleHelper.PrintError("Invalid selection.");
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }
    }

    private void ShowManageBooking(BookingDetail booking)
    {
        ConsoleHelper.ClearAndPrintHeader("Manage Booking");
        ConsoleHelper.PrintDivider();
        Console.WriteLine($"    Reference: {booking.Reference}");
        Console.WriteLine($"    Event:     {booking.EventTitle}");
        Console.WriteLine($"    Date:      {booking.EventDate:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"    Ticket:    {booking.TicketName}");
        Console.WriteLine($"    Price:     kr {(int)booking.PriceAtBooking}");
        ConsoleHelper.PrintDivider();
        Console.WriteLine(" 1. Cancel this booking");
        Console.WriteLine(" 0. Go back");
        ConsoleHelper.PrintDivider();
        Console.Write("Choose: ");
        string input = Console.ReadLine() ?? "";

        if (input == "0") return;

        if (input == "1")
        {
            Console.Write("Are you sure you want to cancel? (Y/N): ");
            string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (confirm != "Y")
            {
                Console.WriteLine(" Cancellation aborted.");
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }

            var user = _userService.GetCurrentUser();
            bool success = _bookingService.CancelBooking(booking.BookingId, user!);

            if (success)
                ConsoleHelper.PrintSuccess("Booking cancelled successfully.");
            else
                ConsoleHelper.PrintError("Could not cancel booking. Please try again.");

            ConsoleHelper.PressAnyKeyToContinue();
        }
        else
        {
            ConsoleHelper.PrintError("Invalid option.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    public void ShowCancelBooking(int bookingId) => throw new NotImplementedException();

    private static void PrintBookingTable(List<BookingDetail> bookings, bool showIndex)
    {
        string indexCol = showIndex ? "#".PadRight(4) : "    ";
        Console.WriteLine(
            indexCol +
            "Reference".PadRight(12) +
            "Event".PadRight(22) +
            "Date".PadRight(12) +
            "Ticket".PadRight(14) +
            "Price".PadRight(8) +
            "Status"
        );
        ConsoleHelper.PrintDivider();

        for (int i = 0; i < bookings.Count; i++)
        {
            var b = bookings[i];
            string index = showIndex ? $"{i + 1}".PadRight(4) : "    ";
            Console.WriteLine(
                index +
                b.Reference.PadRight(12) +
                Truncate(b.EventTitle, 21).PadRight(22) +
                b.EventDate.ToString("yyyy-MM-dd").PadRight(12) +
                Truncate(b.TicketName, 13).PadRight(14) +
                $"kr {(int)b.PriceAtBooking}".PadRight(8) +
                b.Status
            );
        }
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength) return value;
        return value[..(maxLength - 1)] + "…";
    }
}