using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;
using AirportTicketBookingExercise.Services;

namespace AirportTicketBookingExercise.Utils
{
    public static class Utilities
    {
        public static bool HasBookedFlight(int flightNumber)
        {
            List<Booking> userBookings = FileService.ReadCsvFile<Booking>(Files.bookingsFilePath, typeof(BookingMap))
                .Where(booking => booking.UserId == UserService.UserLoggedIn.Id && booking.FlightNumber == flightNumber)
                .ToList();

            return userBookings.Any();
        }

        public static void BookFlight(this User user)
        {
            Console.WriteLine("Select your booking to update, please!\n");
            DisplayService.ShowAvailableFlightsSummary();
            var flightNumber = int.TryParse(Console.ReadLine(), out var fn);

            if (HasBookedFlight(fn))
            {
                Console.WriteLine($"You have already booked Flight number {fn}. Cannot book the same flight again.");
                return;
            }

            var flight = FlightService.GetFlight(fn);
            Console.WriteLine($"Select the Flight class for Flight number {fn}, please!");
            DisplayService.ShowFlightClassesPrices(flight);
            var readFlightClass = int.TryParse(Console.ReadLine(), out var fc);
            var flightClass = FlightService.GetFlightClassByIndex(flight, fc);
            user.BookFlight(flight, flightClass, user);
        }

        public static void ModifyBooking(this User user)
        {
            Console.WriteLine("Select a booking to update, please!\n");
            DisplayService.ShowBookingsForUser(user);
            var flightNumber = int.TryParse(Console.ReadLine(), out var fn);
            var flight = FlightService.GetFlight(fn);
            Console.WriteLine($"Select the Flight class for Flight number {fn} to update your booking, please!");
            DisplayService.ShowFlightClassesPrices(flight);
            var booking = FlightService.GetBooking(fn);
            var readFlightClass = int.TryParse(Console.ReadLine(), out var fc);
            var newFlightClass = FlightService.GetFlightClassByIndex(flight, fc);
            user.UpdateBookingRecord(flight, newFlightClass, booking);
        }

        public static void CancelBooking(this User user)
        {
            Console.WriteLine("Select a booking to remove, please!\n");
            DisplayService.ShowBookingsForUser(user);
            var flightNumber = int.TryParse(Console.ReadLine(), out var fn);
            var flight = FlightService.GetFlight(fn);
            var booking = FlightService.GetBooking(fn);
            user.RemoveBookingRecord(booking);
        }

        public static void ViewPersonalBookings(this User user)
        {
            List<Booking> userBookings = User.GetBookings().Where(booking => booking.UserId == user.Id).ToList();

            if (userBookings.Any())
            {
                Console.WriteLine("Here are your bookings!\n");
                DisplayService.ShowBookingsForUser(user);
            }
            else
            {
                Console.WriteLine("There are no bookings to show.");
            }
        }

        public static void FilterBooking()
        {
            var bookings = User.GetBookings();
            DisplayService.ShowFilterBookingsOptions();
            Console.Write("Your selection: ");
            string selection = Console.ReadLine();
            Console.Write("Enter filter value: ");
            string userInput = Console.ReadLine();
            Console.WriteLine();

            var filteredBookings = User.FilterBookings(bookings, selection, userInput);
            DisplayService.ShowFilteredBookings(filteredBookings);
        }

        public static void ProcessBatchUpload()
        {
            Console.Write("Enter the path of the CSV file for batch flight upload: ");
            string filePath = Console.ReadLine();

            List<Flight> flights = FileService.ReadCsvFile<Flight>(filePath, typeof(FlightMap));
            var result = User.BatchFlightUpload(flights);

            if (result.ValidFlights.Any())
            {
                FileService.AppendCsvFlightRecord<Flight>(Files.flightsFilePath, result.ValidFlights);
                Console.WriteLine($"{result.ValidFlights.Count} flights have been added successfully.");
            }

            if (result.InvalidFlights.Any())
            {
                Console.WriteLine($"{result.InvalidFlights.Count} flights have not been added successfully. Validation errors:");

                foreach (var error in result.InvalidFlights)
                {
                    Console.WriteLine($"- Error at line {error.Index + 1}: {error.Error}");
                }
            }
            else
            {
                Console.WriteLine($"All {flights.Count} flights have been added successfully.");
            }
        }

        public static string ConvertDictionaryToString(Dictionary<FlightClass, decimal> classPrices)
        {
            string result = string.Join("-", classPrices.Select(kv => $"{kv.Key};{kv.Value}"));

            return result;
        }

        public static void ExportFlightImportFile()
        {
            Console.Write("Enter file directory to save: ");
            string fileDirectory = Console.ReadLine();

            FileService.ImportFlightFile(fileDirectory);
        }

    }
}
