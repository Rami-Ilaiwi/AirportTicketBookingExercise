using AirportTicketBookingExercise.Domain;
using CsvHelper.Configuration;
using System.Globalization;

namespace AirportTicketBookingExercise.Mappers
{
    public class FlightMap : ClassMap<Flight>
    {
        public FlightMap()
        {
            Map(f => f.FlightNumber).Index(0);
            Map(f => f.DepartureCountry).Index(1);
            Map(f => f.DestinationCountry).Index(2);
            Map(f => f.DepartureDate).Index(3);
            Map(f => f.DepartureAirport).Index(4);
            Map(f => f.ArrivalAirport).Index(5);
            Map(f => f.ClassPrices).Convert(row =>

            {
                var classPrices = new Dictionary<FlightClass, decimal>();

                var classPricesString = row.Row[6];
                if (!string.IsNullOrEmpty(classPricesString))
                {
                    var classes = classPricesString.Split("-");
                    foreach (var classPrice in classes)
                    {
                        var classPricePairs = classPrice.Split(',');
                        foreach (var classPricePair in classPricePairs)
                        {
                            var parts = classPricePair.Split(';');
                            if (parts.Length == 2)
                            {
                                var classTypeStr = parts[0].Trim();
                                var priceStr = parts[1].Trim();

                                if (Enum.TryParse<FlightClass>(classTypeStr, out var classType) &&
                                    decimal.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
                                {
                                    classPrices.Add(classType, price);
                                }
                                else
                                {
                                    Console.WriteLine($"Error parsing class price pair: {classPricePair}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Unexpected format in class price pair: {classPricePair}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ClassPrices string is null or empty.");
                }
                return classPrices;
            });
        }
    }
}
