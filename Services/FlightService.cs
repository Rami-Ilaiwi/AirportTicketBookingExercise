using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;
using AirportTicketBookingExercise.Utils;

namespace AirportTicketBookingExercise.Services
{
    public class FlightService
    {
        public static Flight GetFlight(int fn)
        {
            List<Flight> flights = FileService.ReadCsvFile<Flight>(Files.flightsFilePath, typeof(FlightMap));
            return flights.FirstOrDefault(f => f.FlightNumber == fn);
        }

        public static Booking GetBooking(int bk)
        {
            List<Booking> booking = FileService.ReadCsvFile<Booking>(Files.bookingsFilePath, typeof(BookingMap));
            return booking.FirstOrDefault(b => b.FlightNumber == bk);
        }

        public static Dictionary<FlightClass, decimal> GetFlightClasses(Flight flight)
        {
            return flight.ClassPrices;
        }

        public static FlightClass GetFlightClassByIndex(Flight flight, int index)
        {
            return GetFlightClasses(flight).Keys.ElementAtOrDefault(index - 1);
        }

        public static List<Flight> GetAllFlights()
        {
            List<Flight> flights = FileService.ReadCsvFile<Flight>(Files.flightsFilePath, typeof(FlightMap));
            return flights;
        }
    }
}
