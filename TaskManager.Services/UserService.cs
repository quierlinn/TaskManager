using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace TaskManager.Services;

public class UserService
{
    /*private const string UsersFileName = "users.json";
    private List<User> Users { get; set; }

    public UserService()
    {
        if (File.Exists(UsersFileName))
        {
            var json = File.ReadAllText(UsersFileName);
            Users = JsonConvert.DeserializeObject<List<User>>(json);
        }
        else
        {
            Users = new List<User>();
            var admin = new User
            {
                Username = "admin",
                Password = ComputeSha256Hash("admin"),
                Role = "Admin"
            };
            Users.Add(admin);
            SaveUsers();
        }
    }

    public bool Authenticate(string username, string password, out User user)
    {
        user = Users.SingleOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                                          && u.Password == ComputeSha256Hash(password));
        return user != null;
    }

    public void AddUser(string username, string password, string role)
    {
        if (Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            throw new Exception("User already exists");
        }

        var newUser = new User
        {
            Username = username,
            Password = ComputeSha256Hash(password),
            Role = role
        };
        Users.Add(newUser);
        SaveUsers();
    }

    public void SaveUsers()
    {
        var json = JsonConvert.SerializeObject(Users);
        File.WriteAllText(UsersFileName, json);
    }
    private string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Преобразуем в строку
            StringBuilder builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }*/
}