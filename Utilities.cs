using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;
using AirportTicketBookingExercise.Services;

namespace AirportTicketBookingExercise
{
    public static class Utilities
    {
        private static readonly string bookingsFilePath = "bookings.csv";

        public static bool HasBookedFlight(int flightNumber)
        {
            List<Booking> userBookings = FileService.ReadCsvFile<Booking>(Utilities.bookingsFilePath, typeof(BookingMap))
                .Where(booking => booking.UserId == UserService.UserLoggedIn.Id && booking.FlightNumber == flightNumber)
                .ToList();

            return userBookings.Any();
        }

        public static void BookFlight(this User user)
        {
            Console.WriteLine("Select your booking to update, please!\n");
            DisplayService.ShowAvailableFlightsSummary();
            var flightNumber = int.TryParse(Console.ReadLine(), out var fn);

            if (HasBookedFlight(fn))
            {
                Console.WriteLine($"You have already booked Flight number {fn}. Cannot book the same flight again.");
                return;
            }

            var flight = FlightService.GetFlight(fn);
            Console.WriteLine($"Select the Flight class for Flight number {fn}, please!");
            DisplayService.ShowFlightClassesPrices(flight);
            var readFlightClass = int.TryParse(Console.ReadLine(), out var fc);
            var flightClass = FlightService.GetFlightClassByIndex(flight, fc);
            user.BookFlight(flight, flightClass, user);
        }

        public static void ModifyBooking(this User user)
        {
            Console.WriteLine("Select a booking to update, please!\n");
            DisplayService.ShowBookingsForUser(user);
            var flightNumber = int.TryParse(Console.ReadLine(), out var fn);
            var flight = FlightService.GetFlight(fn);
            Console.WriteLine($"Select the Flight class for Flight number {fn} to update your booking, please!");
            DisplayService.ShowFlightClassesPrices(flight);
            var booking = FlightService.GetBooking(fn);
            var readFlightClass = int.TryParse(Console.ReadLine(), out var fc);
            var newFlightClass = FlightService.GetFlightClassByIndex(flight, fc);
            user.UpdateBookingRecord(flight, newFlightClass, booking);
        }

        public static void CancelBooking(this User user)
        {
            Console.WriteLine("Select a booking to remove, please!\n");
            DisplayService.ShowBookingsForUser(user);
            var flightNumber = int.TryParse(Console.ReadLine(), out var fn);
            var flight = FlightService.GetFlight(fn);
            var booking = FlightService.GetBooking(fn);
            user.RemoveBookingRecord(booking);
        }

        public static void ViewPersonalBookings(this User user)
        {
            List<Booking> userBookings = User.GetBookings().Where(booking => booking.UserId == user.Id).ToList();

            if (userBookings.Any())
            {
                Console.WriteLine("Here are your bookings!\n");
                DisplayService.ShowBookingsForUser(user);
            }
            else
            {
                Console.WriteLine("There are no bookings to show.");
            }
        }
    }
}
