using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;
using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;

namespace AirportTicketBookingExercise.Services
{
    public class FileService
    {
        private static readonly string directory = @"C:\Users\Rami Ilaiwi\source\repos\AirportTicketBookingExercise\Files\";
        private static readonly string flightsFilePath = "flights.csv";
        private static readonly string usersFilePath = "users.csv";
        private static readonly string bookingsFilePath = "bookings.csv";

        public static void InitializeFiles()
        {
            EnsureDirectoryExists(directory);

            CreateCsvFileIfNotExists<Flight>(flightsFilePath);
            CreateCsvFileIfNotExists<User>(usersFilePath);
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
            string fullPath = Path.Combine(directory, filePath);

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
            string fullPath = Path.Combine(directory, bookingsFilePath);

            using (var writer = new StreamWriter(fullPath))
            using (var csv = new CsvWriter(writer, GetCsvConfiguration()))
            {
                csv.WriteHeader<Booking>();
            }
        }

        public static List<T> ReadCsvFile<T>(string filePath, Type classMapType)
        {
            var configuration = GetCsvConfiguration();
            string fullPath = Path.Combine(directory, filePath);

            using (var reader = new StreamReader(fullPath))
            using (var csv = new CsvReader(reader, configuration))
            {
                csv.Context.RegisterClassMap(classMapType);
                return csv.GetRecords<T>().ToList();
            }
        }
        public static void WriteToCsv<T>(string filePath, List<T> records)
        {
            string fullPath = Path.Combine(directory, filePath);
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
            string fullPath = Path.Combine(directory, filePath);

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

        public static void RemoveCsvRecord<T>(string filePath, Func<T, bool> predicate, Type classMapType)
        {
            var configuration = GetCsvConfiguration();
            string fullPath = Path.Combine(directory, filePath);

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
