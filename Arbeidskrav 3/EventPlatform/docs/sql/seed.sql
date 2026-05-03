-- EventPlatform Oslo — Norwegian-themed Seed Data
-- Run after schema.sql.

INSERT INTO Users (Username, PasswordHash, CreatedAt)
VALUES ('anna', '100000:78ZXOQB/5j0Zevqa8+TbWg==:39gy77PBhARNFkK4a2DaSxsYJ3CaogIXaVYYZ4mfOf0=', datetime('now')),
       ('techevents', '100000:kKJUTsv3qxsap39X78D31g==:/dWv0iFwSQL2UsopeeUZiImm3xRY2rJd6T3bllQMG7Y=', datetime('now')),
       ('aksel', '100000:m8NC+JIPj22yCHb2Kkfb4w==:xtUdKUYLJRziKnhwfuVLnwHEr4uApJedu/y2YgeTmvI=', datetime('now'));

INSERT INTO Events (Title, Description, EventType, Category, EventDate, Venue, OrganiserId, Status)
VALUES ('Nordic Dev Summit', 'A full-day conference on modern backend development.', 'Conference', 'Technology',
        '2026-04-15 09:00', 'DogA, Oslo', 2, 'Upcoming'),
       ('The Midnight Strings', 'Live chamber music under the midnight sun.', 'Concert', 'Music', '2026-04-22 19:30',
        'Rockefeller, Oslo', 2, 'Upcoming'),
       ('Intro to Watercolour', 'Beginner-friendly watercolour painting workshop.', 'Workshop', 'Arts',
        '2026-04-28 13:00', 'Kunstnernes Hus', 1, 'Upcoming'),
       ('Oslo Jazz Festival', 'Annual jazz festival across multiple Oslo venues.', 'Concert', 'Music',
        '2026-05-01 17:00', 'Aker Brygge, Oslo', 2, 'Upcoming');

INSERT INTO Concerts (EventId, Performer, Genre)
VALUES (2, 'The Midnight Strings Ensemble', 'Classical'),
       (4, 'Various Artists', 'Jazz');

INSERT INTO Conferences (EventId, SessionSchedule)
VALUES (1, 'Morning: Keynote, Backend Deep Dive. Afternoon: Workshops, Panel Q&A.');

INSERT INTO Workshops (EventId, RequiredMaterials, MaxParticipants)
VALUES (3, 'Watercolour paints, brushes, and paper provided.', 15);

INSERT INTO TicketTypes (EventId, Name, Price, TotalQuantity, Remaining)
VALUES (1, 'Early Bird', 350, 10, 8),
       (1, 'Standard', 500, 40, 34),
       (1, 'VIP', 950, 8, 6),
       (2, 'Standard', 400, 50, 0),
       (3, 'Standard', 200, 15, 12),
       (4, 'General', 250, 100, 75);