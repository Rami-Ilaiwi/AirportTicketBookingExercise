using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
