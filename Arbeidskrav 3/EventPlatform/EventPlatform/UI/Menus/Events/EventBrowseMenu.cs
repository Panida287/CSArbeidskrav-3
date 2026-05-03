using EventPlatform.Services;
using EventPlatform.Enums;
using EventPlatform.Models;
using EventPlatform.Models.Events;

namespace EventPlatform.UI.Menus.Events;

/// <summary>
/// Browse, Search, and Filter event screens.
/// </summary>
public class EventBrowseMenu
{
    private readonly EventService _eventService;
    private readonly UserService _userService;
    private readonly BookingService _bookingService;
    private readonly ReviewService _reviewService;

    public EventBrowseMenu(EventService eventService, UserService userService,
        BookingService bookingService, ReviewService reviewService)
    {
        _eventService = eventService;
        _userService = userService;
        _bookingService = bookingService;
        _reviewService = reviewService;
    }

    public void ShowBrowse()
    {
        while (true)
        {
            ConsoleHelper.ClearAndPrintHeader("Browse Events");
            ConsoleHelper.PrintDivider();

            List<Event> events;
            try
            {
                events = _eventService.GetAll()
                    .OrderBy(e => e.EventDate)
                    .ToList();
            }
            catch (NotImplementedException)
            {
                ConsoleHelper.PrintError("Browse Events is not available yet.");
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }

            if (events.Count == 0)
            {
                ConsoleHelper.PrintError("No events found.");
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }

            PrintEventTable(events);
            ConsoleHelper.PrintDivider();
            Console.WriteLine(" 0. Go back");
            ConsoleHelper.PrintDivider();
            Console.Write("Select event number to view details: ");

            string input = Console.ReadLine() ?? "";
            if (input == "0") return;

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= events.Count)
                ShowDetail(events[choice - 1].EventId);
            else
            {
                ConsoleHelper.PrintError("Invalid selection. Please try again.");
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }
    }

