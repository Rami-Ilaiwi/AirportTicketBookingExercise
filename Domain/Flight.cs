using CsvHelper.Configuration.Attributes;
using System.Text;

namespace AirportTicketBookingExercise.Domain
{
    public class Flight
    {
        [Index(0)]
        public int FlightNumber { get; set; }
        [Index(1)]
        public string DepartureCountry { get; set; }
        [Index(2)]
        public string DestinationCountry { get; set; }
        [Index(3)]
        public DateTime DepartureDate { get; set; }
        [Index(4)]
        public string DepartureAirport { get; set; }
        [Index(5)]
        public string ArrivalAirport { get; set; }
        [Index(6)]
        public Dictionary<FlightClass, decimal> ClassPrices { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"| {FlightNumber,-9} | {DepartureDate,-15:MM/dd/yyyy} | {DepartureCountry,-18} | {DestinationCountry,-20} | {DepartureAirport,-17} | {ArrivalAirport,-15} |");
            foreach (var classPrice in ClassPrices)
            {
                sb.Append($" {classPrice.Value,-11} |");
            }

            sb.Append("\n-----------------------------------------------------------------------------------------------------------------------------------------------------------");


            return sb.ToString();
        }

        public string FlightSummary()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"| {FlightNumber,-9} | {DepartureDate,-15:MM/dd/yyyy} | {DepartureCountry,-18} | {DestinationCountry,-20} |");
            sb.AppendLine("---------------------------------------------------------------------------");

            return sb.ToString();
        }
    }
}
