using AirportTicketBookingExercise.Domain;
using AirportTicketBookingExercise.Mappers;

namespace AirportTicketBookingExercise.Services
{
    public class UserService
    {
        private static readonly string usersFilePath = "users.csv";

        public static User UserLoggedIn { get; private set; }

        public static User Login(List<User> users)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            // Find user in the list with the provided username and password
            User user = users.FirstOrDefault(u => u.UserName == username && u.Password == password);

            return user;
        }

        public static void RunApplication()
        {
            Console.WriteLine("Login System");

            List<User> users = FileService.ReadCsvFile<User>(usersFilePath, typeof(UserMap));

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
