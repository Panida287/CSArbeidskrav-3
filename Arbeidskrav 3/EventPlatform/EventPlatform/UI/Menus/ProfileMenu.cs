using EventPlatform.Services;

namespace EventPlatform.UI.Menus;

/// <summary>
/// Personal summary page for the logged-in user.
/// </summary>
public class ProfileMenu
{
    private readonly UserService _userService;
    private readonly EventService _eventService;
    private readonly ReviewService _reviewService;

    public ProfileMenu(UserService userService, EventService eventService, ReviewService reviewService)
    {
        _userService = userService;
        _eventService = eventService;
        _reviewService = reviewService;
    }

    public void Show()
    {
        var user = _userService.GetCurrentUser();
        
        ConsoleHelper.ClearAndPrintHeader($"My Profile - {user!.Username}");
        ConsoleHelper.PrintDivider();

        ShowOrganisedEvents(user.UserId);
        ShowReceivedReviews(user.UserId);
        
        ConsoleHelper.PrintDivider();
        Console.WriteLine(" 0. Go back");
        ConsoleHelper.PrintDivider();
        Console.Write("Choose an option: ");
        Console.ReadLine();
    }

    private void ShowOrganisedEvents(int userId)
    {
        ConsoleHelper.PrintHeader("Events I Organise");
        ConsoleHelper.PrintDivider();

        try
        {
            var events = _eventService.GetAll()
                .Where(e => e.OrganiserId == userId)
                .ToList();

            if (events.Count == 0)
            {
                ConsoleHelper.PrintError("No events organised yet.");
                return;
            }
            
            //Print column headers
            //TODO: Add Bookings/Capacity column when BookingService.GetByEvent() is available
            Console.WriteLine(
                " #".PadRight(4) +
                "Title".PadRight(26) +
                "Date".PadRight(14) +
                "Status"
                );
            ConsoleHelper.PrintDivider();
            
            //Print each event
            for (int i = 0; i < events.Count; i++)
            {
                var e = events[i];
                string date = e.EventDate.ToString("yyyy-MM-dd");
                string status = e.Status.ToString();

                Console.WriteLine(
                    $"{i + 1}".PadRight(4) +
                    e.Title.PadRight(26) +
                    date.PadRight(14) +
                    status
                    );
            }
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Events data is not available yet.");
        }
    }

    private void ShowReceivedReviews(int userId)
    {
        ConsoleHelper.PrintHeader("Reviews I Have Received");
        ConsoleHelper.PrintDivider();

        try
        {
            //Get all events organised by this user
            var myEvents = _eventService.GetAll()
                .Where(e => e.OrganiserId == userId)
                .ToList();

            if (myEvents.Count == 0)
            {
                ConsoleHelper.PrintError("No reviews received yet.");
                return;
            }
            
            //Collects all reviews across all my events
            var allReviews = new List<(string EventTitle, int Rating, string Comment)>();

            foreach (var ev in myEvents)
            {
                var reviews = _reviewService.GetReviews(ev.EventId);
                foreach (var r in reviews)
                {
                    allReviews.Add((ev.Title, r.Rating, r.Comment));
                }
            }

            if (allReviews.Count == 0)
            {
                ConsoleHelper.PrintError("No reviews received yet.");
                return;
            }
            
            //Print each review
            foreach (var (eventTitle, rating, comment) in allReviews)
            {
                string stars = new string('★', rating).PadRight(5);
                string excerpt = comment.Length > 30
                    ? comment[..30] + "..."
                    : comment;

                Console.WriteLine($" {stars} \"{excerpt}\" - {eventTitle}");
            }
            
            //Calculate and show average rating
            double average = allReviews.Average(r => r.Rating);
            ConsoleHelper.PrintDivider();
            Console.WriteLine($" Your average rating: ★ {average:F1}");
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Reviews data is not available yet.");
        }
    }
}