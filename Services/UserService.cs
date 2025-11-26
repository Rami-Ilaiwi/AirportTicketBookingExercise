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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Login System");

            List<User> users = FileService.ReadCsvFile<User>(Files.usersFilePath, typeof(UserMap));

            UserLoggedIn = Login(users);

            if (UserLoggedIn != null)
            {

                int width = Math.Max(UserLoggedIn.UserName.Length, UserLoggedIn.Type.ToString().Length) + 29;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"╔{new string('═', width)}╗");
                Console.WriteLine($"║  Login successful. Welcome, {UserLoggedIn.UserName}!".PadRight(width) + " ║");
                Console.WriteLine($"║  User Type: {UserLoggedIn.Type}".PadRight(width) + " ║");
                Console.WriteLine($"╚{new string('═', width)}╝");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("╔══════════════════════════╗");
                Console.WriteLine("║   Login failed.          ║");
                Console.WriteLine("║   Invalid credentials.   ║");
                Console.WriteLine("╚══════════════════════════╝");
                Console.ResetColor();
            }
        }

        public static void LogoutUser()
        {
            UserLoggedIn = null;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║   Logged out successfully.   ║");
            Console.WriteLine("╚══════════════════════════════╝");
            Console.ResetColor();
        }
    }
}
