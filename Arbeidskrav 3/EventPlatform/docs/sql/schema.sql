-- EventPlatform Oslo — Database Schema
-- Run this first to create all tables.

CREATE TABLE IF NOT EXISTS Users (
                                     UserId       INTEGER PRIMARY KEY AUTOINCREMENT,
                                     Username     TEXT    NOT NULL UNIQUE,
                                     PasswordHash TEXT    NOT NULL,
                                     CreatedAt    TEXT    NOT NULL DEFAULT (datetime('now'))
    );

CREATE TABLE IF NOT EXISTS Events (
                                      EventId     INTEGER PRIMARY KEY AUTOINCREMENT,
                                      Title       TEXT    NOT NULL,
                                      Description TEXT,
                                      EventType   TEXT    NOT NULL,
                                      Category    TEXT    NOT NULL,
                                      EventDate   TEXT    NOT NULL,
                                      Venue       TEXT    NOT NULL,
                                      OrganiserId INTEGER NOT NULL,
                                      Status      TEXT    NOT NULL DEFAULT 'Upcoming',
                                      FOREIGN KEY (OrganiserId) REFERENCES Users(UserId)
    );

CREATE TABLE IF NOT EXISTS Concerts (
                                        ConcertId INTEGER PRIMARY KEY AUTOINCREMENT,
                                        EventId   INTEGER NOT NULL UNIQUE,
                                        Performer TEXT    NOT NULL,
                                        Genre     TEXT    NOT NULL,
                                        FOREIGN KEY (EventId) REFERENCES Events(EventId)
    );

CREATE TABLE IF NOT EXISTS Conferences (
                                           ConferenceId    INTEGER PRIMARY KEY AUTOINCREMENT,
                                           EventId         INTEGER NOT NULL UNIQUE,
                                           SessionSchedule TEXT,
                                           FOREIGN KEY (EventId) REFERENCES Events(EventId)
    );

CREATE TABLE IF NOT EXISTS Workshops (
                                         WorkshopId        INTEGER PRIMARY KEY AUTOINCREMENT,
                                         EventId           INTEGER NOT NULL UNIQUE,
                                         RequiredMaterials TEXT,
                                         MaxParticipants   INTEGER NOT NULL,
                                         FOREIGN KEY (EventId) REFERENCES Events(EventId)
    );

CREATE TABLE IF NOT EXISTS TicketTypes (
                                           TicketTypeId     INTEGER PRIMARY KEY AUTOINCREMENT,
                                           EventId          INTEGER NOT NULL,
                                           Name             TEXT    NOT NULL,
                                           Price            REAL    NOT NULL,
                                           TotalQuantity    INTEGER NOT NULL,
                                           Remaining        INTEGER NOT NULL,
                                           FOREIGN KEY (EventId) REFERENCES Events(EventId)
    );

CREATE TABLE IF NOT EXISTS Bookings (
                                        BookingId    INTEGER PRIMARY KEY AUTOINCREMENT,
                                        UserId       INTEGER NOT NULL,
                                        EventId      INTEGER NOT NULL,
                                        TicketTypeId INTEGER NOT NULL,
                                        Status       TEXT    NOT NULL DEFAULT 'Confirmed',
                                        Reference    TEXT    NOT NULL UNIQUE,
                                        BookingDate  TEXT    NOT NULL DEFAULT (datetime('now')),
    PriceAtBooking REAL  NOT NULL,
    FOREIGN KEY (UserId)       REFERENCES Users(UserId),
    FOREIGN KEY (EventId)      REFERENCES Events(EventId),
    FOREIGN KEY (TicketTypeId) REFERENCES TicketTypes(TicketTypeId)
    );

CREATE TABLE IF NOT EXISTS Reviews (
                                       ReviewId  INTEGER PRIMARY KEY AUTOINCREMENT,
                                       BookingId INTEGER NOT NULL UNIQUE,
                                       UserId    INTEGER NOT NULL,
                                       EventId   INTEGER NOT NULL,
                                       Rating    INTEGER NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment   TEXT,
    CreatedAt TEXT    NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (BookingId) REFERENCES Bookings(BookingId),
    FOREIGN KEY (UserId)    REFERENCES Users(UserId),
    FOREIGN KEY (EventId)   REFERENCES Events(EventId)
    );