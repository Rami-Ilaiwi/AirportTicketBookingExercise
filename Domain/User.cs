using AirportTicketBookingExercise.Mappers;
using AirportTicketBookingExercise.Services;
using AirportTicketBookingExercise.Utils;
using CsvHelper.Configuration.Attributes;

namespace AirportTicketBookingExercise.Domain
{
    public class User
    {
        [Index(0)]
        public int Id { get; set; }
        [Index(1)]
        public string UserName { get; set; }
        [Index(2)]
        public string Password { get; set; }
        [Index(3)]
        public UserType Type { get; set; }

        public void BookFlight(Flight flight, FlightClass flightClass, User user)
        {
            var booking = new List<Booking>();
            booking.Add(new Booking(user.Id, flight.FlightNumber, flightClass));
            FileService.WriteToCsv(Files.bookingsFilePath, booking);

            DisplayService.DisplaySuccessMessage($"Flight number {flight.FlightNumber} in {flightClass} class booked successfully!");
        }

        public void UpdateBookingRecord(Flight flight, FlightClass flightClass, Booking book)
        {
            var booking = new List<Booking>();
            var predicate = book.GetBookingPredicate();
            booking.Add(new Booking(book.UserId, flight.FlightNumber, flightClass));
            FileService.UpdateCsvRecord(Files.bookingsFilePath, booking, predicate, typeof(BookingMap));

            DisplayService.DisplaySuccessMessage($"Flight number {flight.FlightNumber} has been updated to {flightClass} class successfully!");
        }

        public void RemoveBookingRecord(Booking book)
        {
            var predicate = book.GetBookingPredicate();
            FileService.RemoveCsvRecord<Booking>(Files.bookingsFilePath, predicate, typeof(BookingMap));
            DisplayService.DisplaySuccessMessage($"Flight number {book.FlightNumber} booking has been canceled successfully!");
        }

        public static List<Booking> FilterByFlightNumber(List<Booking> bookings, string userInput)
        {
            if (!int.TryParse(userInput, out int fn))
            {
                DisplayService.DisplayErrorMessage("Invalid flight number value.");
                return new List<Booking>();
            }

            return bookings.Where(b => b.FlightNumber == fn).ToList();
        }

