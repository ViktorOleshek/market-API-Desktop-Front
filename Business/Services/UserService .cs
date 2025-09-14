using Abstraction.Entities;
using Abstraction.IRepositories;
using Abstraction.IServices;
using Abstraction.Models;
using AutoMapper;
using Google.Apis.Auth;
using System;
using System.Threading.Tasks;

namespace Business.Services;

public class UserService
    : AbstractService<UserModel, User>, IUserService
{
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper, unitOfWork.UserRepository)
    {
    }

    public async Task<UserModel> AuthenticateAsync(string username, string password)
    {
        var user = await this.UnitOfWork.UserRepository.GetByUsernameAsync(username);
        if (user == null || user.Password != password)
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

    public override async Task AddAsync(UserModel model)
    {
        var existingUser = await this.UnitOfWork.UserRepository.GetByUsernameAsync(model.Username);
        if (existingUser != null)
        {
            throw new ArgumentException("Username is already taken.");
        }

        // Перевіряємо email якщо він є
        if (!string.IsNullOrWhiteSpace(model.Email))
        {
            var existingEmailUser = await this.UnitOfWork.UserRepository.GetByEmailAsync(model.Email);
            if (existingEmailUser != null)
            {
                throw new ArgumentException("Email is already taken.");
            }
        }

        await base.AddAsync(model);
    }

    protected override void Validation(UserModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Username))
        {
            throw new ArgumentException("Username is required.");
        }

        if (string.IsNullOrWhiteSpace(model.Password) && !string.IsNullOrWhiteSpace(model.Email))
        {
            throw new ArgumentException("Password is required for regular users.");
        }
    }

    public async Task<UserModel> FindOrCreateUserByEmailAsync(GoogleJsonWebSignature.Payload payload)
    {
        var existingUser = await this.UnitOfWork.UserRepository.GetByEmailAsync(payload.Email);

        if (existingUser != null)
        {
            return this.Mapper.Map<UserModel>(existingUser);
        }

        var newPerson = new Person
        {
            Name = payload.GivenName,
            Surname = payload.FamilyName,
            BirthDate = DateTime.MinValue,
        };
        await this.UnitOfWork.PersonRepository.AddAsync(newPerson);
        await this.UnitOfWork.SaveAsync();


        var newCustomer = new Customer
        {
            PersonId = newPerson.Id,
            DiscountValue = 0,
        };
        await this.UnitOfWork.CustomerRepository.AddAsync(newCustomer);

        var newUser = new UserModel
        {
            Username = payload.Name,
            Email = payload.Email,
            Password = null,
            Role = "User",
            PersonId = newPerson.Id,
        };
        await this.AddGoogleUserAsync(newUser);
        await this.UnitOfWork.SaveAsync();

        return newUser;
    }

    private async Task AddGoogleUserAsync(UserModel model)
    {
        var entity = this.Mapper.Map<User>(model);
        await this.UnitOfWork.UserRepository.AddAsync(entity);
        await this.UnitOfWork.SaveAsync();

        model.Id = entity.Id;
    }
}
