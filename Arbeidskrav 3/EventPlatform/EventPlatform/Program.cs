using System.Globalization;
using EventPlatform.Data;
using EventPlatform.Services;
using EventPlatform.UI.Menus;


// Entry point — wires database, services, and menus together.

var db = new AppDatabase("eventplatform.db");
var seeder = new DatabaseSeeder(db, sqlFolder: "../docs/sql");
seeder.Seed();

var userService = new UserService();
var eventService = new EventService();
var bookingService = new BookingService();
var reviewService = new ReviewService();

var authMenu = new AuthMenu(userService, eventService, bookingService, reviewService);
authMenu.Show();

var mainMenu = new MainMenu(userService);
mainMenu.Show();
