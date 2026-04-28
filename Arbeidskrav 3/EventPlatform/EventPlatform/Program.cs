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
var eventService = new EventService();
var bookingService = new BookingService();
var reviewService = new ReviewService();

var authMenu = new AuthMenu(userService, eventService, bookingService, reviewService);
authMenu.Show();