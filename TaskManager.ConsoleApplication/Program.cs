using TaskManager.DataAccess;
using TaskManager.Services;

class Program
{
    static void Main(string[] args)
    {
        
        var authService = new AuthenticationService(new TaskManagerDbContext());

        Console.WriteLine("1. Register");
        Console.WriteLine("2. Login");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();

        if (option == "1")
        {
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();
            Console.Write("Role (Admin/User): ");
            var role = Console.ReadLine();
            authService.Register(username, password, role);
            Console.WriteLine("User registered successfully.");
        }
        else if (option == "2")
        {
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            var user = authService.Login(username, password);
            if (user != null)
            {
                Console.WriteLine($"Welcome, {user.Username}!");
                if (user.Role == "Admin")
                {
                    Console.WriteLine("Access granted to admin panel.");
                }
                else
                {
                    Console.WriteLine("Access granted to user panel.");
                }
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
            }
        }
    }
}