    public void ShowSearch()
    {
        while (true)
        {
            ConsoleHelper.ClearAndPrintHeader("Search Events");
            ConsoleHelper.PrintDivider();
            Console.Write("Enter keyword (or 0 to go back): ");

            string keyword = Console.ReadLine() ?? "";
            if (keyword == "0") return;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                ConsoleHelper.PrintError("Please enter a keyword to search.");
                ConsoleHelper.PressAnyKeyToContinue();
                continue;
            }

            try
            {
                var events = _eventService.SearchEvents(keyword);
                ConsoleHelper.ClearAndPrintHeader($"Search Results: \"{keyword}\"");
                ConsoleHelper.PrintDivider();

                if (events.Count == 0)
                {
                    ConsoleHelper.PrintError($"No events found matching \"{keyword}\".");
                    ConsoleHelper.PressAnyKeyToContinue();
                    continue;
                }

                PrintEventTable(events);
                ConsoleHelper.PrintDivider();
                Console.WriteLine(" 0. Go back");
                ConsoleHelper.PrintDivider();
                Console.Write("Select event number to view details: ");

                string input = Console.ReadLine() ?? "";
                if (input == "0") return;

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= events.Count)
                    ShowDetail(events[choice - 1].EventId);
                else
                {
                    ConsoleHelper.PrintError("Invalid selection. Please try again.");
                    ConsoleHelper.PressAnyKeyToContinue();
                }
            }
            catch (NotImplementedException)
            {
                ConsoleHelper.PrintError("Search is not available yet.");
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }
        }
    }

    public void ShowFilter()
    {
        while (true)
        {
            ConsoleHelper.ClearAndPrintHeader("Filter Events");
            ConsoleHelper.PrintDivider();
            Console.WriteLine(" 1. Filter by Category");
            Console.WriteLine(" 2. Filter by Type");
            Console.WriteLine(" 0. Go back");
            ConsoleHelper.PrintDivider();
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (choice == "1") ShowFilterByCategory();
            else if (choice == "2") ShowFilterByType();
            else
            {
                ConsoleHelper.PrintError("Invalid option. Please try again.");
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }
    }

    private void ShowFilterByCategory()
    {
        ConsoleHelper.ClearAndPrintHeader("Filter by Category");
        ConsoleHelper.PrintDivider();
        Console.WriteLine(" 1. Music");
        Console.WriteLine(" 2. Technology");
        Console.WriteLine(" 3. Arts");
        Console.WriteLine(" 4. Food");
        Console.WriteLine(" 5. Sports");
        Console.WriteLine(" 6. Education");
        Console.WriteLine(" 7. Other");
        Console.WriteLine(" 0. Go back");
        ConsoleHelper.PrintDivider();
        Console.Write("Choose a category: ");

        string input = Console.ReadLine() ?? "";

        EventCategory? selected = input switch
        {
            "1" => EventCategory.Music,
            "2" => EventCategory.Technology,
            "3" => EventCategory.Arts,
            "4" => EventCategory.Food,
            "5" => EventCategory.Sports,
            "6" => EventCategory.Education,
            "7" => EventCategory.Other,
            _ => null
        };

        if (input == "0") return;
        if (selected == null)
        {
            ConsoleHelper.PrintError("Invalid option. Please try again.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        try
        {
            var events = _eventService.GetAll()
                .Where(e => e.Category == selected)
                .ToList();

            ConsoleHelper.ClearAndPrintHeader($"Events: {selected}");
            ConsoleHelper.PrintDivider();

            if (events.Count == 0)
            {
                ConsoleHelper.PrintError("No events found for this category.");
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }

            PrintEventTable(events);
            ConsoleHelper.PrintDivider();
            Console.WriteLine(" 0. Go back");
            ConsoleHelper.PrintDivider();
            Console.Write("Select event number to view details: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (int.TryParse(choice, out int eventChoice) && eventChoice >= 1 && eventChoice <= events.Count)
                ShowDetail(events[eventChoice - 1].EventId);
            else
            {
                ConsoleHelper.PrintError("Invalid selection.");
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Filter is not available yet.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private void ShowFilterByType()
    {
        ConsoleHelper.ClearAndPrintHeader("Filter by Type");
        ConsoleHelper.PrintDivider();
        Console.WriteLine(" 1. Concert");
        Console.WriteLine(" 2. Conference");
        Console.WriteLine(" 3. Workshop");
        Console.WriteLine(" 0. Go back");
        ConsoleHelper.PrintDivider();
        Console.Write("Choose a type: ");

        string input = Console.ReadLine() ?? "";
        if (input == "0") return;

        string? selectedType = input switch
        {
            "1" => "Concert",
            "2" => "Conference",
            "3" => "Workshop",
            _ => null
        };

        if (selectedType == null)
        {
            ConsoleHelper.PrintError("Invalid option. Please try again.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        try
        {
            var events = _eventService.GetAll()
                .Where(e => e.EventType == selectedType)
                .ToList();

            ConsoleHelper.ClearAndPrintHeader($"Events: {selectedType}");
            ConsoleHelper.PrintDivider();

            if (events.Count == 0)
            {
                ConsoleHelper.PrintError("No events found for this type.");
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }

            PrintEventTable(events);
            ConsoleHelper.PrintDivider();
            Console.WriteLine(" 0. Go back");
            ConsoleHelper.PrintDivider();
            Console.Write("Select event number to view details: ");

            string choice = Console.ReadLine() ?? "";
            if (choice == "0") return;

            if (int.TryParse(choice, out int eventChoice) && eventChoice >= 1 && eventChoice <= events.Count)
                ShowDetail(events[eventChoice - 1].EventId);
            else
            {
                ConsoleHelper.PrintError("Invalid selection.");
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Filter is not available yet.");
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private void ShowDetail(int eventId)
    {
        var detailMenu = new EventDetailMenu(_eventService, _userService, _bookingService, _reviewService);
        detailMenu.Show(eventId);
    }

    public static void PrintEventTable(List<Event> events)
    {
        Console.WriteLine(
            " #".PadRight(4) +
            "Title".PadRight(26) +
            "Type".PadRight(14) +
            "Date".PadRight(14) +
            "Tickets"
        );
        ConsoleHelper.PrintDivider();

        for (int i = 0; i < events.Count; i++)
        {
            var e = events[i];
            string tickets = e.Status == EventStatus.Upcoming ? "Available" : "Sold Out";
            string date = e.EventDate.ToString("yyyy-MM-dd");

            Console.WriteLine(
                $" {i + 1}".PadRight(4) +
                e.Title.PadRight(26) +
                e.EventType.PadRight(14) +
                date.PadRight(14) +
                tickets
            );
        }
    }
}