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

        public Flight() { }

        public Flight(int flightNumber, string departureCountry, string destinationCountry, DateTime departureDate,
                      string departureAirport, string arrivalAirport, Dictionary<FlightClass, decimal> classPrices)
        {
            FlightNumber = flightNumber;
            DepartureCountry = departureCountry;
            DestinationCountry = destinationCountry;
            DepartureDate = departureDate;
            DepartureAirport = departureAirport;
            ArrivalAirport = arrivalAirport;
            ClassPrices = classPrices;
        }

        public override string ToString()
        {
            var flightString = new string('-', 155) + "\n";
            flightString += $"| {("Flight #"),-9} | {("Departure Date"),-15} | {("Departure Country"),-18} | {("Destination Country"),-20} | {("Departure Airport"),-17} | {("Arrival Airport"),-15} | {("Economy"),-11} | {("Business"),-11} | {("First Class"),-11} |\n";
            flightString += new string('-', 155) + "\n";

            flightString += $"| {FlightNumber,-9} | {DepartureDate,-15:MM/dd/yyyy} | {DepartureCountry,-18} | {DestinationCountry,-20} | {DepartureAirport,-17} | {ArrivalAirport,-15} |";
            flightString += $" {string.Join(" | ", ClassPrices.Select(classPrice => $"{classPrice.Value,-11}"))} |";

            flightString += "\n" + new string('-', 155);

            return flightString;
        }

        public string FlightSummary()
        {
            var flightString = $"| {FlightNumber,-9} | {DepartureDate,-15:MM/dd/yyyy} | {DepartureCountry,-18} | {DestinationCountry,-20} |\n";
            flightString += new string('-', 75) + "\n";

            return flightString;
        }
    }
}
