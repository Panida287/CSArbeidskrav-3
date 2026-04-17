# How to Read the UML Class Diagram

This guide explains how to read the class diagram for Event Platform Oslo.

---

## The Colours

Each colour represents a different type of class:

| Colour | Meaning | Classes |
|--------|---------|---------|
| Purple | Abstract base class | `Event` |
| Coral | Subclasses (inherit from Event) | `Concert`, `Conference`, `Workshop` |
| Teal | Interfaces | `IReviewable`, `ISearchable` |
| Blue | Supporting data classes | `User`, `Booking`, `TicketType`, `Review` |
| Gray | Enums | `EventCategory`, `EventStatus`, `BookingStatus` |

---

## The Symbols Inside Each Box

Every class box shows its properties using these symbols:

| Symbol | Meaning |
|--------|---------|
| `+` | Public — anyone can access this |
| `#` | Protected — only this class and its children can access this |
| `-` | Private — only this class can access this |

Example:
```
+ Title : string       ← public property called Title, type is string
# EventId : int        ← protected property called EventId, type is int
```

The word in italics at the top of the purple box (*Event*) and the label `«abstract»` means
that class cannot be created directly — you must use one of its subclasses (Concert, Conference, or Workshop).

The label `«interface»` on the teal boxes means those are contracts, not real classes.
Any class that implements an interface must provide the methods listed inside it.

The label `«enum»` on the gray boxes means those are a fixed list of allowed values,
not a class with properties.

---

## The Lines

| Line Type | Arrow Head | Meaning |
|-----------|-----------|---------|
| Solid line | Hollow triangle | **Inheritance** — the child class extends the parent |
| Dashed line | Hollow triangle | **Interface implementation** — the class must provide those methods |
| Dashed line | Filled arrow | **Association** — one class uses or references another |

---

## Reading the Relationships

### Inheritance (solid line, hollow triangle)
Concert, Conference, and Workshop all have a solid line pointing up to Event.
This means they **inherit** everything Event has — all those shared properties like
Title, Date, Venue — and then each one adds its own extra fields on top.

> Think of it as: Concert **is a** type of Event.

### Interface implementation (dashed line, hollow triangle)
Event has dashed lines pointing up to IReviewable and ISearchable.
This means Event **promises** to have those methods:
- `GetAverageRating()`, `GetReviews()` — from IReviewable
- `MatchesKeyword(keyword)` — from ISearchable

### Association (dashed line, filled arrow)
These show that one class **uses** another:
- `Booking` → `User` — a booking belongs to a user
- `Booking` → `TicketType` — a booking is for a specific ticket type
- `Review` → `Booking` — a review is linked to a booking
- `TicketType` → `Event` — a ticket type belongs to an event

---

## The Big Picture in Plain Words

> Event is the parent class. It holds all the shared fields every event needs —
> title, date, venue, status. Concert, Conference, and Workshop each inherit those
> fields and add their own on top (e.g. Concert adds Performer and Genre).
>
> Event also implements two interfaces, meaning it must support searching and reviewing.
>
> A User can make Bookings. Each Booking links to an Event and a TicketType.
> After attending, the User can leave a Review, which is linked back to their Booking.

---

*Document owned by: Project Manager — Last updated: April 2026*