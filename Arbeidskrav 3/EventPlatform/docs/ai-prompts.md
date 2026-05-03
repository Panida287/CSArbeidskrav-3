# AI Prompts Log — EventPlatform Oslo

All AI prompts used in this project are documented here as required by the assignment specification.

**Tool used throughout:** Claude (Anthropic)

---

## How We Used AI

AI was used as a development assistant throughout the project — not to write the application for us, but to support specific tasks where we needed guidance, review, or a starting point. The main areas were:

**Domain and data model design** — We used Claude to help think through the event hierarchy (abstract `Event` base class with `Concert`, `Conference`, and `Workshop` subclasses), discuss where interfaces made sense (`IReviewable`, `ISearchable`), and validate that our class structure matched OOP principles before we started coding.

**Seed data generation** — Claude helped generate realistic SQL seed data for `seed.sql`, including test users, events of all three types, bookings, ticket types, and reviews. This saved significant time and gave us meaningful data to test against from day one.

**Git and branching guidance** — Several team members were less experienced with Git branching and pull request workflows. We used Claude to understand commands like `git pull origin feature --no-rebase`, how to resolve merge conflicts, what `--no-ff` means, and how to recover from divergent branches.

**Merge conflict resolution** — When a team member worked on a branch without pulling the latest changes from `feature`, their code conflicted with a refactored file structure. Claude helped us understand the 3-way merge view in Rider, decide which side to accept for each conflict section, and verify the result compiled correctly.

**Code review support** — Claude reviewed pull requests when we pasted in the code, identifying bugs such as wrong return types, typos in method names, missing `using` directives, and logic errors in repository methods.

**Unit test structure** — Claude explained the Arrange-Act-Assert pattern and helped write fake repository implementations so tests could run without a database. It also explained why this approach is used in real projects.

**Documentation scaffolding** — Claude generated the initial versions of `requirements.md`, `project-plan.md`, `file-structure-and-ui-guide.md`, `how-to-read-uml.md`, and this file. Content was reviewed and adjusted by the project manager before committing.

---

## Selected Prompts

**Date:** April 2026
**Prompt:** "Help me design the event class hierarchy for a C# console app. We need at least two event types with their own fields. What should be abstract and what should be concrete?"
**Output summary:** Suggested abstract `Event` base class with `Concert`, `Conference`, `Workshop` as derived classes. Discussed which properties belong on the base vs. subclasses.
**Used in:** `Models/Events/`

---

**Date:** April 2026
**Prompt:** "Generate realistic SQLite seed data for our event platform. We need test users, events of all three types, ticket types with different prices and quantities, bookings, and reviews."
**Output summary:** Complete `seed.sql` with hashed passwords, diverse events, multiple ticket tiers, and review data with varied ratings.
**Used in:** `docs/sql/seed.sql`

---

**Date:** April–May 2026
**Prompt:** "I ran git pull origin feature --no-rebase on Christian's branch and now I have a merge conflict in EventBrowseMenu.cs. The conflict dialog shows Yours / Theirs / Merge. What do I do?"
**Output summary:** Explained the 3-way merge view, which sections to accept from each side, and how to verify the result with `dotnet build`.
**Used in:** Merge conflict resolution, branch `11-ui-browse-search-filter-events`

---

**Date:** May 2026
**Prompt:** "Can you review EventRepository pull request and identify any bugs before I leave comments?"
**Output summary:** Identified ~14 bugs including duplicate method stubs, capitalisation errors, wrong return types, a missing `using` directive, a table name spelling error, and a logic issue where `GetAll` always returned plain `Event` objects instead of the correct subtypes.
**Used in:** PR #26 review comments

---

**Date:** May 2026
**Prompt:** "Write xUnit tests for BookingService and ReviewService using fake repositories instead of the real SQLite ones. Follow Arrange-Act-Assert and cover all the guard conditions."
**Output summary:** Full test suite with `FakeBookingRepository`, `FakeReviewRepository`, `FakeEventRepository`, and a `Fake` builder class. Tests cover capacity, ownership, duplicate booking, cancellation, review eligibility, and average rating calculation.
**Used in:** `EventPlatform.Tests/`