using TaskManager;
using TaskManager.Core.Abstractions;
using TaskManager.DataAccess;

public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateUser(string username, string password, string role)
        {
            var existingUser = _userRepository.GetByUsername(username);
            if (existingUser != null)
            {
                throw new ArgumentException("Пользователь с таким именем уже существует.");
            }

            var user = new User
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role
            };

            _userRepository.Add(user);
            Console.WriteLine("Пользователь успешно создан.");
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public User GetUserByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public void UpdateUser(int id, string username, string role)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new ArgumentException("Пользователь не найден.");
            }

            user.Username = username;
            user.Role = role;

            _userRepository.Update(user);
            Console.WriteLine("Пользователь успешно обновлен.");
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                throw new ArgumentException("Пользователь не найден.");
            }

            _userRepository.Delete(user);
            Console.WriteLine("Пользователь успешно удален.");
        }
    }