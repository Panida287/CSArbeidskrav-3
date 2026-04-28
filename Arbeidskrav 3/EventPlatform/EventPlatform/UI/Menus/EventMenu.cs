using EventPlatform.Services;
using EventPlatform.Enums;
using EventPlatform.Models.Events;

namespace EventPlatform.UI.Menus;

/// <summary>
/// Browse, Search, Filter, Event Detail, Create Event, My Events screens.
/// </summary>
public class EventMenu
{
    private readonly EventService _eventService;
    private readonly UserService _userService;

    public EventMenu(EventService eventService, UserService userService)
    {
        _eventService = eventService;
        _userService = userService;
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
                events = _eventService.GetAll();
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
            {
                ShowDetail(events[choice - 1].EventId);
            }
            else
            {
                ConsoleHelper.PrintError("Invalid selection. Please try again.");
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }
    }

    private void PrintEventTable(List<Event> events)
    {
        //Print column headers
        Console.WriteLine(
            " #".PadRight(4) +
            "Title".PadRight(26) +
            "Type".PadRight(14) +
            "Date".PadRight(14) +
            "Tickets"
            );
        ConsoleHelper.PrintDivider();
        
        //Print each event as a row
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

                if (int.TryParse(input, out int choice) &&
                    choice >= 1 && choice <= events.Count)
                {
                    ShowDetail(events[choice - 1].EventId);
                }
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
    
    
    
    public void ShowFilter() => throw new NotImplementedException();
    public void ShowDetail(int eventId) => throw new NotImplementedException();
    public void ShowCreate() => throw new NotImplementedException();
    public void ShowMyEvents() => throw new NotImplementedException();
}
