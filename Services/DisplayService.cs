using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;

namespace AirportTicketBookingExercise.Services
{
    public class DisplayService
    {
        private static readonly string flightsFilePath = "flights.csv";
        private static readonly string bookingsFilePath = "bookings.csv";

        public static void ShowAvailableFlightsSummary()
        {
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine($"| {("Flight #"),-9} | {("Departure Date"),-15} | {("Departure Country"),-18} | {("Destination Country"),-20} |");
            Console.WriteLine("---------------------------------------------------------------------------");

            List<Flight> flights = FileService.ReadCsvFile<Flight>(flightsFilePath, typeof(FlightMap));

            flights.ForEach(flight => Console.Write(flight.FlightSummary()));
        }
        public static void ShowAvailableFlightsDetail()
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"| {("Flight #"),-9} | {("Departure Date"),-15} | {("Departure Country"),-18} | {("Destination Country"),-20} | {("Departure Airport"),-17} | {("Arrival Airport"),-15} | {("Economy"),-11} | {("Business"),-11} | {("First Class"),-11} |");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------");

            List<Flight> flights = FileService.ReadCsvFile<Flight>(flightsFilePath, typeof(FlightMap));

            foreach (Flight flight in flights)
            {
                Console.WriteLine(flight);
            }
        }
        public static void ShowFlightClassesPrices(Flight flight)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"|{(""),-6}| {("Class"),-11} | {("Price"),-9} |");
            Console.WriteLine("----------------------------------");

            var index = 1;
            FlightService.GetFlightClasses(flight)
                .ToList()
                .ForEach(classPrice => Console.WriteLine($"| #{(index++),-3} | {(classPrice.Key),-11} | {(classPrice.Value),-9} |"));

            Console.WriteLine("----------------------------------");
        }

        public static void ShowBookingsForUser(User user)
        {
            List<Booking> bookings = FileService.ReadCsvFile<Booking>(bookingsFilePath, typeof(BookingMap));
            List<Booking> userBookings = User.GetBookings().Where(booking => booking.UserId == user.Id).ToList();

            if (userBookings.Any())
            {
                Console.WriteLine($"Your bookings:");
                Console.WriteLine("--------------------------");
                Console.WriteLine($"| {("Flight #"),-8} | {("Class"),-11} |");
                Console.WriteLine("--------------------------");

                foreach (var booking in userBookings)
                {
                    Console.WriteLine($"| {(booking.FlightNumber),-8} | {(booking.FlightClass),-11} |");
                    Console.WriteLine("--------------------------");
                }
            }
            else
            {
                Console.WriteLine($"No bookings found for UserId {user.Id}.");
            }
        }

        public static void DisplayPassengerMenu()
        {
            Console.WriteLine("1: Book Flight");
            Console.WriteLine("2: Update Booking");
            Console.WriteLine("3: Cancel Booking");
            Console.WriteLine("4: View Personal Bookings");
            Console.WriteLine("5: Logout");

            Console.Write("Your selection: ");
            string selection = Console.ReadLine();
            Console.WriteLine();

            switch (selection)
            {
                case "1":
                    Utilities.BookFlight(UserService.UserLoggedIn);
                    break;
                case "2":
                    Utilities.ModifyBooking(UserService.UserLoggedIn);
                    break;
                case "3":
                    Utilities.CancelBooking(UserService.UserLoggedIn);
                    break;
                case "4":
                    Utilities.ViewPersonalBookings(UserService.UserLoggedIn);
                    break;
                case "5":
                    UserService.LogoutUser();
                    break;
                default:
                    Console.WriteLine("Wrong selection! Please enter a valid selection");
                    break;
            }
        }

        public static void DisplayManagerMenu()
        {
            Console.WriteLine("1: Book Flight");
            Console.WriteLine("2: Update Booking");
            Console.WriteLine("3: Cancel Booking");
            Console.WriteLine("4: View Personal Bookings");
            Console.WriteLine("5: Logout");

            Console.Write("Your selection: ");
            string selection = Console.ReadLine();
            Console.WriteLine();

            switch (selection)
            {
                case "1":
                    Utilities.BookFlight(UserService.UserLoggedIn);
                    break;
                case "2":
                    Utilities.ModifyBooking(UserService.UserLoggedIn);
                    break;
                case "3":
                    Utilities.CancelBooking(UserService.UserLoggedIn);
                    break;
                case "4":
                    Utilities.ViewPersonalBookings(UserService.UserLoggedIn);
                    break;
                case "5":
                    UserService.LogoutUser();
                    break;
                default:
                    Console.WriteLine("Wrong selection! Please enter a valid selection");
                    break;
            }
        }
    }
}
