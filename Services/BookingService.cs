using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;
using AirportTicketBookingExercise.Utils;

namespace AirportTicketBookingExercise.Services
{
    public class BookingService
    {
        public static Booking GetBooking(int bk)
        {
            List<Booking> booking = FileService.ReadCsvFile<Booking>(Files.bookingsFilePath, typeof(BookingMap));
            return booking.FirstOrDefault(b => b.FlightNumber == bk);
        }

        public static bool DoesUserHasBookings(User user)
        {
            var bookings = GetAllBookingsForUser(user);
            return bookings.Any(booking => booking.UserId == user.Id);
        }

        public static List<Booking> GetAllBookings()
        {
            return FileService.ReadCsvFile<Booking>(Files.bookingsFilePath, typeof(BookingMap));
        }

        public static List<Booking> GetAllBookingsForUser(User user)
        {
            return GetAllBookings().Where(booking => booking.UserId == user.Id).ToList();
        }

        public static bool DoesFlightBookedForUser(User user, int flightNumber)
        {
            var booking = GetAllBookingsForUser(user);
            return booking.Any(b => b.FlightNumber == flightNumber);
        }
    }
}
