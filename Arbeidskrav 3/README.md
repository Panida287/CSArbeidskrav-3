# Event Platform Oslo

A console-based event management platform built in C# with SQLite.

---

## Group Members

| Name | GitHub | Role |
|------|--------|------|
| Panida | Panida287 | Project Manager |
| Aksel | AkselOldeide | Developer — Services & Testing |
| Christian | xghrgmax2089-web | Developer — Repositories & UI Logic |
| Laila | LailaElisabeth | Developer — Console UI & Menus |

---

## Project Description

Event Platform Oslo allows users to browse, search, and book tickets for events across three categories: Concerts, Conferences, and Workshops. Organisers can create and manage their own events. Attendees can leave reviews for events they have attended.

The application runs entirely in the console and stores all data in a local SQLite database.

---

## How to Build and Run

> Requirements: .NET 8 SDK installed

```bash
# Clone the repository
git clone https://github.com/your-org/event-platform-oslo.git
cd event-platform-oslo/EventPlatform

# Build the project
dotnet build

# Run the application
dotnet run

# Run tests
dotnet test
```

The database file `eventplatform.db` will be created automatically on first run with seed data loaded.

---

## Project Structure

```
EventPlatform/
├── Data/               # Database connection and seeder
├── Models/             # Domain model classes
│   ├── Events/         # Event, Concert, Conference, Workshop
│   └── ...             # User, Booking, Review, TicketType, enums
├── Repositories/       # SQLite queries
├── Services/           # Business logic
├── UI/                 # Console menus and helpers
└── Program.cs

EventPlatform.Tests/    # xUnit test suite
EventPlatform.Avalonia/ # GUI stretch goal
docs/                   # All project documentation
```

---

## Design Decisions

> *This section will be completed in Sprint 4.*

Topics to cover:
- Inheritance and abstract classes (Event hierarchy)
- Interfaces (IReviewable, ISearchable)
- Repository pattern for database separation
- Service layer for business logic
- Why SQLite was chosen over JSON persistence

---

## Documentation

- [Requirements & User Stories](docs/UserStory.md)
