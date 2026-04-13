-- EventPlatform Oslo — Norwegian-themed Seed Data
-- Run after schema.sql.

INSERT INTO Users (Username, PasswordHash) VALUES
('anna',       '$2a$12$placeholder_hash_anna'),
('techevents', '$2a$12$placeholder_hash_techevents'),
('aksel',      '$2a$12$placeholder_hash_aksel');

INSERT INTO Events (Title, Description, Type, Category, Date, Venue, OrganiserId, Status) VALUES
('Nordic Dev Summit',    'A full-day conference on modern backend development.', 'Conference', 'Technology', '2026-04-15 09:00', 'DogA, Oslo',          2, 'Upcoming'),
('The Midnight Strings', 'Live chamber music under the midnight sun.',           'Concert',    'Music',      '2026-04-22 19:30', 'Rockefeller, Oslo',   2, 'Upcoming'),
('Intro to Watercolour', 'Beginner-friendly watercolour painting workshop.',     'Workshop',   'Arts',       '2026-04-28 13:00', 'Kunstnernes Hus',     1, 'Upcoming'),
('Oslo Jazz Festival',   'Annual jazz festival across multiple Oslo venues.',    'Concert',    'Music',      '2026-05-01 17:00', 'Aker Brygge, Oslo',   2, 'Upcoming');

INSERT INTO TicketTypes (EventId, Name, Price, Quantity, RemainingTickets) VALUES
(1, 'Early Bird', 350, 10, 8),
(1, 'Standard',   500, 40, 34),
(1, 'VIP',        950,  8,  6),
(2, 'Standard',   400, 50,  0),
(3, 'Standard',   200, 15, 12),
(4, 'General',    250, 100, 75);
