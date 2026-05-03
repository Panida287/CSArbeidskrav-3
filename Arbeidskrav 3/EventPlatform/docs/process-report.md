# Process Report — EventPlatform Oslo

## 1. Project Management Philosophy

We followed a Scrum-inspired approach organised into four sprints, each lasting approximately one week. A GitHub Projects Kanban board tracked all tasks, and GitHub Issues were used for every feature with assignees, labels, and milestones. In practice we leaned more towards Kanban than strict Scrum — we did not hold formal sprint reviews, but the board gave everyone a clear view of what was in progress and what was blocked.

The sprint structure worked well for planning. Breaking the project into models → services → UI → testing gave a natural build order where each sprint depended on the previous one being stable.

## 2. Roles

| Member | Primary Responsibility |
|--------|----------------------|
| Panida | Project Manager — GitHub setup, documentation, integration, final review, PR feedback |
| Aksel | Lead Developer — services, business logic, password security, unit tests |
| Christian | Repository Developer — all SQLite repositories, booking and event UI logic |
| Laila | UI Developer — all console menus, display layer, auth screens |

Roles stayed largely stable throughout the project. Panida took on more integration and conflict-resolution work than originally planned as the codebase grew more interconnected. Aksel also supported Christian with code review and debugging when repository methods had issues.

## 3. Development Practices

**Branching:** Feature branches named `feature/description` for shared work, and issue-specific branches (e.g. `11-ui-browse-search-filter-events`) for individual tasks. The `main` branch was protected and required a pull request before merging.

**Code review:** Pull requests were reviewed by Panida before merging. Inline GitHub comments were used to flag bugs and request changes. Team members pushed fixes to the same branch rather than opening new PRs.

**Task tracking:** GitHub Issues with labels (`sprint-1` through `sprint-4`, `services`, `database`, `ui`, `testing`, `docs`) and milestones. The Kanban board was updated throughout each sprint.

**Communication:** Primarily through a group chat with async updates. We met in person when possible but were limited by scheduling and available workspace.

**Version control:** Conventional Commits format (`feat:`, `fix:`, `docs:`, `refactor:`) throughout. Each team member committed under their own GitHub account.

## 4. Reflection

### Panida (Project Manager)
Setting up the full project structure, Kanban board, and documentation scaffold before coding started made a real difference — the team always knew what to work on and where files should go. What I underestimated was how much time merge conflicts would take. Several times a team member had worked on a branch without pulling the latest changes from `feature`, leading to conflicts that had to be resolved manually. This cost more time than expected and pushed the Avalonia stretch goal out of reach.

I also wish we could have met more frequently in person. Finding a shared workspace was difficult, and working async meant some issues blocked others for longer than they needed to. If I did this again I would push for a fixed weekly meeting from the start, and I would also set a stricter rule about pulling from `feature` before starting any new work.

Lastly — I underestimated everyone's personal obligations outside of school. Tasks that looked straightforward on paper sometimes took longer because people had work, other courses, or life things in the way. Building more buffer into the sprint plan would have helped.

### Laila
I feel like I learned a lot more during this project than the other arbeidskravs. The structured setup made it easy to know exactly what I was supposed to do at any given time. I would have loved to see or work on the Avalonia GUI, but I understand we ran out of time. If I did this again I would start coding earlier — we had time at the beginning that we didn't use as well as we could have. Better time management early on would have given us the buffer we needed for the stretch goal.

### Aksel
The project scope was larger than previous assignments, but the main challenges were Git-related rather than code-related. Merge conflicts and branch issues took more time than expected. Different schedules also made it hard to work in person, which is something I would prioritise more in a future project. Panida's leadership — structuring the workflow, handing out tasks clearly, bridging gaps within the team, and being patient when helping others — made a big difference to how smoothly the project ran overall.

### Christian
This project was more complex than previous assignments, both in terms of codebase size and coordination. My main responsibility was the repository layer and some UI logic, which gave me a good understanding of how the database connects to the rest of the application. The biggest challenge for me was keeping up with changes in the shared codebase — a few times I worked on a branch without pulling the latest updates, which caused merge conflicts that took time to resolve. That taught me a lot about Git workflows in practice. I would have liked to meet more in person, but schedules made it difficult. Panida kept the project well organised and was always available when I got stuck, which made it easier to stay on track.

## 5. Unit Testing

Unit tests were scoped out before submission. The assignment lists them as an optional extension, and given the scale of the project the team judged that thorough manual testing was sufficient. All core features — registration, login, browsing, booking, cancellation, reviews, and the organiser flow — were manually tested across multiple sessions before submission. If the project were larger or maintained over a longer period, automated tests would have been a priority.

## 6. What Went Well

- The upfront planning paid off — the file structure, GitHub setup, and documentation scaffold meant the team could work in parallel without stepping on each other
- Password security was implemented correctly from the start (PBKDF2-SHA256, 100k iterations, random salts)
- The separation of concerns between models, repositories, services, and UI made the codebase easy to navigate and debug
- All core features are working: register, login, browse, search, filter, book, cancel, review, and profile

## 7. What We Would Do Differently

- Enforce a rule that every team member pulls from `feature` before starting work on a new branch
- Schedule a fixed weekly in-person meeting from sprint 1
- Start the stretch goal (Avalonia) earlier, or scope it out entirely from the plan so it does not create false expectations
- Build more time buffer into each sprint for review, conflict resolution, and unexpected blockers