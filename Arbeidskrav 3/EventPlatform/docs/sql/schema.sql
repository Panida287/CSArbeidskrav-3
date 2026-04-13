-- EventPlatform Oslo — Database Schema
-- Run this first to create all tables.

CREATE TABLE IF NOT EXISTS Users (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
    Username    TEXT    NOT NULL UNIQUE,
    PasswordHash TEXT   NOT NULL
);

CREATE TABLE IF NOT EXISTS Events (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
    Title       TEXT    NOT NULL,
    Description TEXT,
    Type        TEXT    NOT NULL,   -- Concert | Conference | Workshop
    Category    TEXT    NOT NULL,
    Date        TEXT    NOT NULL,
    Venue       TEXT    NOT NULL,
    OrganiserId INTEGER NOT NULL,
    Status      TEXT    NOT NULL DEFAULT 'Upcoming',
    FOREIGN KEY (OrganiserId) REFERENCES Users(Id)
);

CREATE TABLE IF NOT EXISTS TicketTypes (
    Id               INTEGER PRIMARY KEY AUTOINCREMENT,
    EventId          INTEGER NOT NULL,
    Name             TEXT    NOT NULL,
    Price            REAL    NOT NULL,
    Quantity         INTEGER NOT NULL,
    RemainingTickets INTEGER NOT NULL,
    FOREIGN KEY (EventId) REFERENCES Events(Id)
);

CREATE TABLE IF NOT EXISTS Bookings (
    Id           INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId       INTEGER NOT NULL,
    EventId      INTEGER NOT NULL,
    TicketTypeId INTEGER NOT NULL,
    Status       TEXT    NOT NULL DEFAULT 'Confirmed',
    Reference    TEXT    NOT NULL UNIQUE,
    BookedAt     TEXT    NOT NULL,
    FOREIGN KEY (UserId)       REFERENCES Users(Id),
    FOREIGN KEY (EventId)      REFERENCES Events(Id),
    FOREIGN KEY (TicketTypeId) REFERENCES TicketTypes(Id)
);

CREATE TABLE IF NOT EXISTS Reviews (
    Id        INTEGER PRIMARY KEY AUTOINCREMENT,
    BookingId INTEGER NOT NULL UNIQUE,
    UserId    INTEGER NOT NULL,
    EventId   INTEGER NOT NULL,
    Rating    INTEGER NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment   TEXT,
    CreatedAt TEXT    NOT NULL,
    FOREIGN KEY (BookingId) REFERENCES Bookings(Id),
    FOREIGN KEY (UserId)    REFERENCES Users(Id),
    FOREIGN KEY (EventId)   REFERENCES Events(Id)
);
