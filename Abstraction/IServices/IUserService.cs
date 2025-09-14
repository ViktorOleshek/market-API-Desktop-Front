using Abstraction.Models;
using Google.Apis.Auth;

namespace Abstraction.IServices;

public interface IUserService
{
    Task<UserModel> AuthenticateAsync(string username, string password);
    Task RegisterUserAsync(UserModel userModel, string password);
    Task<UserModel> GetByUsernameAsync(string username);
    Task<UserModel> FindOrCreateUserByEmailAsync(GoogleJsonWebSignature.Payload payload);
}
