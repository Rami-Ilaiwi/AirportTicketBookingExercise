using CsvHelper.Configuration.Attributes;

namespace AirportTicketBookingExercise.Domain
{
    public class Booking
    {
        [Index(0)]
        public int UserId { get; set; }
        [Index(1)]
        public int FlightNumber { get; set; }
        [Index(2)]
        public FlightClass FlightClass { get; set; }

        public Booking() { }

        public Booking(int userId, int flightNumber, FlightClass flightClass)
        {
            UserId = userId;
            FlightNumber = flightNumber;
            FlightClass = flightClass;
        }
    }
}
