using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Services;

bool exitProgram = false;
FileService.InitializeFiles();

while (!exitProgram)
{
    try
    {
        if (UserService.UserLoggedIn == null)
        {
            Console.Clear();
            Console.WriteLine("Welcome to Airport Ticket Booking System");
            UserService.RunApplication();
        }
        else
        {
            Console.Clear();
            Console.WriteLine($"Welcome, {UserService.UserLoggedIn.UserName}!");
            Console.WriteLine($"User Type: {UserService.UserLoggedIn.Type}\n");

            if (UserService.UserLoggedIn.Type == UserType.Passenger)
            {
                DisplayService.DisplayPassengerMenu();
            }
            else if (UserService.UserLoggedIn.Type == UserType.Manager)
            {
                DisplayService.DisplayManagerMenu();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        Console.Clear();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

Console.WriteLine("Goodbye!");
