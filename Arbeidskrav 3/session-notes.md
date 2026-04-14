# Session Notes — EventPlatform Setup & Models
**Date:** April 13–14, 2026
**Issues covered:** #1 Project & Repository Initialisation, #3 Data Models

---

## What we did

Set up the full project foundation for EventPlatform Oslo from scratch — folder structure, all stub files, database layer, build verification, and all data model classes with correct properties and documentation.

---

## Part 1 — Project structure and file scaffold

### What
Created the entire folder and file structure for three projects:
- `EventPlatform` — main console app
- `EventPlatform.Tests` — xUnit test project
- `EventPlatform.Avalonia` — GUI stub (added to satisfy the issue requirement, will not be touched until much later)

Along with all supporting files: `.gitignore`, `EventPlatform.sln`, `README.md`, and a `docs/` folder containing `requirements.md`, `project-plan.md`, `process-report.md`, `ai-prompts.md`, `schema.sql`, and `seed.sql`.

### How
Used `mkdir -p` to create all directories in one go, then used heredoc (`cat > file << 'EOF'`) to write each file's content directly from the terminal. Every `.cs` file was created as a stub — correct namespace, XML doc comments, and method signatures that throw `NotImplementedException` so the project compiles cleanly without needing any real logic yet.

### Why
Starting with a complete structure means the whole team can clone the repo and immediately see where everything belongs. Stubs with `NotImplementedException` are better than empty files because they make the compiler aware of the methods, so you get a clear error when something unimplemented is called rather than a mysterious crash.

---

## Part 2 — Target framework and NuGet package

### What
Updated both `EventPlatform.csproj` and `EventPlatform.Tests.csproj` from `net8.0` to `net10.0`, and added `Microsoft.Data.Sqlite` as a NuGet package.

### Why
The machine has .NET 10 installed, not .NET 8. They work together but it's cleaner to match them. `Microsoft.Data.Sqlite` is the library that lets C# talk to a SQLite database — without it, `AppDatabase.cs` can't compile.

### How
```bash
dotnet add "Arbeidskrav 3/EventPlatform/EventPlatform/EventPlatform.csproj" package Microsoft.Data.Sqlite
```

**Problem — wrong path**
The first attempt used `EventPlatform/EventPlatform.csproj` but the folder name `Arbeidskrav 3` has a space in it, so the full path needs quotes. Fixed by wrapping the entire path in double quotes.

---

## Part 3 — Implementing AppDatabase and DatabaseSeeder

### What
Replaced the stubs in `AppDatabase.cs` and `DatabaseSeeder.cs` with real working code, and updated `Program.cs` to wire everything together.

**AppDatabase.cs** opens a SQLite connection using the path `eventplatform.db`.

**DatabaseSeeder.cs** checks if the `Users` table already exists. If not, it reads `schema.sql` and `seed.sql` from `docs/sql/` and runs them. Prints `[DB] Test data loaded.` on success. On any run after the first it prints a skip message instead.

**Program.cs** passes `../docs/sql` as the SQL folder path because `dotnet run` runs from inside the `EventPlatform/` project folder, so it needs to go one level up to find `docs/`.

### Why
The acceptance criteria for Issue #1 required the app to create and populate `eventplatform.db` automatically on first run with no manual steps.

### Problem — .NET not found
**Error:** `zsh: command not found: dotnet`
**Fix:** Downloaded and installed .NET 10 SDK from dotnet.microsoft.com/download.

---

## Part 4 — Avalonia stub project

### What
Added an empty `EventPlatform.Avalonia` project and updated `EventPlatform.sln` to reference all three projects.

### Why
Issue #1 required all three projects to be in the solution. The Avalonia project is completely empty — it's just there to satisfy the checklist. The team will not touch it until the console app is fully working.

**Note for the team:** Don't worry about Avalonia at all right now. The plan is to build and finish the console app first across Sprints 1–4, then revisit the GUI later. Everything in `EventPlatform.Avalonia/` can be ignored.

### Problems encountered

**Error 1:** `error CS5001: Program does not contain a static 'Main' method`
**Cause:** The project was set as a runnable app (`WinExe`) but had no entry point.
**Fix:** Changed `OutputType` to `Library` so it compiles as a class library instead of an executable. Added a TODO to change it back in Sprint 6.

**Error 2:** `Avalonia error AVLN2100: Cannot parse a compiled binding without x:DataType`
**Cause:** Compiled bindings were turned on but the stub views don't specify data types.
**Fix:** Turned compiled bindings off for now. Added a TODO to re-enable in Sprint 6.

---

## Part 5 — Data model classes (Issue #3)

### What
Wrote all 13 model files with correct property names, types, and XML doc comments on every property. Updated from the initial stubs to match the issue spec exactly.

### Why models are in their own folder with no logic
The `Models/` folder contains pure data — just properties describing what a thing is. No database queries, no console output, no business rules. This separation means any part of the app (services, menus, tests) can use the same model class without pulling in unwanted dependencies.

---

### Why Event is abstract

`Event` is declared as `abstract` because you can never have "just an event" — every event in the real world is *specifically* a Concert, a Conference, or a Workshop. Abstract prevents anyone from writing `new Event()` by mistake.

```csharp
// This is NOT allowed — compiler error:
var e = new Event();

// This IS allowed:
var e = new Concert();
var e = new Conference();
var e = new Workshop();
```

