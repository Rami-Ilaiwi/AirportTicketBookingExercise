using AirportTicketBookingExercise.Domain;
using CsvHelper.Configuration;

namespace AirportTicketBookingExercise.Mappers
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Map(u => u.Id).Index(0);
            Map(u => u.UserName).Index(1);
            Map(u => u.Password).Index(2);
            Map(u => u.Type).Index(3);
        }
    }
}
