namespace EventPlatform.Enums;

/// <summary>
/// Status of a user's booking.
/// </summary>
public enum BookingStatus
{
    /// <summary>The booking is active and valid.</summary>
    Confirmed,

    /// <summary>The booking was cancelled by the user.</summary>
    Cancelled
}
