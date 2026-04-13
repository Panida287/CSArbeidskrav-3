namespace EventPlatform.Models;

/// <summary>
/// Represents a ticket tier for an event (e.g. Early Bird, Standard, VIP).
/// </summary>
public class TicketType
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int RemainingTickets { get; set; }
}
