using AirportTicketBookingExercise.Mappers;
using AirportTicketBookingExercise.Services;
using CsvHelper.Configuration.Attributes;

namespace AirportTicketBookingExercise.Domain
{
    public class User
    {
        private static string bookingsFlightFile = "bookings.csv";

        [Index(0)]
        public int Id { get; set; }
        [Index(1)]
        public string UserName { get; set; }
        [Index(2)]
        public string Password { get; set; }
        [Index(3)]
        public UserType Type { get; set; }

        public void BookFlight(Flight flight, FlightClass flightClass, User user)
        {
            List<Booking> booking = new List<Booking>();
            booking.Add(new Booking(user.Id, flight.FlightNumber, flightClass));
            FileService.WriteToCsv(bookingsFlightFile, booking);
        }

        public void UpdateBookingRecord(Flight flight, FlightClass flightClass, Booking book)
        {
            List<Booking> booking = new List<Booking>();
            booking.Add(new Booking(book.UserId, flight.FlightNumber, flightClass));
            FileService.UpdateCsvRecord(bookingsFlightFile, booking, x => x.UserId == book.UserId && x.FlightNumber == book.FlightNumber, typeof(BookingMap));
        }

        public void RemoveBookingRecord(Booking book)
        {
            FileService.RemoveCsvRecord<Booking>(bookingsFlightFile, x => x.UserId == book.UserId && x.FlightNumber == book.FlightNumber, typeof(BookingMap));
        }

        public static List<Booking> GetBookings()
        {
            List<Booking> bookings = FileService.ReadCsvFile<Booking>(bookingsFlightFile, typeof(BookingMap));
            return bookings;
        }
    }
}
