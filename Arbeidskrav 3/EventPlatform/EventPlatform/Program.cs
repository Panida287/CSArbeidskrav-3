using EventPlatform.Data;
using EventPlatform.Services;
using EventPlatform.UI.Menus;

// Entry point — wires database, services, and menus together.

var db = new AppDatabase();
var seeder = new DatabaseSeeder(db);
seeder.Seed();

var userService = new UserService();
var eventService = new EventService();
var bookingService = new BookingService();
var reviewService = new ReviewService();

var authMenu = new AuthMenu(userService);
authMenu.Show();

var mainMenu = new MainMenu(userService);
mainMenu.Show();
