using EventPlatform.Services;
using EventPlatform.UI.Menus.Events;
using System;

namespace EventPlatform.UI.Menus;

/// <summary>
/// Post-login navigation hub. Routes to all other menus.
/// </summary>
public class MainMenu
{
    private readonly UserService _userService;
    private readonly EventService _eventService;
    private readonly BookingService _bookingService;
    private readonly ReviewService _reviewService;

    public MainMenu(UserService userService, EventService eventService, BookingService bookingService, ReviewService reviewService)
    {
        _userService = userService;
        _eventService = eventService;
        _bookingService = bookingService;
        _reviewService = reviewService;
    }

    public void Show()
    {
        while (true)
        {
            var user = _userService.GetCurrentUser();
            ConsoleHelper.ClearAndPrintHeader($"Main menu | Logged in as: {user!.Username}");
            ConsoleHelper.PrintDivider();
            Console.WriteLine(" 1. Browse Events");
            Console.WriteLine(" 2. Search Events");
            Console.WriteLine(" 3. My Bookings");
            Console.WriteLine(" 4. Create Event");
            Console.WriteLine(" 5. My Events");
            Console.WriteLine(" 6. My Profile");
            Console.WriteLine(" 7. Log Out");
            ConsoleHelper.PrintDivider();
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    try
                    {
                        new EventBrowseMenu(_eventService, _userService, _bookingService, _reviewService).ShowBrowse();
                    }
                    catch (NotImplementedException)
                    {
                        ConsoleHelper.PrintError("Browse Events is not available yet.");
                        ConsoleHelper.PressAnyKeyToContinue();
                    }
                    break;

                case "2":
                    try
                    {
                        new EventBrowseMenu(_eventService, _userService, _bookingService, _reviewService).ShowSearch();
                    }
                    catch (NotImplementedException)
                    {
                        ConsoleHelper.PrintError("Search Events is not available yet.");
                        ConsoleHelper.PressAnyKeyToContinue();
                    }
                    break;

                case "3":
                    try
                    {
                        new BookingMenu(_bookingService, _userService).ShowMyBookings();
                    }
                    catch (NotImplementedException)
                    {
                        ConsoleHelper.PrintError("My Bookings is not available yet.");
                        ConsoleHelper.PressAnyKeyToContinue();
                    }
                    break;

                case "4":
                    try
                    {
                        new EventCreateMenu(_eventService, _userService).ShowCreate();
                    }
                    catch (NotImplementedException)
                    {
                        ConsoleHelper.PrintError("Create Event is not available yet.");
                        ConsoleHelper.PressAnyKeyToContinue();
                    }
                    break;

                case "5":
                    try
                    {
                        new EventManageMenu(_eventService, _userService).ShowMyEvents();
                    }
                    catch (NotImplementedException)
                    {
                        ConsoleHelper.PrintError("My Events is not available yet.");
                        ConsoleHelper.PressAnyKeyToContinue();
                    }
                    break;

                case "6":
                    try
                    {
                        new ProfileMenu(_userService, _eventService, _reviewService).Show();
                    }
                    catch (NotImplementedException)
                    {
                        ConsoleHelper.PrintError("My Profile is not available yet.");
                        ConsoleHelper.PressAnyKeyToContinue();
                    }
                    break;

                case "7":
                    _userService.Logout();
                    new AuthMenu(_userService, _eventService, _bookingService, _reviewService).Show();
                    return;

                default:
                    ConsoleHelper.PrintError("Invalid option. Please try again.");
                    ConsoleHelper.PressAnyKeyToContinue();
                    break;
            }
        }
    }
}