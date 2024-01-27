using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Services;

var exitProgram = false;
FileService.InitializeFiles();
var c = new Dictionary<FlightClass, decimal>
{
    { FlightClass.Economy, 500.00m },
    { FlightClass.Business, 1000.00m },
    { FlightClass.FirstClass, 1500.00m }
};
var flight = new Flight(111, "sddsf", "sdfsdf", new DateTime(), "sdfsdfsdf", "sdfsdf", c);
Console.WriteLine(flight.ToString());

while (!exitProgram)
{
    try
    {
        if (UserService.UserLoggedIn == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Welcome to Airport Ticket Booking System");
            Console.ResetColor();
            UserService.RunApplication();
        }
        else
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Welcome, {UserService.UserLoggedIn.UserName}!");
            Console.WriteLine($"User Type: {UserService.UserLoggedIn.Type}\n");
            Console.ResetColor();

            if (UserService.UserLoggedIn.Type == UserType.Passenger)
            {
                DisplayService.DisplayPassengerMenu();
            }
            else if (UserService.UserLoggedIn.Type == UserType.Manager)
            {
                DisplayService.DisplayManagerMenu();
            }
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        DisplayService.DisplayErrorMessage($"An error occurred: {ex.Message}");
    }
}

Console.WriteLine("Goodbye!");
