namespace EventPlatform.Models;

/// <summary>
/// Represents a ticket tier for an event, e.g. Early Bird, Standard, or VIP.
/// </summary>
public class TicketType
{
    /// <summary>Unique identifier for the ticket type.</summary>
    public int TicketTypeId { get; set; }

    /// <summary>ID of the event this ticket type belongs to.</summary>
    public int EventId { get; set; }

    /// <summary>Display name of the ticket tier, e.g. "Early Bird".</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Price of this ticket in NOK.</summary>
    public decimal Price { get; set; }

    /// <summary>Total number of tickets available when the event was created.</summary>
    public int TotalQuantity { get; set; }

    /// <summary>Number of tickets still available for purchase.</summary>
    public int Remaining { get; set; }
}
