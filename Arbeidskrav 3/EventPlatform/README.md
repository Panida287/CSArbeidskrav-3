# EventPlatform Oslo

A ticket booking and event management platform built in C# (.NET 8) for the Norwegian market. Users can browse, search, and book tickets for concerts, conferences, and workshops — and leave reviews for events they have attended.

---

## Project Structure

```
EventPlatform/
├── EventPlatform.sln
├── .gitignore
│
├── EventPlatform/                  ← Main console app
│   ├── Models/
│   │   ├── Events/
│   │   │   ├── Event.cs            ← Abstract base class
│   │   │   ├── Concert.cs
│   │   │   ├── Conference.cs
│   │   │   └── Workshop.cs
│   │   ├── User.cs
│   │   ├── Booking.cs
│   │   ├── Review.cs
│   │   └── TicketType.cs
│   ├── Enums/
│   │   ├── EventCategory.cs        ← Music, Technology, Arts, Food, Sports, Education, Other
│   │   ├── EventStatus.cs          ← Upcoming, Completed, Cancelled
│   │   └── BookingStatus.cs        ← Confirmed, Cancelled
│   ├── Interfaces/
│   │   ├── IReviewable.cs
│   │   └── ISearchable.cs
│   ├── Services/                   ← Business logic only, no Console I/O
│   │   ├── UserService.cs
│   │   ├── EventService.cs
│   │   ├── BookingService.cs
│   │   └── ReviewService.cs
│   ├── Repositories/               ← All SQLite queries, one file per entity
│   │   ├── UserRepository.cs
│   │   ├── EventRepository.cs
│   │   ├── BookingRepository.cs
│   │   └── ReviewRepository.cs
│   ├── Data/
│   │   ├── AppDatabase.cs          ← Connection factory
│   │   └── DatabaseSeeder.cs       ← Runs schema.sql + seed.sql on first startup
│   ├── UI/
│   │   ├── ConsoleHelper.cs        ← Shared display utilities (headers, errors, dividers)
│   │   ├── Menus/
│   │   │   ├── AuthMenu.cs
│   │   │   ├── MainMenu.cs
│   │   │   ├── EventMenu.cs
│   │   │   ├── BookingMenu.cs
│   │   │   └── ReviewMenu.cs
│   │   └── Validators/
│   │       └── InputValidator.cs
│   ├── Utilities/
│   │   └── PasswordHelper.cs
│   └── Program.cs
│
├── EventPlatform.Tests/            ← xUnit tests
│   ├── BookingServiceTests.cs
│   ├── ReviewServiceTests.cs
│   └── SearchFilterTests.cs
│
└── docs/
    ├── requirements.md
    ├── project-plan.md
    ├── process-report.md
    ├── ai-prompts.md
    └── sql/
        ├── schema.sql              ← Run first — creates all 5 tables
        └── seed.sql                ← Norwegian-themed test data
```

> **Avalonia GUI** (`EventPlatform.Avalonia/`) is excluded from this phase. Begin only after the console app is fully working (Sprint 6).

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [JetBrains Rider](https://www.jetbrains.com/rider/) (recommended) — open `EventPlatform.sln` directly

### Run the console app

```bash
cd EventPlatform
dotnet run
```

On first run, `DatabaseSeeder` will create `eventplatform.db` and populate it using `docs/sql/schema.sql` and `docs/sql/seed.sql`.

### Run the tests

```bash
cd EventPlatform.Tests
dotnet test
```

---

## Architecture

The project follows a strict layered architecture. Each layer has one responsibility and one direction of dependency:

```
UI (Menus)  →  Services  →  Repositories  →  Database
```

- **Models** — pure data classes with no logic or I/O
- **Enums** — shared status and category values
- **Interfaces** — `IReviewable` and `ISearchable` contracts
- **Services** — all business rules; called by menus, call repositories
- **Repositories** — all SQL queries; one file per table
- **Data** — database connection factory and schema seeder
- **UI** — all `Console.WriteLine` / `Console.ReadLine` lives here only
- **Utilities** — stateless helpers (password hashing)

### Key rules

- Services and Repositories must never contain `Console` calls
- Menus must never contain SQL or business logic
- `ConsoleHelper` methods must be used for all output in menu classes (no raw `Console.WriteLine` scattered around)
- All public methods and properties must have XML doc comments

---

## Naming Conventions

| Element         | Convention                     | Example                  |
|-----------------|-------------------------------|--------------------------|
| Classes         | PascalCase                    | `EventService`           |
| Interfaces      | `I` prefix + PascalCase       | `IReviewable`            |
| Methods         | PascalCase                    | `GetUpcomingEvents()`    |
| Properties      | PascalCase                    | `BookingDate`            |
| Private fields  | `_camelCase`                  | `_currentUser`           |
| Local variables | camelCase                     | `selectedEvent`          |
| Enums           | PascalCase (type and values)  | `EventStatus.Upcoming`   |
| Constants       | PascalCase                    | `MaxRating`              |

---

## Console UI Conventions

- Every screen starts with `=== Title ===`
- Every screen ends with a numbered menu
- Input is always prefixed with `> `
- Use `ConsoleHelper.PrintError()` for all error messages — never throw stack traces at the user
- See the full screen layout reference in the design document

---

## Database

SQLite via `Microsoft.Data.Sqlite`. The database file (`eventplatform.db`) is excluded from version control (see `.gitignore`).

**Tables:** `Users`, `Events`, `TicketTypes`, `Bookings`, `Reviews`

Schema and seed data are in `docs/sql/`. `DatabaseSeeder` runs them automatically on startup if the database does not exist.

---

## Recommended NuGet Packages

| Package                  | Purpose                        |
|--------------------------|-------------------------------|
| `Microsoft.Data.Sqlite`  | SQLite database access         |
| `BCrypt.Net-Next`        | Password hashing               |
| `xunit`                  | Unit testing                   |

Add to `EventPlatform.csproj` via:

```bash
dotnet add package Microsoft.Data.Sqlite
dotnet add package BCrypt.Net-Next
```

---

## Sprint Overview

| Sprint | Focus                                    |
|--------|------------------------------------------|
| 1      | Models, Enums, Interfaces                |
| 2      | Repositories + Database setup            |
| 3      | Services + Console UI                    |
| 4      | Tests, documentation, process report     |
| 5–6    | Avalonia GUI (optional, after Sprint 3)  |

---

## Documentation

All documentation lives in `docs/` as `.md` files. No `.docx` files.

| File                | Contents                                      |
|---------------------|-----------------------------------------------|
| `requirements.md`   | 10+ user stories with acceptance criteria     |
| `project-plan.md`   | Task breakdown and sprint timeline            |
| `process-report.md` | Roles, philosophy, reflection (due Sprint 4)  |
| `ai-prompts.md`     | All AI prompts used — required by assignment  |
| `sql/schema.sql`    | Database schema — run first                  |
| `sql/seed.sql`      | Norwegian-themed test data — run second       |
