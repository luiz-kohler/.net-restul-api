using API.Exceptions;
using API.Handlers;
using API.Infra;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using XAct;

namespace API.Services
{
    public interface IUserService
    {
        Task<IActionResult> Create(CreateUserRequest request);
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Update(int id, UpdateUserRequest request);
        Task<IActionResult> Delete(int id);
        Task<IActionResult> Login(LoginRequest request);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPetRepository _petRepository;
        private readonly IHashHandler _hashHandler;

        public UserService(
            IUserRepository userRepository,
            IPetRepository petRepository,
            IHashHandler hashHandler)
        {
            _userRepository = userRepository;
            _petRepository = petRepository;
            _hashHandler = hashHandler;
        }

        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var isEmailAlreadyRegistered = await _userRepository.ExistsAsync(user => user.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

            if (isEmailAlreadyRegistered)
                throw new ConflictException("This email is already registered.");

            var user = new User 
            {
                Email = request.Email,
                Name = request.Name,
                HashedPassword = _hashHandler.Hash(request.Password),
                CreatedAt = DateTime.UtcNow,
                ETag = Guid.NewGuid().ToString().Replace("-", ""),
            };

            await _userRepository.CreateAsync(user);

            return new CreatedResult();
        }

        public Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();

            var response = users.Map(user => new BasicUserResponse { Id = user.Id, Name = user.Name, Email = user.Email });

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if(user is null)
                throw new NotFoundException("User not found.");

            var pets = await _petRepository.GetAllAsync(pet => pet.UserId == user.Id);

            var response = new DetailedUserResponse 
            { 
                Id = user.Id, 
                Name = user.Name, 
                Email = user.Email, 
                Pets = pets.Map(pet => new BasicPetResponse() 
                { 
                    Id = pet.Id,
                    UserId = user.Id,
                    Age = pet.Age,
                    Name = pet.Name,
                    IsVaccinated = pet.IsVaccinated,
                })  
            };

            return new OkObjectResult(response);
        }

        public Task<IActionResult> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
