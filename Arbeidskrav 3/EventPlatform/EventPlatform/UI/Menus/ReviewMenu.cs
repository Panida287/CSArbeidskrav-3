using System;
using System.Collections.Generic;
using EventPlatform.Services;
using EventPlatform.Models;

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

    public void ShowLeaveReview(int bookingId)
    {
        ConsoleHelper.ClearAndPrintHeader("Leave a Review");
        ConsoleHelper.PrintDivider();

        var user = _userService.GetCurrentUser();

        if (user == null)
        {
            ConsoleHelper.PrintError("You must be logged in to leave a review.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }
        
        //Check eligibility before asking for input
        //This handles: own event, already reviewed, event not completed
        try
        {
            bool eligible = _reviewService.IsEligible(user.UserId, bookingId);

            if (!eligible)
            {
                ConsoleHelper.PrintError("You are not eligible to review this event.");
                ConsoleHelper.PressAnyKeyToContinue();
                return;
            }
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Review eligibility check is not available yet.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }
        
        //Ask for rating
        int rating = 0;

        while (true)
        {
            Console.Write("Rating (1-5): ");
            string input = Console.ReadLine() ?? "";
            
            if (int.TryParse(input, out rating) && rating >= 1 && rating <= 5)
                break;
            
            ConsoleHelper.PrintError("Rating must be a number between 1 and 5.");
        }
        
        //Ask for a comment - optional
        Console.Write("Comment (optional, press Enter to skip): ");
        string comment = Console.ReadLine() ?? "";
        
        //Submit the review
        try
        {
            bool success = _reviewService.AddReview(user.UserId, bookingId, rating, comment);

            if (success)
            {
                ConsoleHelper.PrintSuccess("Review submitted. Thank you!");
            }
            else
            {
                ConsoleHelper.PrintError("Could not submit review. Please check eligibility.");
            }
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Review submission is not available yet.");
        }
        ConsoleHelper.PressAnyKeyToContinue();
    }

    public void ShowEventReviews(int eventId)
    {
        ConsoleHelper.ClearAndPrintHeader("Event Reviews");
        ConsoleHelper.PrintDivider();

        try
        {
            //Get reviews and average rating
            var reviews = _reviewService.GetReviews(eventId);
            double? average = _reviewService.GetAverageRating(eventId);

            if (reviews.Count == 0)
            {
                Console.WriteLine("No reviews yet.");
            }
            else
            {
                string reviewWord = reviews.Count == 1 ? "review" : "reviews";
                string star = "★";
                int count = reviews.Count;

                Console.WriteLine($"{star} {average:F1} / 5 ({count} {reviewWord})");
                ConsoleHelper.PrintDivider();

                foreach (var r in reviews)
                {
                    string stars = new string('★', r.Rating).PadRight(5);
                    string reviewComment = string.IsNullOrWhiteSpace(r.Comment)
                        ? "no comment)"
                        : r.Comment;

                    Console.WriteLine($" {stars} {reviewComment}");
                }
            }
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Reviews are not available yet.");
        }
    }
}
