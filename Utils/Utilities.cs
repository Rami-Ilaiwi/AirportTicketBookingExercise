using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;
using AirportTicketBookingExercise.Services;

namespace AirportTicketBookingExercise.Utils
{
    public static class Utilities
    {
        private static bool HasBookedFlight(int flightNumber)
        {
            List<Booking> userBookings = FileService.ReadCsvFile<Booking>(Files.bookingsFilePath, typeof(BookingMap))
                .Where(booking => booking.UserId == UserService.UserLoggedIn.Id && booking.FlightNumber == flightNumber)
                .ToList();

            return userBookings.Any();
        }

        private static bool ValidateFlightNumber(out int flightNumber)
        {
            Console.Write("Enter the flight number: ");
            var isFlightNumberValid = int.TryParse(Console.ReadLine(), out flightNumber);

            if (!isFlightNumberValid || flightNumber <= 0)
            {
                DisplayService.DisplayWarningMessage("Invalid input. Please enter a valid positive integer for the flight number.");
                return false;
            }

            return true;
        }

        private static bool ValidateFlightClassIndex(Flight flight, out int flightClassIndex)
        {
            Console.Write("Enter the flight class index: ");
            var isFlightClassIndexValid = int.TryParse(Console.ReadLine(), out flightClassIndex);

            if (!isFlightClassIndexValid || flightClassIndex < 1 || flightClassIndex > 3)
            {
                DisplayService.DisplayWarningMessage("Invalid input. Please enter a valid index between 1 and 3 for the flight class.");
                return false;
            }

            return true;
        }

        public static void BookFlight(this User user)
        {
            try
            {
                Console.WriteLine("Select the flight number to book, please!\n");
                DisplayService.ShowAvailableFlightsSummary();

                if (!ValidateFlightNumber(out var flightNumber))
                    return;

                if (!FlightService.DoesFlightExist(flightNumber))
                {
                    DisplayService.DisplayWarningMessage($"Flight number {flightNumber} not found. Please enter a valid flight number.");
                    return;
                }

                if (HasBookedFlight(flightNumber))
                {
                    DisplayService.DisplayWarningMessage($"You have already booked Flight number {flightNumber}. Cannot book the same flight again.");
                    return;
                }

                var flight = FlightService.GetFlight(flightNumber);
                Console.WriteLine($"Select the Flight class for Flight number {flightNumber}, please!");
                DisplayService.ShowFlightClassesPrices(flight);

                if (!ValidateFlightClassIndex(flight, out var flightClassIndex))
                    return;

                var flightClass = FlightService.GetFlightClassByIndex(flight, flightClassIndex);
                user.BookFlight(flight, flightClass, user);
            }
            finally
            {
                Console.ResetColor();
            }
        }

        public static void ModifyBooking(this User user)
        {
            try
            {

                if (BookingService.DoesUserHasBookings(user))
                {
                    Console.WriteLine("Select a booking to update, please!\n");
                    DisplayService.ShowBookingsForUser(user);
                    if (!ValidateFlightNumber(out var flightNumber))
                        return;

                    if (!FlightService.DoesFlightExist(flightNumber))
                    {
                        DisplayService.DisplayWarningMessage($"Flight number {flightNumber} not found. Please enter a valid flight number.");
                        return;
                    }

                    if (!BookingService.DoesFlightBookedForUser(user, flightNumber))
                    {
                        DisplayService.DisplayWarningMessage($"Flight number {flightNumber} is not booked for the UserId {user.Id}.");
                        return;
                    }

                    var flight = FlightService.GetFlight(flightNumber);

                    Console.WriteLine($"Select the Flight class for Flight number {flightNumber} to update your booking, please!");
                    DisplayService.ShowFlightClassesPrices(flight);
                    var booking = BookingService.GetBooking(flightNumber);
                    if (!ValidateFlightClassIndex(flight, out var flightClassIndex))
                        return;
                    var newFlightClass = FlightService.GetFlightClassByIndex(flight, flightClassIndex);
                    user.UpdateBookingRecord(flight, newFlightClass, booking);
                }
                else
                {
                    DisplayService.DisplayWarningMessage($"No bookings found for UserId {user.Id}.");
                }
            }
            finally
            {
                Console.ResetColor();
            }
        }

        public static void CancelBooking(this User user)
        {
            if (BookingService.DoesUserHasBookings(user))
            {
                Console.WriteLine("Select a booking to cancel, please!\n");
                DisplayService.ShowBookingsForUser(user);
                if (!ValidateFlightNumber(out var flightNumber))
                    return;

                if (!FlightService.DoesFlightExist(flightNumber))
                {
                    DisplayService.DisplayWarningMessage($"Flight number {flightNumber} not found. Please enter a valid flight number.");
                    return;
                }

                if (!BookingService.DoesFlightBookedForUser(user, flightNumber))
                {
                    DisplayService.DisplayWarningMessage($"Flight number {flightNumber} is not booked for the UserId {user.Id}.");
                    return;
                }
                var booking = BookingService.GetBooking(flightNumber);
                user.RemoveBookingRecord(booking);
            }
            else
            {
                DisplayService.DisplayWarningMessage($"No bookings found for UserId {user.Id}.");
            }
        }

        public static void ViewPersonalBookings(this User user)
        {
            if (BookingService.DoesUserHasBookings(user))
            {

                DisplayService.ShowBookingsForUser(user);
            }
            else
            {
                DisplayService.DisplayWarningMessage($"No bookings found for UserId {user.Id}.");
            }
        }

        public static void FilterBooking()
        {
            var bookings = BookingService.GetAllBookings();
            DisplayService.ShowFilterBookingsOptions();
            Console.Write("Your selection: ");
            string selection = Console.ReadLine();

            var filteredBookings = User.FilterBookings(bookings, selection);
            DisplayService.ShowFilteredBookings(filteredBookings);
        }

        public static void ProcessBatchUpload()
        {
            Console.Write("Enter the path of the CSV file for batch flight upload: ");
            string filePath = Console.ReadLine();
            if (!FileService.IsValidFilePath(filePath))
            {
                Console.WriteLine("Invalid path.");
                return;
            }

            var flights = FlightService.GetAllFlights();
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
            if (!FileService.IsValidFilePath(fileDirectory))
            {
                Console.WriteLine("Invalid path.");
                return;
            }

            FileService.ImportFlightFile(fileDirectory);
        }
    }
}
