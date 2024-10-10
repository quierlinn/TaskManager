using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore.Design;
using TaskManager.Core.Abstractions;
using TaskManager.Services;


var services = new ServiceCollection();
ConfigureServices(services);
var serviceProvider = services.BuildServiceProvider();

RunApp(serviceProvider);

static void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<TaskManagerDbContext>();
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IProjectService, ProjectService>();
}

static void RunApp(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var projectService = scope.ServiceProvider.GetRequiredService<IProjectService>();

    Console.WriteLine("=== Система Управления Проектами ===");
    Console.WriteLine("1. Войти");
    Console.WriteLine("2. Выйти");
    Console.Write("Выберите опцию: ");
    var option = Console.ReadLine();
    if (option == "1")
    {
        Console.Write("Имя пользователя: ");
        var username = Console.ReadLine();
        Console.Write("Пароль: ");
        var password = ReadPassword();
        var user = authService.Login(username, password);
        if (user != null)
        {
            Console.WriteLine($"\nДобро пожаловать, {user.Username}!");
            if (user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                AdminMenu(userService, projectService);
            }
            else
            {
                UserMenu(projectService);
            }
        }
        else
        {
            Console.WriteLine("Неверное имя пользователя или пароль.");
        }
    }
    else
    {
        Console.WriteLine("Выход из приложения.");
    }

    static void AdminMenu(IUserService userService, IProjectService projectSservice)
    {
        while (true)
        {
            Console.WriteLine("\n=== Админ Меню ===");
            Console.WriteLine("1. Управление Пользователями");
            Console.WriteLine("2. Управление Проектами");
            Console.WriteLine("3. Выйти");
            Console.Write("Выберите опцию: ");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    ManageUsers(userService);
                    break;
                case "2":
                    ManageProjects(projectSservice);
                    break;
                case "3":
                    Console.WriteLine("Возврат в главное меню.");
                    return;
                default:
                    Console.WriteLine("Неверная опция. Попробуйте снова.");
                    break;
            }
        }
    }

    static void UserMenu(IProjectService projectService)
    {
        while (true)
        {
            Console.WriteLine("\n=== Пользовательское Меню ===");
            Console.WriteLine("1. Просмотреть Проекты");
            Console.WriteLine("2. Выйти");
            Console.Write("Выберите опцию: ");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    ViewProjects(projectService);
                    break;
                case "2":
                    Console.WriteLine("Выход в главное меню.");
                    return;
                default:
                    Console.WriteLine("Неверная опция. Попробуйте снова.");
                    break;
            }
        }
    }

    static void ManageProjects(IProjectService projectService)
    {
        Console.WriteLine("\n=== Управление Проектами ===");
        Console.WriteLine("1. Просмотреть все проекты");
        Console.WriteLine("2. Добавить проект");
        Console.WriteLine("3. Обновить проект");
        Console.WriteLine("4. Удалить проект");
        Console.WriteLine("5. Назад");
        Console.Write("Выберите опцию: ");
        var option = Console.ReadLine();
        switch (option)
        {
            case "1":
                var projects = projectService.GetAllProjects();
                Console.WriteLine("\nПроекты:");
                foreach (var project in projects)
                {
                    Console.WriteLine(
                        $"ID: {project.Id}, Название: {project.Name}, Описание: {project.Description}");
                }

                break;
            case "2":
                Console.Write("Введите название проекта: ");
                var name = Console.ReadLine();
                Console.Write("Введите описание проекта: ");
                var description = Console.ReadLine();
                projectService.AddProject(name, description);
                break;
            case "3":
                Console.Write("Введите ID проекта для обновления: ");
                var updateId = int.Parse(Console.ReadLine());
                Console.Write("Введите новое название проекта: ");
                var newName = Console.ReadLine();
                Console.Write("Введите новое описание проекта: ");
                var newDescription = Console.ReadLine();
                projectService.UpdateProject(updateId, newName, newDescription);
                break;
            case "4":
                Console.Write("Введите ID проекта для удаления: ");
                var deleteId = int.Parse(Console.ReadLine());
                projectService.DeleteProject(deleteId);
                break;
            case "5":
                return;
            default:
                Console.WriteLine("Неверная опция.");
                break;
        }
    }

    static void ManageUsers(IUserService userService)
    {
        while (true)
        {
            Console.WriteLine("\n--- Управление Пользователями ---");
            Console.WriteLine("1. Просмотреть всех пользователей");
            Console.WriteLine("2. Добавить пользователя");
            Console.WriteLine("3. Обновить пользователя");
            Console.WriteLine("4. Удалить пользователя");
            Console.WriteLine("5. Возврат");
            Console.Write("Выберите опцию: ");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    var users = userService.GetAllUsers();
                    Console.WriteLine("\nСписок пользователей:");
                    foreach (var user in users)
                    {
                        Console.WriteLine($"ID: {user.Id}, Имя: {user.Username}, Роль: {user.Role}");
                    }

                    break;
                case "2":
                    Console.Write("Имя пользователя: ");
                    var username = Console.ReadLine();
                    Console.Write("Пароль: ");
                    var password = ReadPassword();
                    Console.Write("Роль (Admin/User): ");
                    var role = Console.ReadLine();
                    userService.AddUser(username, password, role);
                    Console.WriteLine("Пользователь добавлен успешно.");
                    break;
                case "3":
                    Console.Write("ID пользователя для обновления: ");
                    if (int.TryParse(Console.ReadLine(), out int updateId))
                    {
                        Console.Write("Новое имя пользователя: ");
                        var newUsername = Console.ReadLine();
                        Console.Write("Новая роль (Admin/User): ");
                        var newRole = Console.ReadLine();
                        userService.UpdateUser(updateId, newUsername, newRole);
                        Console.WriteLine("Пользователь обновлен успешно.");
                    }
                    else
                    {
                        Console.WriteLine("Неверный ID.");
                    }

                    break;
                case "4":
                    Console.Write("ID пользователя для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        userService.DeleteUser(deleteId);
                        Console.WriteLine("Пользователь удален успешно.");
                    }
                    else
                    {
                        Console.WriteLine("Неверный ID.");
                    }

                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Неверная опция. Попробуйте снова.");
                    break;
            }
        }
    }

    static void ViewProjects(IProjectService projectService)
    {
        var projects = projectService.GetAllProjects();
        Console.WriteLine("\nДоступные проекты:");
        foreach (var project in projects)
        {
            Console.WriteLine($"ID: {project.Id}, Название: {project.Name}, Описание: {project.Description}");
        }
    }

    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, (password.Length - 1));
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}

public class BloggingContextFactory : IDesignTimeDbContextFactory<TaskManagerDbContext>
{
    public TaskManagerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaskManagerDbContext>();
        optionsBuilder.UseSqlite("Data Source=TaskManagerDb.db");

        return new TaskManagerDbContext(optionsBuilder.Options);
    }
}