using AirportTicketBookingExercise.Domain;

namespace AirportTicketBookingExercise.Utils
{
    public static class Extensions
    {
        public static Func<Booking, bool> GetBookingPredicate(this Booking book)
        {
            return x => x.UserId == book.UserId && x.FlightNumber == book.FlightNumber;
        }
    }
}
