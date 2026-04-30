using System;
using System.Collections.Generic;
using EventPlatform.Enums;
using EventPlatform.Services;
using EventPlatform.Models;
using EventPlatform.Models.Events;

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

    public void ShowLeaveReview(Booking booking, Event ev)
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

        //Check 1: event must be completed
        if (ev.Status != EventStatus.Completed)
        {
            ConsoleHelper.PrintError("This event has not yet completed.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        //Check 2: user cannot review their own event
        if (user.UserId == ev.OrganiserId)
        {
            ConsoleHelper.PrintError("You cannot review your own event.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        //Check 3: user hasn't already reviewed this booking
        bool eligible = _reviewService.IsEligible(user, ev, booking);
        if (!eligible)
        {
            ConsoleHelper.PrintError("You have already reviewed this booking.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        //Ask for rating - re-prompt if outside 1-5
        int rating = 0;

        while (true)
        {
            Console.Write("Rating (1-5): ");
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out rating) && rating >= 1 && rating <= 5)
                break;

            ConsoleHelper.PrintError("Rating must be a number between 1 and 5.");
        }

        //Ask for a comment - optional, pressing Enter skips it
        Console.Write("Comment (optional, press Enter to skip): ");
        string comment = Console.ReadLine() ?? "";

        //Submit the review
        bool success = _reviewService.AddReview(user, ev, booking, rating, comment);

        if (success)
        {
            ConsoleHelper.PrintSuccess("Review submitted. Thank you!");
        }
        else
        {
            ConsoleHelper.PrintError("Could not submit review. Please try again.");
        }

        ConsoleHelper.PressAnyKeyToContinue();
    }

    public void ShowEventReviews(int eventId)
    {
        ConsoleHelper.ClearAndPrintHeader("Event Reviews");
        ConsoleHelper.PrintDivider();

        try
        {
            var reviews = _reviewService.GetReviewsForEvent(eventId);
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

                Console.WriteLine($" {star} {average:F1} / 5 ({count} {reviewWord})");
                ConsoleHelper.PrintDivider();

                foreach (var r in reviews)
                {
                    string stars = new string('★', r.Rating).PadRight(5);
                    string reviewComment = string.IsNullOrWhiteSpace(r.Comment)
                        ? "(no comment)"
                        : r.Comment;

                    Console.WriteLine($" {stars} {reviewComment}");
                }
            }
        }
        catch (NotImplementedException)
        {
            ConsoleHelper.PrintError("Reviews are not available yet.");
        }

        ConsoleHelper.PrintDivider();
        Console.WriteLine(" 0. Go back");
        ConsoleHelper.PrintDivider();
        Console.Write("Choose an option: ");
        Console.ReadLine();
    }
}