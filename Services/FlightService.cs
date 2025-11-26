using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;
using AirportTicketBookingExercise.Utils;

namespace AirportTicketBookingExercise.Services
{
    public class FlightService
    {
        public static Flight GetFlight(int fn)
        {
            var flights = GetAllFlights();
            return flights.FirstOrDefault(f => f.FlightNumber == fn);
        }

        public static bool DoesFlightExist(int fn)
        {
            var flights = GetAllFlights();
            return flights.Any(f => f.FlightNumber == fn);
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
            var flights = FileService.ReadCsvFile<Flight>(Files.flightsFilePath, typeof(FlightMap));
            return flights;
        }
    }
}
