using EventPlatform.Services;
using EventPlatform.Enums;
using EventPlatform.Models;
using EventPlatform.Models.Events;

namespace EventPlatform.UI.Menus.Events;

/// <summary>
/// My Events, Edit, and Cancel event screens.
/// </summary>
public class EventManageMenu
{
    private readonly EventService _eventService;
    private readonly UserService _userService;

    public EventManageMenu(EventService eventService, UserService userService)
    {
        _eventService = eventService;
        _userService = userService;
    }

    public void ShowMyEvents()
    {
        User? currentUser = _userService.GetCurrentUser();
        if (currentUser == null)
        {
            ConsoleHelper.PrintError("You must be logged in to view your events.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        while (true)
        {
            ConsoleHelper.ClearAndPrintHeader($"My Events — {currentUser.Username}");
            ConsoleHelper.PrintDivider();

            List<Event> myEvents;
            try
            {
                myEvents = _eventService.GetAll()
                    .Where(e => e.OrganiserId == currentUser.UserId)
                    .ToList();
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }

            if (myEvents.Count == 0)
            {
                Console.WriteLine(" You have not created any events yet.");
                ConsoleHelper.PrintDivider();
                Console.WriteLine(" 0. Go back");
                ConsoleHelper.PrintDivider();
                Console.Write("Choose: ");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(
                " #".PadRight(4) +
                "Title".PadRight(26) +
                "Date".PadRight(14) +
                "Status".PadRight(14) +
                "Bookings"
            );
            ConsoleHelper.PrintDivider();

            for (int i = 0; i < myEvents.Count; i++)
            {
                var e = myEvents[i];
                Console.WriteLine(
                    $" {i + 1}".PadRight(4) +
                    Truncate(e.Title, 25).PadRight(26) +
                    e.EventDate.ToString("yyyy-MM-dd").PadRight(14) +
                    e.Status.ToString().PadRight(14) +
                    "—"
                );
            }

            ConsoleHelper.PrintDivider();
            Console.WriteLine(" 0. Go back");
            ConsoleHelper.PrintDivider();
            Console.Write("Select an event to manage: ");
            string input = Console.ReadLine() ?? "";

            if (input == "0") return;

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= myEvents.Count)
                ShowManageEvent(myEvents[choice - 1]);
            else
            {
                ConsoleHelper.PrintError("Please enter a number from the list.");
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }
    }

    private void ShowManageEvent(Event ev)
    {
        while (true)
        {
            ConsoleHelper.ClearAndPrintHeader($"Manage — {ev.Title}");
            ConsoleHelper.PrintDivider();
            Console.WriteLine($" Status : {ev.Status}");
            Console.WriteLine($" Date   : {ev.EventDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($" Venue  : {ev.Venue}");
            ConsoleHelper.PrintDivider();

            if (ev.Status == EventStatus.Cancelled)
            {
                ConsoleHelper.PrintError("This event is already cancelled.");
                ConsoleHelper.PrintDivider();
                Console.WriteLine(" 0. Go back");
                ConsoleHelper.PrintDivider();
                Console.Write("Choose: ");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(" 1. Edit Event");
            Console.WriteLine(" 2. Cancel Event");
            Console.WriteLine(" 0. Go back");
            ConsoleHelper.PrintDivider();
            Console.Write("Choose: ");
            string input = Console.ReadLine() ?? "";

            if (input == "0") return;
            if (input == "1") { ShowEditEvent(ev); return; }
            if (input == "2") { ShowCancelEvent(ev); return; }

            ConsoleHelper.PrintError("Please enter 1, 2, or 0.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private void ShowEditEvent(Event ev)
    {
        User? currentUser = _userService.GetCurrentUser();
        if (currentUser == null || currentUser.UserId != ev.OrganiserId)
        {
            ConsoleHelper.PrintError("Only the organiser can edit this event.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        ConsoleHelper.ClearAndPrintHeader($"Edit Event — {ev.Title}");
        ConsoleHelper.PrintDivider();
        Console.WriteLine(" Press Enter to keep the existing value.\n");

        Console.Write($" Title [{ev.Title}]: ");
        string title = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(title)) ev.Title = title;

        Console.Write($" Description [{ev.Description}]: ");
        string desc = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(desc)) ev.Description = desc;

        Console.Write($" Venue [{ev.Venue}]: ");
        string venue = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(venue)) ev.Venue = venue;

        Console.Write($" Date [{ev.EventDate:yyyy-MM-dd}]: ");
        string dateInput = Console.ReadLine()?.Trim() ?? "";
        if (!string.IsNullOrEmpty(dateInput))
        {
            if (DateTime.TryParse(dateInput, out DateTime newDate) && newDate.Date >= DateTime.Today)
                ev.EventDate = newDate;
            else
                ConsoleHelper.PrintError("Invalid or past date — date unchanged.");
        }

        if (ev is Concert concert)
        {
            Console.Write($" Performer [{concert.Performer}]: ");
            string p = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(p)) concert.Performer = p;

            Console.Write($" Genre [{concert.Genre}]: ");
            string g = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(g)) concert.Genre = g;
        }
        else if (ev is Conference conference)
        {
            Console.Write($" Session Schedule [{conference.SessionSchedule}]: ");
            string s = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(s)) conference.SessionSchedule = s;
        }
        else if (ev is Workshop workshop)
        {
            Console.Write($" Required Materials [{workshop.RequiredMaterials}]: ");
            string m = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(m)) workshop.RequiredMaterials = m;

            Console.Write($" Max Participants [{workshop.MaxParticipants}]: ");
            string mp = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrEmpty(mp) && int.TryParse(mp, out int max) && max > 0)
                workshop.MaxParticipants = max;
        }

        ConsoleHelper.PrintDivider();
        Console.Write("Save changes? (Y/N): ");
        string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "";
        if (confirm != "Y")
        {
            ConsoleHelper.PrintError("Edit cancelled — no changes saved.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        try
        {
            _eventService.Edit(ev, currentUser.UserId);
            ConsoleHelper.PrintSuccess("Event updated successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }

        ConsoleHelper.PressAnyKeyToContinue();
    }

    private void ShowCancelEvent(Event ev)
    {
        User? currentUser = _userService.GetCurrentUser();
        if (currentUser == null || currentUser.UserId != ev.OrganiserId)
        {
            ConsoleHelper.PrintError("Only the organiser can cancel this event.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        ConsoleHelper.ClearAndPrintHeader($"Cancel Event — {ev.Title}");
        ConsoleHelper.PrintDivider();
        ConsoleHelper.PrintError($"This will cancel \"{ev.Title}\" on {ev.EventDate:yyyy-MM-dd}.");
        Console.Write("Are you sure? (Y/N): ");
        string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "";

        if (confirm != "Y")
        {
            Console.WriteLine(" Cancellation aborted.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        try
        {
            _eventService.Cancel(ev.EventId, currentUser.UserId);
            ConsoleHelper.PrintSuccess($"\"{ev.Title}\" has been cancelled.");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }

        ConsoleHelper.PressAnyKeyToContinue();
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength) return value;
        return value[..(maxLength - 1)] + "…";
    }
}