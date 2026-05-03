# Event Management Platform — File Structure & UI Design Guide

> Reference document for the development team. Updated: April 2026.

---

## Table of Contents

1. [Solution File Structure](#1-solution-file-structure)
2. [Namespace & Class Map](#2-namespace--class-map)
3. [Console UI Design](#3-console-ui-design)
4. [Avalonia UI Design](#4-avalonia-ui-design)
5. [Naming Conventions](#5-naming-conventions)

---

## 1. Solution File Structure

```
EventPlatform/                          ← Root solution folder
│
├── EventPlatform.sln                   ← Solution file
├── .gitignore                          ← VisualStudio template + .db files excluded
│
├── EventPlatform/                      ← Main console app project
│   ├── EventPlatform.csproj
│   │
│   ├── Models/                         ← Pure data classes, no Console I/O here
│   │   ├── Events/
│   │   │   ├── Event.cs                ← Abstract base class
│   │   │   ├── Concert.cs              ← Derived: adds Performer, Genre
│   │   │   ├── Conference.cs           ← Derived: adds SpeakerList, SessionSchedule
│   │   │   └── Workshop.cs             ← Derived: adds RequiredMaterials, MaxParticipants
│   │   ├── User.cs
│   │   ├── Booking.cs
│   │   ├── Review.cs
│   │   └── TicketType.cs
│   │
│   ├── Enums/                          ← All enums live here
│   │   ├── EventCategory.cs            ← Music, Technology, Arts, Food, Sports, Education, Other
│   │   ├── EventStatus.cs              ← Upcoming, Completed, Cancelled
│   │   └── BookingStatus.cs            ← Confirmed, Cancelled
│   │
│   ├── Interfaces/                     ← Shared contracts
│   │   ├── IReviewable.cs              ← GetAverageRating(), GetReviews()
│   │   └── ISearchable.cs              ← MatchesKeyword(string keyword)
│   │
│   ├── Services/                       ← Business logic only, no Console I/O here
│   │   ├── UserService.cs              ← Register, Login, Logout, GetCurrentUser
│   │   ├── EventService.cs             ← Create, Edit, Cancel, GetAll, GetById, Filter
│   │   ├── BookingService.cs           ← BookTicket, CancelBooking, GetUserBookings
│   │   └── ReviewService.cs            ← AddReview, GetReviews, IsEligible, GetAverageRating
│   │
│   ├── Repositories/                   ← All SQLite queries live here, one file per entity
│   │   ├── UserRepository.cs           ← Insert, FindByUsername, Exists
│   │   ├── EventRepository.cs          ← Insert, Update, UpdateStatus, GetAll, GetById (with JOINs)
│   │   ├── BookingRepository.cs        ← Insert, UpdateStatus, GetByUser, Decrement/IncrementRemaining
│   │   └── ReviewRepository.cs         ← Insert, GetByEvent, ExistsForBooking
│   │
│   ├── Data/                           ← Database setup and connection
│   │   ├── AppDatabase.cs              ← Connection factory — inject into repositories
│   │   └── DatabaseSeeder.cs           ← Runs schema.sql + seed.sql on first startup
│   │
│   ├── UI/                             ← All Console.WriteLine / ReadLine lives here
│   │   ├── ConsoleHelper.cs            ← Shared display utilities (headers, dividers, errors)
│   │   ├── Menus/
│   │   │   ├── AuthMenu.cs             ← Register / Login / Welcome screen
│   │   │   ├── MainMenu.cs             ← Post-login navigation hub
│   │   │   ├── EventMenu.cs            ← Browse, Search, Filter, Event Detail, Create, My Events
│   │   │   ├── BookingMenu.cs          ← Book a ticket, My Bookings, Cancel booking
│   │   │   └── ReviewMenu.cs           ← Leave a review, View reviews
│   │   └── Validators/
│   │       └── InputValidator.cs       ← GetValidInt, GetNonEmptyString, GetFutureDate, GetValidPrice
│   │
│   ├── Utilities/
│   │   └── PasswordHelper.cs           ← Hash(password), Verify(password, hash)
│   │
│   └── Program.cs                      ← Entry point — wires database, services, and menus
│
├── EventPlatform.Tests/                ← xUnit test project (Aksel)
│   ├── EventPlatform.Tests.csproj
│   ├── BookingServiceTests.cs          ← Capacity, ownership, cancellation rules
│   ├── ReviewServiceTests.cs           ← Eligibility rules, average rating
│   └── SearchFilterTests.cs            ← Keyword search, category filter, type filter
│
├── EventPlatform.Avalonia/             ← Avalonia GUI — only start after Sprint 3 is done
│   ├── EventPlatform.Avalonia.csproj
│   ├── App.axaml
│   ├── App.axaml.cs
│   ├── Assets/
│   │   └── icon.ico
│   ├── ViewModels/
│   │   ├── MainWindowViewModel.cs      ← ContentControl navigation with DataTemplates
│   │   ├── LoginViewModel.cs
│   │   ├── EventListViewModel.cs
│   │   ├── EventDetailViewModel.cs
│   │   └── BookingViewModel.cs
│   ├── Views/
│   │   ├── MainWindow.axaml
│   │   ├── LoginView.axaml
│   │   ├── EventListView.axaml
│   │   ├── EventDetailView.axaml
│   │   └── BookingView.axaml
│   └── Converters/
│       └── StatusColorConverter.cs     ← e.g. Upcoming → green, Cancelled → red
│
└── docs/                               ← All documentation (no .docx — .md or .pdf only)
    ├── requirements.md
    ├── uml-class-diagram.png
    ├── project-plan.md
    ├── process-report.md               ← Due Sprint 4
    ├── ai-prompts.md
    ├── file-structure-and-ui-guide.md  ← This file
    └── sql/
        ├── schema.sql
        └── seed.sql
```

---

## 2. Namespace & Class Map

```
EventPlatform.Models.Events     → Event, Concert, Conference, Workshop
EventPlatform.Models            → User, Booking, Review, TicketType
EventPlatform.Enums             → EventCategory, EventStatus, BookingStatus
EventPlatform.Interfaces        → IReviewable, ISearchable
EventPlatform.Services          → UserService, EventService, BookingService, ReviewService
EventPlatform.UI.Menus          → MainMenu, AuthMenu, EventMenu, BookingMenu, ReviewMenu
EventPlatform.UI.Validators     → InputValidator
EventPlatform.UI                → ConsoleHelper
EventPlatform.Data              → AppDatabase, DatabaseSeeder
EventPlatform.Utilities         → PasswordHelper
```

---

## 3. Console UI Design

### 3.1 General Principles

- Every screen starts with a header using `=== Title ===` format
- Every screen ends with a numbered menu for navigation
- Input is always on a new line after a `>` prompt
- Errors are shown inline, never as stack traces
- The user is never left on a blank screen

### 3.2 ConsoleHelper — Shared Utilities

```csharp
ConsoleHelper.PrintHeader("Browse Events");
ConsoleHelper.PrintDivider();
ConsoleHelper.PrintSuccess("Booking confirmed!");
ConsoleHelper.PrintError("Invalid input. Please try again.");
ConsoleHelper.PressAnyKeyToContinue();
```

Output examples:

```
=== Browse Events ===
------------------------------------------------------------
[✓] Booking confirmed!
[✗] Invalid input. Please try again.

Press any key to continue...
```

### 3.3 Screen Layouts

---

#### SCREEN: Welcome / Not Logged In

```
============================================================
         Welcome to EventPlatform Oslo
============================================================

  1. Register
  2. Log In
  3. Exit

> _
```

---

#### SCREEN: Register

```
=== Create an Account ===

  Username : _
  Password : (hidden)
  Confirm  : (hidden)

[✓] Account created! You are now logged in as "anna".
```

---

#### SCREEN: Main Menu (Logged In)

```
============================================================
  Logged in as: anna
============================================================

  1. Browse Events
  2. Search Events
  3. My Bookings
  4. Create Event
  5. My Events
  6. My Profile
  7. Log Out

> _
```

---

#### SCREEN: Browse Events

```
=== Upcoming Events ===

  #   Title                        Type          Date           Tickets
  --  ---------------------------  ------------  -------------  ----------
  1   Nordic Dev Summit            Conference    15 Apr 2026    Available
  2   The Midnight Strings         Concert       22 Apr 2026    Sold Out
  3   Intro to Watercolour         Workshop      28 Apr 2026    Available
  4   Jazz & Wine Evening          Concert       02 May 2026    Available

  F. Filter by category
  S. Search
  0. Back

> _
```

---

#### SCREEN: Filter Menu

```
=== Filter Events ===

  Filter by:
  1. Category
  2. Event Type
  0. Back

> _

--- Filter by Category ---
  1. Music
  2. Technology
  3. Food & Drink
  4. Arts & Culture
  5. Other
  0. Cancel

> _
```

---

#### SCREEN: Search

```
=== Search Events ===

  Enter keyword (title, description, or venue):
> oslo

  Results for "oslo":
  #   Title                        Type          Date
  1   Nordic Dev Summit            Conference    15 Apr 2026
  2   Oslo Jazz Festival           Concert       01 May 2026

  Select an event to view (0 to go back):
> _
```

---

#### SCREEN: Event Detail

```
=== Nordic Dev Summit ===

  Organiser  : techevents
  Type       : Conference
  Category   : Technology
  Date       : 15 April 2026, 09:00
  Venue      : DogA, Oslo
  Status     : Upcoming
  Rating     : ★ 4.8 / 5 (12 reviews)

  Description:
  A full-day conference on modern backend development.
  Includes talks, hands-on sessions, and networking lunch.

  Ticket Types:
  1. Early Bird   —  350 kr    (8 remaining)
  2. Standard     —  500 kr    (34 remaining)
  3. VIP          —  950 kr    (includes speaker dinner, 6 remaining)

  Options:
  1. Book a ticket
  2. View reviews
  0. Back

> _
```

---

#### SCREEN: Book a Ticket

```
=== Book a Ticket — Nordic Dev Summit ===

  Select ticket type:
  1. Early Bird   —  350 kr
  2. Standard     —  500 kr
  3. VIP          —  950 kr
  0. Cancel

> 2

  --- Booking Summary ---
  Event  : Nordic Dev Summit
  Ticket : Standard — 500 kr
  Date   : 15 April 2026

  Confirm booking? (Y/N)
> Y

[✓] Booking confirmed!
  Reference : BK-00142
  Booked    : 29 March 2026

  Payment is handled externally. Enjoy the event!

  Press any key to continue...
```

---

#### SCREEN: My Bookings

```
=== My Bookings ===

  --- Upcoming ---
  #   Event                  Date           Ticket      Price   Ref
  1   Nordic Dev Summit      15 Apr 2026    Standard    500 kr  BK-00142

  --- Past ---
  #   Event                  Date           Ticket      Status
  2   Oslo Tech Meetup       10 Mar 2026    Standard    Attended
  3   Jazz Evening           01 Feb 2026    VIP         Cancelled

  Select a booking to manage (0 to go back):
> _
```

---

#### SCREEN: Leave a Review

```
=== Leave a Review — Oslo Tech Meetup ===

  Rating (1–5): _
  Comment (optional, press Enter to skip): _

[✓] Review submitted. Thank you!

  Press any key to continue...
```

---

#### SCREEN: Create Event

```
=== Create New Event ===

  Event Type:
  1. Concert
  2. Conference
  3. Workshop
  0. Cancel

> 2

  --- New Conference ---
  Title       : _
  Description : _
  Category    : (1) Technology  (2) Music  (3) Food & Drink  (4) Arts  (5) Other
              > _
  Date        : (dd/mm/yyyy) _
  Time        : (hh:mm) _
  Venue       : _

  --- Ticket Types (add at least one) ---
  Ticket name : _
  Price (kr)  : _
  Quantity    : _
  Add another ticket type? (Y/N): _

  --- Conference Details ---
  Speaker 1   : _
  Speaker 2   : (press Enter to skip) _

[✓] Event created successfully!

  Press any key to continue...
```

---

#### SCREEN: My Profile

```
=== My Profile — anna ===

  --- Events I Organise ---
  #   Title                  Date           Status     Bookings
  1   Nordic Dev Summit      15 Apr 2026    Upcoming   42 / 48

  --- Reviews I Have Received ---
  ★ 5 — "Excellent organisation, smooth check-in." (Nordic Dev Summit)
  ★ 4 — "Good content, room was a bit cold." (Oslo Tech Meetup)

  Average as organiser: ★ 4.5

  0. Back

> _
```

---

### 3.4 Error & Edge Case Messages

```
[✗] You cannot book your own event.
[✗] This event is sold out.
[✗] You have already reviewed this event.
[✗] You can only review events you have attended.
[✗] Only the organiser can cancel this event.
[✗] Please enter a valid date (dd/mm/yyyy).
[✗] Price must be greater than 0.
[✗] Username already taken. Please choose another.
[✗] Incorrect username or password.
```

---

## 4. Avalonia UI Design

> Only begin this after the console app is fully working. Target: Sprint 4 stretch goal.

### 4.1 Architecture — MVVM

```
View (.axaml)  →  binds to  →  ViewModel (.cs)  →  calls  →  Service (.cs)
LoginView                       LoginViewModel              UserService
EventListView                   EventListViewModel          EventService
EventDetailView                 EventDetailViewModel        BookingService
BookingView                     BookingViewModel            ReviewService
```

- **Views** contain only layout and bindings — no logic
- **ViewModels** contain all state and commands — no UI controls
- **Services** are shared with the console project — import `EventPlatform` as a project reference

### 4.2 Navigation Pattern

Use a single `MainWindow` with a `ContentControl` that swaps views based on the current ViewModel:

```xml
<!-- MainWindow.axaml -->
<ContentControl Content="{Binding CurrentPage}">
    <ContentControl.DataTemplates>
        <DataTemplate DataType="{x:Type vm:LoginViewModel}">
            <views:LoginView />
        </DataTemplate>
        <DataTemplate DataType="{x:Type vm:EventListViewModel}">
            <views:EventListView />
        </DataTemplate>
    </ContentControl.DataTemplates>
</ContentControl>
```

### 4.3 Suggested NuGet Packages

```xml
<PackageReference Include="Avalonia" Version="11.*" />
<PackageReference Include="Avalonia.Desktop" Version="11.*" />
<PackageReference Include="Avalonia.Themes.Fluent" Version="11.*" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.*" />
```

`CommunityToolkit.Mvvm` gives you `[ObservableProperty]`, `[RelayCommand]`, and `ObservableObject` — cuts ViewModel boilerplate in half.

---

## 5. Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `EventService`, `TicketType` |
| Interfaces | `I` prefix + PascalCase | `IReviewable` |
| Methods | PascalCase | `GetUpcomingEvents()` |
| Properties | PascalCase | `BookingDate` |
| Private fields | `_camelCase` | `_currentUser` |
| Local variables | camelCase | `selectedEvent` |
| Enums | PascalCase (type and values) | `EventStatus.Upcoming` |
| Constants | PascalCase | `MaxRating` |
| AXAML files | PascalCase + suffix | `EventListView.axaml` |
| ViewModel files | PascalCase + `ViewModel` | `EventListViewModel.cs` |

All public methods and properties must have XML doc comments:

```csharp
/// <summary>
/// Returns all upcoming events matching the given keyword.
/// </summary>
/// <param name="keyword">Case-insensitive search term.</param>
/// <returns>Filtered list of upcoming events.</returns>
public List<Event> SearchEvents(string keyword) { ... }
```

---

*Document owned by: Project Manager — Last updated: April 2026*