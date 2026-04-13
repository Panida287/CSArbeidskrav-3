using EventPlatform.Services;

namespace EventPlatform.UI.Menus;

/// <summary>
/// Leave a review and View reviews screens.
/// </summary>
public class ReviewMenu
{
    private readonly ReviewService _reviewService;
    private readonly UserService _userService;

    public ReviewMenu(ReviewService reviewService, UserService userService)
    {
        _reviewService = reviewService;
        _userService = userService;
    }

    public void ShowLeaveReview(int bookingId) => throw new NotImplementedException();
    public void ShowEventReviews(int eventId) => throw new NotImplementedException();
}
