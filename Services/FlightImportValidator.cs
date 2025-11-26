using AirportTicketBookingExercise.Domain;

namespace AirportTicketBookingExercise.Services
{
    public class FlightImportValidator
    {
        public static List<(int Index, string Error)> ValidateFlightData(List<Flight> flights)
        {
            List<(int Index, string Error)> errors = new List<(int Index, string Error)>();

            for (int i = 0; i < flights.Count; i++)
            {
                if (!ValidateFlightNumber(flights[i].FlightNumber, FlightService.GetAllFlights().Select(f => f.FlightNumber).ToList(), errors, i)) continue;
                if (!ValidateDepartureCountry(flights[i].DepartureCountry, errors, i)) continue;
                if (!ValidateDestinationCountry(flights[i].DestinationCountry, errors, i)) continue;
                if (!ValidateDepartureDate(flights[i].DepartureDate, errors, i)) continue;
                if (!ValidateDepartureAirport(flights[i].DepartureAirport, errors, i)) continue;
                if (!ValidateArrivalAirport(flights[i].ArrivalAirport, errors, i)) continue;
                if (!ValidateClassPrices(flights[i].ClassPrices, errors, i)) continue;
            }

            return errors;
        }
  
        private static bool ValidateFlightNumber(int flightNumber, List<int> existingFlightNumbers, List<(int Index, string Error)> errors, int index)
        {
            if (string.IsNullOrWhiteSpace(flightNumber.ToString()))
            {
                errors.Add((index, "Flight number is required."));
            }
            else if (existingFlightNumbers.Contains(flightNumber))
            {
                errors.Add((index, $"Flight with number {flightNumber} already exists."));
                return false;

            }
            return true;
        }

        private static bool ValidateDepartureCountry(string departureCountry, List<(int Index, string Error)> errors, int index)
        {
            if (string.IsNullOrWhiteSpace(departureCountry))
            {
                errors.Add((index, "Departure Country is required."));
                return false;
            }
            return true;
        }

        private static bool ValidateDestinationCountry(string destinationCountry, List<(int Index, string Error)> errors, int index)
        {
            if (string.IsNullOrWhiteSpace(destinationCountry))
            {
                errors.Add((index, "Destination Country is required."));
                return false;
            }
            return true;
        }

        private static bool ValidateDepartureDate(DateTime departureDate, List<(int Index, string Error)> errors, int index)
        {
            if (string.IsNullOrWhiteSpace(departureDate.ToString()))
            {
                errors.Add((index, "Flight number is required."));
            }
            else if (departureDate < DateTime.Today)
            {
                errors.Add((index, "Departure Date should be today or in the future."));
                return false;

            }
            return true;

        }

        private static bool ValidateDepartureAirport(string departureAirport, List<(int Index, string Error)> errors, int index)
        {
            if (string.IsNullOrWhiteSpace(departureAirport))
            {
                errors.Add((index, "Departure Airport is required."));
                return false;
            }
            return true;
        }

        private static bool ValidateArrivalAirport(string arrivalAirport, List<(int Index, string Error)> errors, int index)
        {
            if (string.IsNullOrWhiteSpace(arrivalAirport))
            {
                errors.Add((index, "Arrival Airport is required."));
                return false;
            }
            return true;
        }

        private static bool ValidateClassPrices(Dictionary<FlightClass, decimal> classPrices, List<(int Index, string Error)> errors, int index)
        {
            if (classPrices.Count == 3 &&
                classPrices.ContainsKey(FlightClass.Economy) &&
                classPrices.ContainsKey(FlightClass.Business) &&
                classPrices.ContainsKey(FlightClass.FirstClass))
            {
                if (classPrices.All(pair => pair.Value > 0))
                {
                    if (classPrices[FlightClass.Economy] < classPrices[FlightClass.Business] &&
                        classPrices[FlightClass.Business] < classPrices[FlightClass.FirstClass])
                    {
                        return true;
                    }
                    else
                    {
                        errors.Add((index, "Prices are not in the specified order."));
                    }
                }
                else
                {
                    errors.Add((index, "All class prices must have a positive value."));
                }
            }
            else
            {
                errors.Add((index, "Missing or incorrect number of class prices."));
            }

            return false;
        }

    }
}