`Concert`, `Conference`, and `Workshop` all inherit from `Event`, which means they automatically get all the shared properties (title, date, venue, etc.) and then add their own on top. You only write the shared stuff once.

```
Event (abstract)
├── EventId, OrganiserId, Title, Description
├── EventType, Category, EventDate, Venue, Status
│
├── Concert       → + Performer, Genre
├── Conference    → + SpeakerList, SessionSchedule
└── Workshop      → + RequiredMaterials, MaxParticipants
```

---

### What the models look like as database tables

Each model class maps directly to a table in the SQLite database. Here's what the data looks like:

**Users**

| UserId | Username | PasswordHash | CreatedAt |
|--------|----------|--------------|-----------|
| 1 | anna | $2a$12$... | 2026-03-01 |
| 2 | techevents | $2a$12$... | 2026-03-02 |

**Events**

| EventId | OrganiserId | Title | EventType | Category | EventDate | Venue | Status |
|---------|-------------|-------|-----------|----------|-----------|-------|--------|
| 1 | 2 | Nordic Dev Summit | Conference | Technology | 2026-04-15 | DogA, Oslo | Upcoming |
| 2 | 2 | The Midnight Strings | Concert | Music | 2026-04-22 | Rockefeller | Upcoming |

**TicketTypes**

| TicketTypeId | EventId | Name | Price | TotalQuantity | Remaining |
|--------------|---------|------|-------|---------------|-----------|
| 1 | 1 | Early Bird | 350 | 10 | 8 |
| 2 | 1 | Standard | 500 | 40 | 34 |
| 3 | 1 | VIP | 950 | 8 | 6 |

**Bookings**

| BookingId | UserId | EventId | TicketTypeId | PriceAtBooking | BookingDate | Status | Reference |
|-----------|--------|---------|--------------|----------------|-------------|--------|-----------|
| 1 | 1 | 1 | 2 | 500 | 2026-03-29 | Confirmed | BK-00142 |

**Reviews**

| ReviewId | BookingId | UserId | EventId | Rating | Comment |
|----------|-----------|--------|---------|--------|---------|
| 1 | 1 | 1 | 1 | 5 | Excellent organisation! |

---

### What are interfaces? (IReviewable and ISearchable)

An interface is a contract. It says: *"any class that uses this interface must have these methods."* It doesn't contain any code — just the method signatures.

Think of it like a job description. The job description says "this role requires someone who can drive and speak English." It doesn't tell you *how* the person drives or *how* they speak English — it just guarantees they can do both.

**IReviewable** is the contract that guarantees an event can be reviewed:
```csharp
public interface IReviewable
{
    double GetAverageRating();
    List<Review> GetReviews();
}
```

**ISearchable** is the contract that guarantees an event can be searched:
```csharp
public interface ISearchable
{
    bool MatchesKeyword(string keyword);
}
```

When `Event` implements these interfaces (which it will in a later sprint), it's making a promise:

```csharp
public abstract class Event : IReviewable, ISearchable
{
    // ... must now provide GetAverageRating(), GetReviews(), MatchesKeyword()
}
```

The practical benefit is in `EventService`. Instead of writing separate search logic for Concert, Conference, and Workshop, you can write it once for anything that is `ISearchable`:

```csharp
// Works for Concert, Conference, AND Workshop — no duplication
public List<Event> Search(List<ISearchable> events, string keyword)
{
    return events.Where(e => e.MatchesKeyword(keyword)).ToList();
}
```

---

## Final build result

```
EventPlatform          net10.0  succeeded
EventPlatform.Tests    net10.0  succeeded
EventPlatform.Avalonia net10.0  succeeded

Build succeeded
```

Running `dotnet run` from the `EventPlatform/` project folder prints:
```
[DB] Test data loaded.
```

---

## Issue #1 checklist

| Task | Status |
|------|--------|
| Create solution with 3 projects | ✅ |
| Add .gitignore | ✅ |
| Set up branch protection on main | ❌ Requires paid GitHub plan — skipped |
| Define branching strategy | ✅ |
| Set up GitHub Projects Kanban board | ✅ |
| Add Microsoft.Data.Sqlite NuGet | ✅ |
| Copy AppDatabase.cs and DatabaseSeeder.cs into EventPlatform/Data/ | ✅ |
| Copy schema.sql and seed.sql into docs/sql/ | ✅ |
| Wire DatabaseSeeder in Program.cs | ✅ |
| Verify database seeds correctly — [DB] Test data loaded. | ✅ |

## Issue #3 checklist

| Task | Status |
|------|--------|
| Event.cs — abstract base class | ✅ |
| Concert.cs — inherits Event | ✅ |
| Conference.cs — inherits Event | ✅ |
| Workshop.cs — inherits Event | ✅ |
| IReviewable.cs — GetAverageRating(), GetReviews() | ✅ |
| ISearchable.cs — MatchesKeyword(string keyword) | ✅ |
| User.cs | ✅ |
| Booking.cs | ✅ |
| Review.cs | ✅ |
| TicketType.cs | ✅ |
| EventCategory.cs enum | ✅ |
| EventStatus.cs enum | ✅ |
| BookingStatus.cs enum | ✅ |
| All classes compile with no errors | ✅ |
| All public properties have XML doc comments | ✅ |
| No Console.WriteLine in Models folder | ✅ |