        public static List<Booking> FilterByMaxPrice(List<Booking> bookings, string userInput)
        {
            if (!int.TryParse(userInput, out int maxPrice))
            {
                DisplayService.DisplayErrorMessage("Invalid max price value.");
                return new List<Booking>();
            }

            return bookings
                .Where(b =>
                {
                    Flight flight = FlightService.GetFlight(b.FlightNumber);
                    if (flight != null)
                    {
                        decimal minClassPrice = flight.ClassPrices.Values.Min();
                        return minClassPrice <= maxPrice;
                    }
                    return false;
                })
                .ToList();
        }
        public static List<Booking> FilterByDepartureCountry(List<Booking> bookings, string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
            {
                DisplayService.DisplayErrorMessage("Your value is null or empty.");
                return new List<Booking>();
            }

            string departureCountry = userInput;
            return bookings.Where(b => FlightService.GetFlight(b.FlightNumber).DepartureCountry.Equals(userInput, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        public static List<Booking> FilterByDestinationCountry(List<Booking> bookings, string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
            {
                DisplayService.DisplayErrorMessage("Your value is null or empty.");
                return new List<Booking>();
            }

            string destinationCountry = userInput;
            return bookings.Where(b => FlightService.GetFlight(b.FlightNumber).DestinationCountry.Equals(userInput, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<Booking> FilterByDepartureDate(List<Booking> bookings, string userInput)
        {
            if (!DateTime.TryParse(userInput, out DateTime departureDate))
            {
                DisplayService.DisplayErrorMessage("Invalid departure date value.");
                return new List<Booking>();
            }

            return bookings
                .Where(b =>
                {
                    Flight flight = FlightService.GetFlight(b.FlightNumber);
                    return flight != null && flight.DepartureDate.ToString("MM/dd/yyyy") == departureDate.ToString("MM/dd/yyyy");
                })
                .ToList();
        }

        public static List<Booking> FilterByDepartureAirport(List<Booking> bookings, string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
            {
                DisplayService.DisplayErrorMessage("Your value is null or empty.");
                return new List<Booking>();
            }

            return bookings.Where(b => FlightService.GetFlight(b.FlightNumber).DepartureAirport.Equals(userInput, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<Booking> FilterByArrivalAirport(List<Booking> bookings, string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
            {
                DisplayService.DisplayErrorMessage("Your value is null or empty.");
                return new List<Booking>();
            }

            return bookings.Where(b => FlightService.GetFlight(b.FlightNumber).ArrivalAirport.Equals(userInput, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static List<Booking> FilterByUserName(List<Booking> bookings, string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
            {
                DisplayService.DisplayErrorMessage("Your value is null or empty.");
                return new List<Booking>();
            }

            var users = FileService.ReadCsvFile<User>(Files.usersFilePath, typeof(UserMap));
            var user = users.FirstOrDefault(u => u.UserName.Equals(userInput, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                return bookings.Where(b => b.UserId == user.Id).ToList();
            }

            return new List<Booking>();
        }

        public static List<Booking> FilterByFlightClass(List<Booking> bookings, string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
            {
                DisplayService.DisplayErrorMessage("Your value is null or empty.");
                return new List<Booking>();
            }

            return bookings
                .Where(b => b.FlightClass.ToString().Equals(userInput, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public static List<Booking> FilterBookings(List<Booking> bookings, string filterOption)
        {
            List<Booking> filteredBookings;

            switch (filterOption)
            {
                case "1":
                    Console.Write("Enter Flight Number filter value: ");
                    filteredBookings = FilterByFlightNumber(bookings, Console.ReadLine());
                    break;
                case "2":
                    Console.Write("Enter max Flight Price filter value: ");
                    filteredBookings = FilterByMaxPrice(bookings, Console.ReadLine());
                    break;
                case "3":
                    Console.Write("Enter Departure Country filter value: ");
                    filteredBookings = FilterByDepartureCountry(bookings, Console.ReadLine());
                    break;
                case "4":
                    Console.Write("Enter Destination Country filter value: ");
                    filteredBookings = FilterByDestinationCountry(bookings, Console.ReadLine());
                    break;
                case "5":
                    Console.Write("Enter Departure Date filter value: ");
                    filteredBookings = FilterByDepartureDate(bookings, Console.ReadLine());
                    break;
                case "6":
                    Console.Write("Enter Departure Airport filter value: ");
                    filteredBookings = FilterByDepartureAirport(bookings, Console.ReadLine());
                    break;
                case "7":
                    Console.Write("Enter Arrival Airport filter value: ");
                    filteredBookings = FilterByArrivalAirport(bookings, Console.ReadLine());
                    break;
                case "8":
                    Console.Write("Enter Passenger name filter value: ");
                    filteredBookings = FilterByUserName(bookings, Console.ReadLine());
                    break;
                case "9":
                    Console.Write("Enter Flight class filter value: ");
                    filteredBookings = FilterByFlightClass(bookings, Console.ReadLine());
                    break;
                default:
                    DisplayService.DisplayErrorMessage("Invalid filter option.");
                    filteredBookings = new List<Booking>();
                    break;
            }

            return filteredBookings;
        }

        public static (List<Flight> ValidFlights, List<(int Index, string Error)> InvalidFlights) BatchFlightUpload(List<Flight> flights)
        {
            var validationErrors = FlightImportValidator.ValidateFlightData(flights);

            return (flights.Where((_, index) => !validationErrors.Any(error => error.Index == index)).ToList(), validationErrors);
        }

        public static void ExportFlightImportTemplate()
        {
            Utilities.ExportFlightImportFile();
        }
    }
}
