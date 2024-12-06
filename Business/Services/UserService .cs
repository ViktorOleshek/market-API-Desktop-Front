using System;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using Abstraction.IServices;
using Abstraction.Models;
using AutoMapper;

namespace Business.Services
{
    public class UserService : AbstractService<UserModel, IUser>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper, unitOfWork.UserRepository)
        {
        }

        public async Task<UserModel> AuthenticateAsync(string username, string password)
        {
            var user = await this.UnitOfWork.UserRepository.GetByUsernameAsync(username);
            if (user == null || user.Password != password) // Порівнюємо пароль в оригінальному вигляді
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return this.Mapper.Map<UserModel>(user);
        }

        public async Task RegisterUserAsync(UserModel userModel, string password)
        {
            userModel.Password = password;

            await base.AddAsync(userModel);
        }

        public async Task<UserModel> GetByUsernameAsync(string username)
        {
            var user = await this.UnitOfWork.UserRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            return this.Mapper.Map<UserModel>(user);
        }

        // Перевірка для додавання користувача (вже є у базовому класі)
        public override async Task AddAsync(UserModel model)
        {
            // Перевірка чи існує користувач з таким username
            var existingUser = await this.UnitOfWork.UserRepository.GetByUsernameAsync(model.Username);
            if (existingUser != null)
            {
                throw new ArgumentException("Username is already taken.");
            }

            // Додаємо користувача
            await base.AddAsync(model);
        }

        protected override void Validation(UserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                throw new ArgumentException("Username and password are required.");
            }
        }
    }
}
