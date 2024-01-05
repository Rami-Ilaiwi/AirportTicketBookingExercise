﻿using AirportTicketBookingExercise.Domain;
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

                // Handle potential null value
                var classPricesString = row.Row[6];
                if (classPricesString != null)
                {
                    var classes = classPricesString.Split("-");
                    foreach (var classPrice in classes)
                    {
                        // Assuming a CSV format like "Economy:100.00,Business:200.00"
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
                                    // Log or throw an exception as needed
                                    Console.WriteLine($"Error parsing class price pair: {classPricePair}");
                                }
                            }
                            else
                            {
                                // Log or throw an exception as needed
                                Console.WriteLine($"Unexpected format in class price pair: {classPricePair}");
                            }
                        }
                    }
                }

                return classPrices;
            });
        }
    }
}
