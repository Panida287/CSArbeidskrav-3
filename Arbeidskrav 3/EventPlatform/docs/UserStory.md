# Requirements — User Stories

## US-01: Register an Account
As a new user, I want to register with a username and password, so that I can access the platform.

**Acceptance Criteria:**
- User cannot register with a username that already exists
- User cannot register with an empty username or password
- Password is never shown in plain text at any point

---

## US-02: Log In
As a registered user, I want to log in with my credentials, so that I can access my account and its features.

**Acceptance Criteria:**
- User cannot log in with wrong credentials — a friendly error is shown
- Successful login navigates to the main menu
- Session persists across all screens until logout

---

## US-03: Log Out
As a logged-in user, I want to log out, so that my account is secure when I am done.

**Acceptance Criteria:**
- Logging out clears the session completely
- After logout, the user is returned to the welcome screen
- No user data is accessible after logout without logging in again

---

## US-04: Browse Upcoming Events
As a user, I want to browse all upcoming events, so that I can see what is available.

**Acceptance Criteria:**
- All upcoming events are shown in date order
- Each event shows title, type, date, and ticket availability
- Sold out events are clearly marked

---

## US-05: Search for Events
As a user, I want to search events by keyword, so that I can quickly find something specific.

**Acceptance Criteria:**
- Search is case-insensitive and matches partial words
- Search checks title, description, and venue
- If no results are found, a friendly message is shown

---

## US-06: Filter Events
As a user, I want to filter events by category or type, so that I can find events that match my interests.

**Acceptance Criteria:**
- User can filter by category (e.g. Music, Technology, Sports)
- User can filter by event type (Concert, Conference, Workshop)
- Filtered results use the same layout as the browse list

---

## US-07: View Event Details
As a user, I want to view full details of an event, so that I can decide whether to book it.

**Acceptance Criteria:**
- All event fields are displayed: title, organiser, type, category, date, venue, description
- Ticket types are shown with price and remaining count
- Average rating and review count are displayed

---

## US-08: Book a Ticket
As a user, I want to book a ticket for an event, so that I can attend it.

**Acceptance Criteria:**
- User cannot book their own event
- User cannot book a sold-out ticket type
- A unique booking reference in BK-XXXXX format is shown after confirmation

---

## US-09: Cancel a Booking
As a user, I want to cancel a booking I have made, so that I can free up my spot if my plans change.

**Acceptance Criteria:**
- User must confirm cancellation before it is processed
- Cancelled booking still appears in booking history with Cancelled status
- Ticket remaining count is restored in the database after cancellation

---

## US-10: View My Bookings
As a user, I want to see all my bookings split into upcoming and past, so that I can keep track of my attendance history.

**Acceptance Criteria:**
- Bookings are split into Upcoming and Past sections
- Each booking shows reference, event, date, ticket type, price, and status
- Cancelled bookings appear in the Past section

---

## US-11: Leave a Review
As an attendee, I want to leave a review for a completed event, so that I can share my experience with others.

**Acceptance Criteria:**
- User can only review an event they have a confirmed booking for
- User cannot review an event they organised
- User cannot leave more than one review per booking

---

## US-12: View Reviews
As a user, I want to read reviews for an event, so that I can make an informed decision about attending.

**Acceptance Criteria:**
- All reviews are shown with star rating, comment, and username
- Average rating is displayed at the top
- If no reviews exist, a friendly message is shown

---

## US-13: Create an Event
As an organiser, I want to create a new event, so that I can invite others to attend.

**Acceptance Criteria:**
- User must provide a title, date, venue, and at least one ticket type
- Event date cannot be in the past
- All three event types (Concert, Conference, Workshop) can be created

---

## US-14: Edit an Event
As an organiser, I want to edit my event details, so that I can correct mistakes or update information.

**Acceptance Criteria:**
- Only the organiser of the event can edit it
- A different user attempting to edit sees a clear error message
- Changes are saved to the database immediately

---

## US-15: Cancel an Event
As an organiser, I want to cancel an event, so that attendees know it will not go ahead.

**Acceptance Criteria:**
- Only the organiser can cancel their own event
- A confirmation prompt is shown before cancellation
- Cancelled events no longer appear in the browse list