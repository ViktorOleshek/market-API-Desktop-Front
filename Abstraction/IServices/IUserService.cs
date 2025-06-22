using Abstraction.Models;

namespace Abstraction.IServices;

public interface IUserService
{
    Task<UserModel> AuthenticateAsync(string username, string password); // Для автентифікації
    Task RegisterUserAsync(UserModel userModel, string password);        // Для реєстрації
    Task<UserModel> GetByUsernameAsync(string username);                // Для отримання користувача за username
}
