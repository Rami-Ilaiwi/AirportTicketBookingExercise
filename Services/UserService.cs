using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;
using AirportTicketBookingExercise.Utils;

namespace AirportTicketBookingExercise.Services
{
    public class UserService
    {
        public static User UserLoggedIn { get; private set; }

        public static User Login(List<User> users)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = GetHiddenConsoleInput();

            User user = users.FirstOrDefault(u => u.UserName == username && u.Password == password);

            return user;
        }

        private static string GetHiddenConsoleInput()
        {
            string input = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Enter)
                {
                    input += key.KeyChar;
                    Console.Write("*");
                }

            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return input;
        }

        public static void RunApplication()
        {
            Console.WriteLine("Login System");

            List<User> users = FileService.ReadCsvFile<User>(Files.usersFilePath, typeof(UserMap));

            UserLoggedIn = Login(users);

            if (UserLoggedIn != null)
            {
                Console.WriteLine($"Login successful. Welcome, {UserLoggedIn.UserName}!");
                Console.WriteLine($"User Type: {UserLoggedIn.Type}");
            }
            else
            {
                Console.WriteLine("Login failed. Invalid credentials.");
            }
        }

        public static void LogoutUser()
        {
            UserLoggedIn = null;
            Console.WriteLine("Logged out successfully.");
        }
    }
}
