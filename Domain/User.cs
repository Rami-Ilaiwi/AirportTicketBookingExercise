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
            List<Booking> booking = new List<Booking>();
            booking.Add(new Booking(user.Id, flight.FlightNumber, flightClass));
            FileService.WriteToCsv(Files.bookingsFilePath, booking);
        }

        public void UpdateBookingRecord(Flight flight, FlightClass flightClass, Booking book)
        {
            List<Booking> booking = new List<Booking>();
            booking.Add(new Booking(book.UserId, flight.FlightNumber, flightClass));
            FileService.UpdateCsvRecord(Files.bookingsFilePath, booking, x => x.UserId == book.UserId && x.FlightNumber == book.FlightNumber, typeof(BookingMap));
        }

        public void RemoveBookingRecord(Booking book)
        {
            FileService.RemoveCsvRecord<Booking>(Files.bookingsFilePath, x => x.UserId == book.UserId && x.FlightNumber == book.FlightNumber, typeof(BookingMap));
        }

        public static List<Booking> GetBookings()
        {
            List<Booking> bookings = FileService.ReadCsvFile<Booking>(Files.bookingsFilePath, typeof(BookingMap));
            return bookings;
        }

        public static List<Booking> FilterBookings(List<Booking> bookings, string filterOption, string userInput)
        {
            List<Booking> filteredBookings;
            var flights = FlightService.GetAllFlights();

            switch (filterOption)
            {
                case "1":
                    int flightNumber = int.Parse(userInput);
                    filteredBookings = bookings.Where(b => b.FlightNumber == flightNumber).ToList();
                    break;

                case "2":
                    int maxPrice = int.Parse(userInput);
                    filteredBookings = bookings
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
                                    .ToList(); break;

                case "3":
                    string departureCountry = userInput;
                    filteredBookings = bookings.Where(b => FlightService.GetFlight(b.FlightNumber).DepartureCountry == departureCountry).ToList();
                    break;
                case "4":
                    string destinationCountry = userInput;
                    filteredBookings = bookings.Where(b => FlightService.GetFlight(b.FlightNumber).DestinationCountry == destinationCountry).ToList();
                    break;
                case "5":
                    string departureDate = userInput;
                    filteredBookings = bookings
                                    .Where(b =>
                                    {
                                        Flight flight = FlightService.GetFlight(b.FlightNumber);
                                        return flight != null && flight.DepartureDate.ToString("MM/dd/yyyy") == departureDate;
                                    })
                                    .ToList(); break;
                case "6":
                    string departureAirport = userInput;
                    filteredBookings = bookings.Where(b => FlightService.GetFlight(b.FlightNumber).DepartureAirport == departureAirport).ToList();
                    break;
                case "7":
                    string arrivalAirport = userInput;
                    filteredBookings = bookings.Where(b => FlightService.GetFlight(b.FlightNumber).ArrivalAirport == arrivalAirport).ToList();
                    break;
                case "8":
                    string passenger = userInput;
                    List<User> users = FileService.ReadCsvFile<User>("users.csv", typeof(UserMap));
                    User user = users.FirstOrDefault(u => u.UserName == passenger);

                    filteredBookings = bookings.Where(b => b.UserId == user.Id).ToList();

                    break;
                case "9":
                    string flightClass = userInput;
                    filteredBookings = bookings
                        .Where(b => b.FlightClass.ToString().Equals(flightClass, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;

                default:
                    Console.WriteLine("Invalid filter option.");
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
