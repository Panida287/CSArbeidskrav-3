using EventPlatform.Services;
using EventPlatform.Enums;
using EventPlatform.Models;
using EventPlatform.Models.Events;
using System;
using System.Collections.Generic;

namespace EventPlatform.UI.Menus;

/// <summary>
/// Browse, Search, Filter, Create Event, My Events screens.
/// </summary>
public class EventMenu
{
    private readonly EventService _eventService;
    private readonly UserService _userService;
    private readonly BookingService _bookingService;
    private readonly ReviewService _reviewService;

    public EventMenu(EventService eventService, UserService userService, BookingService bookingService,
        ReviewService reviewService)
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

            if (choice == "1")
                ShowFilterByCategory();
            else if (choice == "2")
                ShowFilterByType();
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

            if (int.TryParse(choice, out int eventChoice) &&
                eventChoice >= 1 && eventChoice <= events.Count)
            {
                ShowDetail(events[eventChoice - 1].EventId);
            }
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

            if (int.TryParse(choice, out int eventChoice) &&
                eventChoice >= 1 && eventChoice <= events.Count)
            {
                ShowDetail(events[eventChoice - 1].EventId);
            }
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

    public void ShowDetail(int eventId)
    {
        var detailMenu = new EventDetailMenu(_eventService, _userService, _bookingService, _reviewService);
        detailMenu.Show(eventId);
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

        string title = PromptRequired("Title");
        if (title == null) return;

        string description = PromptRequired("Description");
        if (description == null) return;

        string venue = PromptRequired("Venue");
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

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength) return value;
        return value[..(maxLength - 1)] + "…";
    }
}