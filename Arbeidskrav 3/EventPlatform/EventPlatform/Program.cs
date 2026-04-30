using EventPlatform.Data;
using EventPlatform.Repositories;
using EventPlatform.Services;
using EventPlatform.UI.Menus;

// Entry point — wires database, services, and menus together.

var db = new AppDatabase("eventplatform.db");
var seeder = new DatabaseSeeder(db, sqlFolder: "../docs/sql");
seeder.Seed();

var userRepository = new UserRepository(db);
var userService = new UserService(userRepository);
var eventRepository = new EventRepository(db);
var eventService = new EventService(eventRepository);
var bookingRepository = new BookingRepository(db);
var bookingService = new BookingService(bookingRepository);
var reviewRepository = new ReviewRepository();
var reviewService = new ReviewService(reviewRepository, bookingRepository);

var authMenu = new AuthMenu(userService, eventService, bookingService, reviewService);
authMenu.Show();