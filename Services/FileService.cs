using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Utils;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace AirportTicketBookingExercise.Services
{
    public class FileService
    {
        public static void InitializeFiles()
        {
            EnsureDirectoryExists(Files.directory);

            CreateCsvFileIfNotExists<Flight>(Files.flightsFilePath);
            CreateCsvFileIfNotExists<User>(Files.usersFilePath);
            InitializeBookingsCsvFile();
        }
        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        private static CsvConfiguration GetCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true
            };
        }

        private static void CreateCsvFileIfNotExists<T>(string filePath)
        {
            var fullPath = Path.Combine(Files.directory, filePath);

            if (!File.Exists(fullPath))
            {
                using (var writer = new StreamWriter(fullPath))
                using (var csv = new CsvWriter(writer, GetCsvConfiguration()))
                {
                    csv.WriteHeader<T>();
                }
            }
        }

        private static void InitializeBookingsCsvFile()
        {
            var fullPath = Path.Combine(Files.directory, Files.bookingsFilePath);

            using (var writer = new StreamWriter(fullPath))
            using (var csv = new CsvWriter(writer, GetCsvConfiguration()))
            {
                csv.WriteHeader<Booking>();
            }
        }

        public static List<T> ReadCsvFile<T>(string filePath, Type classMapType)
        {
            var configuration = GetCsvConfiguration();
            var fullPath = Path.Combine(Files.directory, filePath);

            using (var reader = new StreamReader(fullPath))
            using (var csv = new CsvReader(reader, configuration))
            {
                csv.Context.RegisterClassMap(classMapType);
                return csv.GetRecords<T>().ToList();
            }
        }
        public static void WriteToCsv<T>(string filePath, List<T> records)
        {
            var fullPath = Path.Combine(Files.directory, filePath);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            using (var writer = new StreamWriter(fullPath, append: true))
            using (var csv = new CsvWriter(writer, config))
            {
                foreach (var record in records)
                {
                    csv.NextRecord();
                    csv.WriteRecord(record);
                }
            }
        }

        public static void UpdateCsvRecord<T>(string filePath, List<T> updatedRecord, Func<T, bool> predicate, Type classMapType)
        {
            var configuration = GetCsvConfiguration();
            var fullPath = Path.Combine(Files.directory, filePath);

            var records = ReadCsvFile<T>(filePath, classMapType);

            var recordToUpdate = records.FirstOrDefault(predicate);

            if (recordToUpdate != null)
            {
                var index = records.IndexOf(recordToUpdate);
                records[index] = updatedRecord.First();

                using (var writer = new StreamWriter(fullPath, append: false))
                using (var csv = new CsvWriter(writer, configuration))
                {
                    csv.WriteHeader<T>();
                    foreach (var record in records)
                    {
                        csv.NextRecord();
                        csv.WriteRecord(record);
                    }
                }
            }
            else
            {
                Console.WriteLine("Record not found for update.");
            }
        }

        public static void ImportFlightFile(string directory)
        {
            var fullPath = Path.Combine(directory, Files.flightsTemplateFilePath);
            Flight flight = new Flight(999, "Lorem ipsum", "Lorem ipsum", new DateTime(2023, 5, 12),
                           "Lorem ipsum", "Lorem ipsum", new Dictionary<FlightClass, decimal>
            {
                { FlightClass.Economy, 800 },
                { FlightClass.Business, 1500 },
                { FlightClass.FirstClass, 2500 }
            });
            string classPricesString = Utilities.ConvertDictionaryToString(flight.ClassPrices);
            string formattedDepartureDate = flight.DepartureDate.ToString("MM/dd/yyyy");

            using (var writer = new StreamWriter(fullPath))
            using (var csv = new CsvWriter(writer, GetCsvConfiguration()))
            {
                csv.WriteHeader<Flight>();
                csv.NextRecord();
                csv.WriteField(flight.FlightNumber);
                csv.WriteField(flight.DepartureCountry);
                csv.WriteField(flight.DestinationCountry);
                csv.WriteField(formattedDepartureDate);
                csv.WriteField(flight.DepartureAirport);
                csv.WriteField(flight.ArrivalAirport);
                csv.WriteField(classPricesString);
            }
        }

        public static void AppendCsvFlightRecord<FlightMap>(string filePath, List<Flight> newRecords)
        {
            var configuration = GetCsvConfiguration();
            var fullPath = Path.Combine(Files.directory, filePath);


            using (var writer = new StreamWriter(fullPath, append: true))
            using (var csv = new CsvWriter(writer, configuration))
            {
                foreach (var record in newRecords)
                {
                    string classPricesString = Utilities.ConvertDictionaryToString(record.ClassPrices);
                    string formattedDepartureDate = record.DepartureDate.ToString("MM/dd/yyyy");
                    csv.NextRecord();
                    csv.WriteField(record.FlightNumber);
                    csv.WriteField(record.DepartureCountry);
                    csv.WriteField(record.DestinationCountry);
                    csv.WriteField(formattedDepartureDate);
                    csv.WriteField(record.DepartureAirport);
                    csv.WriteField(record.ArrivalAirport);
                    csv.WriteField(classPricesString);
                }
            }
        }

        public static void RemoveCsvRecord<T>(string filePath, Func<T, bool> predicate, Type classMapType)
        {
            var configuration = GetCsvConfiguration();
            var fullPath = Path.Combine(Files.directory, filePath);

            var records = ReadCsvFile<T>(filePath, classMapType);

            var recordToRemove = records.FirstOrDefault(predicate);

            if (recordToRemove != null)
            {
                records.Remove(recordToRemove);

                using (var writer = new StreamWriter(fullPath, append: false))
                using (var csv = new CsvWriter(writer, configuration))
                {
                    csv.WriteHeader<T>();
                    foreach (var record in records)
                    {
                        csv.NextRecord();
                        csv.WriteRecord(record);
                    }
                }
            }
            else
            {
                Console.WriteLine("Record not found for removal.");
            }
        }
    }
}
