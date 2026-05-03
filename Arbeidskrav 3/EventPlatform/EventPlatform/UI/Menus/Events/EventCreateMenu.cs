using EventPlatform.Services;
using EventPlatform.Enums;
using EventPlatform.Models;
using EventPlatform.Models.Events;

namespace EventPlatform.UI.Menus.Events;

/// <summary>
/// Create event screen — all 4 steps.
/// </summary>
public class EventCreateMenu
{
    private readonly EventService _eventService;
    private readonly UserService _userService;

    public EventCreateMenu(EventService eventService, UserService userService)
    {
        _eventService = eventService;
        _userService = userService;
    }

    public void ShowCreate()
    {
        User? organiser = _userService.GetCurrentUser();
        if (organiser == null)
        {
            ConsoleHelper.PrintError("You must be logged in to create an event.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        string? eventType = null;
        while (true)
        {
            ConsoleHelper.ClearAndPrintHeader("Create Event — Step 1: Type");
            ConsoleHelper.PrintDivider();
            Console.WriteLine(" 1. Concert");
            Console.WriteLine(" 2. Conference");
            Console.WriteLine(" 3. Workshop");
            Console.WriteLine(" 0. Go back");
            ConsoleHelper.PrintDivider();
            Console.Write("Choose event type: ");
            string input = Console.ReadLine() ?? "";

            if (input == "0") return;
            if (input == "1") { eventType = "Concert"; break; }
            if (input == "2") { eventType = "Conference"; break; }
            if (input == "3") { eventType = "Workshop"; break; }
            ConsoleHelper.PrintError("Please enter 1, 2, 3, or 0.");
            ConsoleHelper.PressAnyKeyToContinue();
        }

        ConsoleHelper.ClearAndPrintHeader("Create Event — Step 2: Details");
        ConsoleHelper.PrintDivider();

        string? title = PromptRequired("Title");
        if (title == null) return;

        string? description = PromptRequired("Description");
        if (description == null) return;

        string? venue = PromptRequired("Venue");
        if (venue == null) return;

        EventCategory? category = null;
        while (true)
        {
            Console.WriteLine("\n Category:");
            Console.WriteLine("  1. Music   2. Technology   3. Arts   4. Food");
            Console.WriteLine("  5. Sports  6. Education    7. Other  0. Cancel");
            Console.Write("Choose: ");
            string cat = Console.ReadLine() ?? "";
            category = cat switch
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
            if (cat == "0") return;
            if (category != null) break;
            ConsoleHelper.PrintError("Please enter a number from the list.");
        }

        DateTime eventDate;
        while (true)
        {
            Console.Write("Date (yyyy-MM-dd): ");
            string d = Console.ReadLine() ?? "";
            if (d == "0") return;
            if (DateTime.TryParse(d, out eventDate) && eventDate.Date >= DateTime.Today) break;
            ConsoleHelper.PrintError("Invalid date or date is in the past.");
        }

        TimeSpan eventTime;
        while (true)
        {
            Console.Write("Time (HH:mm): ");
            string t = Console.ReadLine() ?? "";
            if (t == "0") return;
            if (TimeSpan.TryParse(t, out eventTime)) break;
            ConsoleHelper.PrintError("Invalid time. Use HH:mm format.");
        }

        ConsoleHelper.ClearAndPrintHeader("Create Event — Step 3: Type Details");
        ConsoleHelper.PrintDivider();

        string? performer = null, genre = null, sessionSchedule = null, requiredMaterials = null;
        List<string> speakerList = new();
        int maxParticipants = 0;

        if (eventType == "Concert")
        {
            performer = PromptRequired("Performer");
            if (performer == null) return;
            genre = PromptRequired("Genre");
            if (genre == null) return;
        }
        else if (eventType == "Conference")
        {
            Console.WriteLine("Enter speaker names one at a time. Leave blank when done.");
            while (true)
            {
                Console.Write($"  Speaker {speakerList.Count + 1} (or blank to finish): ");
                string s = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(s))
                {
                    if (speakerList.Count == 0) { ConsoleHelper.PrintError("Add at least one speaker."); continue; }
                    break;
                }
                speakerList.Add(s);
            }
            sessionSchedule = PromptRequired("Session Schedule");
            if (sessionSchedule == null) return;
        }
        else if (eventType == "Workshop")
        {
            requiredMaterials = PromptRequired("Required Materials");
            if (requiredMaterials == null) return;
            while (true)
            {
                Console.Write("Max Participants: ");
                string m = Console.ReadLine() ?? "";
                if (m == "0") return;
                if (int.TryParse(m, out maxParticipants) && maxParticipants > 0) break;
                ConsoleHelper.PrintError("Please enter a positive number.");
            }
        }

        ConsoleHelper.ClearAndPrintHeader("Create Event — Step 4: Tickets");
        ConsoleHelper.PrintDivider();
        Console.WriteLine("Add at least one ticket type. Enter 0 for name when finished.\n");

        var ticketTypes = new List<TicketType>();
        while (true)
        {
            Console.Write($"Ticket {ticketTypes.Count + 1} name (or 0 to finish): ");
            string name = Console.ReadLine()?.Trim() ?? "";

            if (name == "0")
            {
                if (ticketTypes.Count == 0) { ConsoleHelper.PrintError("Add at least one ticket type."); continue; }
                break;
            }
            if (string.IsNullOrEmpty(name)) { ConsoleHelper.PrintError("Name cannot be empty."); continue; }

            decimal price;
            while (true)
            {
                Console.Write("  Price: ");
                string p = Console.ReadLine() ?? "";
                if (decimal.TryParse(p, out price) && price >= 0) break;
                ConsoleHelper.PrintError("Enter 0 or a positive price.");
            }

            int qty;
            while (true)
            {
                Console.Write("  Quantity: ");
                string q = Console.ReadLine() ?? "";
                if (int.TryParse(q, out qty) && qty > 0) break;
                ConsoleHelper.PrintError("Quantity must be greater than 0.");
            }

            ticketTypes.Add(new TicketType
            {
                Name = name,
                Price = price,
                TotalQuantity = qty,
                Remaining = qty
            });
            ConsoleHelper.PrintSuccess($"Added: {name} — {price:C} x {qty}");
        }

        ConsoleHelper.ClearAndPrintHeader("Confirm New Event");
        ConsoleHelper.PrintDivider();
        Console.WriteLine($"  Type        : {eventType}");
        Console.WriteLine($"  Title       : {title}");
        Console.WriteLine($"  Category    : {category}");
        Console.WriteLine($"  Date        : {eventDate.Add(eventTime):yyyy-MM-dd HH:mm}");
        Console.WriteLine($"  Venue       : {venue}");
        Console.WriteLine($"  Description : {description}");
        Console.WriteLine($"  Tickets     : {ticketTypes.Count} type(s)");
        ConsoleHelper.PrintDivider();
        Console.Write("Save this event? (Y/N): ");
        string confirm = Console.ReadLine()?.Trim().ToUpper() ?? "";

        if (confirm != "Y")
        {
            ConsoleHelper.PrintError("Event creation cancelled.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        try
        {
            Event newEvent = eventType switch
            {
                "Concert" => new Concert
                {
                    Performer = performer!,
                    Genre = genre!,
                    OrganiserId = organiser.UserId,
                    Title = title,
                    Description = description,
                    EventType = eventType,
                    Category = category!.Value,
                    EventDate = eventDate.Add(eventTime),
                    Venue = venue,
                    Status = EventStatus.Upcoming
                },
                "Conference" => new Conference
                {
                    SpeakerList = speakerList,
                    SessionSchedule = sessionSchedule!,
                    OrganiserId = organiser.UserId,
                    Title = title,
                    Description = description,
                    EventType = eventType,
                    Category = category!.Value,
                    EventDate = eventDate.Add(eventTime),
                    Venue = venue,
                    Status = EventStatus.Upcoming
                },
                "Workshop" => new Workshop
                {
                    RequiredMaterials = requiredMaterials!,
                    MaxParticipants = maxParticipants,
                    OrganiserId = organiser.UserId,
                    Title = title,
                    Description = description,
                    EventType = eventType,
                    Category = category!.Value,
                    EventDate = eventDate.Add(eventTime),
                    Venue = venue,
                    Status = EventStatus.Upcoming
                },
                _ => throw new InvalidOperationException("Unknown event type.")
            };

            _eventService.Create(newEvent);
            ConsoleHelper.PrintSuccess("Event created successfully!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.PrintError(ex.Message);
        }

        ConsoleHelper.PressAnyKeyToContinue();
    }

    private string? PromptRequired(string fieldName)
    {
        while (true)
        {
            Console.Write($" {fieldName} (or 0 to cancel): ");
            string value = Console.ReadLine()?.Trim() ?? "";
            if (value == "0") return null;
            if (!string.IsNullOrEmpty(value)) return value;
            ConsoleHelper.PrintError($"{fieldName} cannot be empty.");
        }
    }
}