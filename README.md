# Event Platform Oslo

A console-based event management platform built in C# with SQLite.

---

## Group Members

| Name | GitHub | Role |
|------|--------|------|
| Panida | Panida287 | Project Manager — documentation, setup, integration, final review |
| Aksel | AkselOldeide | Developer — services, business logic, testing |
| Christian | xghrgmax2089-web | Developer — repositories, database, UI logic |
| Laila | LailaElisabeth | Developer — console UI, menus, display layer |

---

## Project Description

Event Platform Oslo allows users to browse, search, and book tickets for events across three types: Concerts, Conferences, and Workshops. Organisers can create and manage their own events. Attendees can leave reviews for events they have attended.

The application runs entirely in the console and stores all data in a local SQLite database. All data persists between sessions — creating an account, booking a ticket, or leaving a review will still be there when the app is restarted.

---

## How to Build and Run

> Requirements: .NET 8 SDK installed

```bash
# Clone the repository
git clone https://github.com/Panida287/CSArbeidskrav-3.git
cd CSArbeidskrav-3/Arbeidskrav\ 3/EventPlatform

# Build the project
dotnet build

# Run the application
dotnet run
```

The database file `eventplatform.db` is created automatically on first run. Seed data is loaded on startup — you can log in immediately with the test accounts from `seed.sql`.

> **Note on unit tests:** The `EventPlatform.Tests` project exists in the solution but unit tests were not completed. Given the scale of the project, the team chose to rely on thorough manual testing throughout development instead. All features were manually verified before submission.

---

## Project Structure

```
EventPlatform/
├── Data/                   # Database connection and seeder
├── Enums/                  # EventCategory, EventStatus, BookingStatus
├── Interfaces/             # IReviewable, ISearchable
├── Models/
│   ├── Events/             # Event (abstract), Concert, Conference, Workshop
│   └── ...                 # User, Booking, Review, TicketType, BookingDetail
├── Repositories/           # SQLite queries — one file per entity
├── Services/               # Business logic — one file per domain
├── UI/
│   ├── ConsoleHelper.cs    # Shared display utilities
│   ├── Menus/
│   │   ├── Events/         # EventBrowseMenu, EventCreateMenu, EventDetailMenu, EventManageMenu
│   │   ├── AuthMenu.cs
│   │   ├── BookingMenu.cs
│   │   ├── MainMenu.cs
│   │   ├── ProfileMenu.cs
│   │   └── ReviewMenu.cs
│   └── Validators/         # InputValidator
├── Utilities/              # PasswordHelper (PBKDF2 hashing)
└── Program.cs

EventPlatform.Tests/        # xUnit test suite
docs/                       # All project documentation
```

---

## Design Decisions

### Inheritance and Abstract Classes
`Event` is an abstract base class that cannot be instantiated directly. `Concert`, `Conference`, and `Workshop` inherit from it and each add type-specific fields (e.g. `Performer` and `Genre` for concerts, `SpeakerList` for conferences). This models the real-world "is-a" relationship and allows all event types to be handled through a single `List<Event>`.

### Interfaces
`IReviewable` and `ISearchable` define shared contracts across types without forcing a specific inheritance chain. This keeps the design flexible and demonstrates polymorphism through interfaces.

### Repository Pattern
All SQLite queries are isolated in repository classes (`EventRepository`, `BookingRepository`, etc.). Services never write raw SQL — they call repository methods. This separation makes the code easier to test and easier to swap the database layer if needed.

### Service Layer
Business rules live exclusively in service classes (`EventService`, `BookingService`, `ReviewService`, `UserService`). No menu or UI class makes data decisions — it only displays results and passes input to a service. This enforces separation of concerns.

### Password Security
Passwords are hashed using PBKDF2-SHA256 with 100,000 iterations and a cryptographically secure random salt. Passwords are never stored or printed in plain text anywhere in the application.

### SQLite for Persistence
SQLite was chosen because it requires no server setup, works cross-platform, and fits the scope of a console application. The database file is created and seeded automatically on first run.

### Auto-Completing Past Events
`EventService.GetAll()` and `GetById()` automatically update any event whose date has passed from `Upcoming` to `Completed`. This means the "Leave a Review" option appears correctly for attendees without any manual database changes.

---

## Documentation

| File | Description |
|------|-------------|
| [Requirements & User Stories](Arbeidskrav%203/docs/UserStory.md) | User stories with acceptance criteria |
| [GitHub Project Board](https://github.com/users/Panida287/projects/14/views/3) | Task breakdown and sprint timeline |
| [Process Report](Arbeidskrav%203/docs/process-report.md) | Team reflection and process review |
| [UML Class Diagram](Arbeidskrav%203/docs/uml-class-diagram.html) | UML class diagram |
| [File Structure & UI Guide](Arbeidskrav%203/docs/file-structure-and-ui-guide.md) | File structure and UI design reference |
| [AI Prompts Log](Arbeidskrav%203/docs/ai-prompts.md) | All AI prompts used in this project |
| [Database Schema](Arbeidskrav%203/docs/sql/schema.sql) | Database schema |
| [Seed Data](Arbeidskrav%203/docs/sql/seed.sql) | Seed data |
