using AirportTicketBookingExercise.Domain;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingExercise.Mappers
{
    public class BookingMap : ClassMap<Booking>
    {
        public BookingMap()
        {
            Map(u => u.UserId).Index(0);
            Map(u => u.FlightNumber).Index(1);
            Map(u => u.FlightClass).Index(2);
        }
    }
}
